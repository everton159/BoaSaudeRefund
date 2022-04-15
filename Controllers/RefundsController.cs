using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoaSaudeRefund;
using BoaSaudeRefund.Data;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using BoaSaudeRefund.Infra;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace BoaSaudeRefund.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundsController : ControllerBase
    {
        private readonly BoaSaudeRefundContext _context;
        private readonly RabbitMQProducer _producer;

        public RefundsController(BoaSaudeRefundContext context)
        {
            _context = context;
            _producer = new RabbitMQProducer();
        }

        // GET: api/Refunds
        [HttpGet,Authorize]
        public async Task<ActionResult<IEnumerable<Refund>>> GetRefund()
        {
            string token = HttpContext.Request.Headers.First(x => x.Key == "Authorization").Value;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var role = jwtSecurityToken.Claims.First(c => c.Type.Contains("role")).Value;
            var user = jwtSecurityToken.Claims.First(c => c.Type.Contains("name")).Value;


            if (role == "Prestador")
                return await _context.Refund.ToListAsync();
            else
                return await _context.Refund.Where(x => x.User== user).ToListAsync();
        }


        

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Refund>> GetRefund(long id)
        {
            //var refund = await _context.Refund.FindAsync(id);
            var refund = await _context.Refund.FindAsync(id);

            if (refund == null)
            {
                return NotFound();
            }

            return refund;
        }

        [HttpGet("user/{id}"), Authorize]
        public async Task<ActionResult<IEnumerable<Refund>>> GetRefundByUser(string user)
        {
            var refunds = await _context.Refund.Where(r => r.User == user).ToListAsync();

            if (refunds == null)
            {
                return NotFound();
            }

            return refunds;
        }

        // PUT: api/Refunds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Prestador")]
        public async Task<IActionResult> PutRefund(long id, Refund refund)
        {
            if (id != refund.Id)
            {
                return BadRequest();
            }

            _context.Entry(refund).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RefundExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return Ok(refund);
        }

        // POST: api/Refunds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //
        [Authorize(Roles = "Associado")]
        public async Task<ActionResult<Refund>> PostRefund([FromForm] RefundCreateModel refund)
        {

            byte[] file;
            using (var ms = new MemoryStream())
            {
                refund.File.CopyTo(ms);
                file = ms.ToArray();
                //string s = Convert.ToBase64String(fileBytes);
                // act on the Base64 data
            }
            
            var redundNew =_context.Refund.Add(
                    new Refund
                    {
                        Reason = refund.Reason,
                        Status = "Novo",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        File = file
                    }
            );
            try
            {
              var newRefund = await _context.SaveChangesAsync();
                
                _producer.SendMessage(

                    JsonConvert.SerializeObject(
                        new RefundMessage
                        {
                            Reason = refund.Reason,
                            Status = "Novo",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            File = file
                        }
                    )

                    , "New-Refund");
               
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction("GetRefund", new { id = redundNew.Entity.Id });
        }

        // DELETE: api/Refunds/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRefund(long id)
        {
            var refund = await _context.Refund.FindAsync(id);
            if (refund == null)
            {
                return NotFound();
            }
            
            _context.Refund.Remove(refund);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RefundExists(long id)
        {
            return _context.Refund.Any(e => e.Id == id);
        }
    }

    internal class RefundMessage
    {
        public long Id { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public byte[] File { get; set; }
    }
}
