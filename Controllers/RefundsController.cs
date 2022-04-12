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
            return await _context.Refund.ToListAsync();
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
        [HttpPut("{id}"),Authorize]
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

            return NoContent();
        }

        // POST: api/Refunds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
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
                
                _producer.SendMessage(new RefundMessage
                {
                    Id = redundNew.Entity.Id,
                    Reason = redundNew.Entity.Reason,
                    Status = redundNew.Entity.Status,
                    CreatedAt = redundNew.Entity.CreatedAt,
                    UpdatedAt = redundNew.Entity.UpdatedAt,
                    File = redundNew.Entity.File
                },"New-Refund");
               
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction("GetRefund", new { id = redundNew.Entity.Id });
        }

        // DELETE: api/Refunds/5
        [HttpDelete("{id}")]
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
