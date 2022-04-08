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

namespace BoaSaudeRefund.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundsController : ControllerBase
    {
        private readonly BoaSaudeRefundContext _context;

        public RefundsController(BoaSaudeRefundContext context)
        {
            _context = context;
        }

        // GET: api/Refunds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Refund>>> GetRefund()
        {
            return await _context.Refund.ToListAsync();
        }

        // GET: api/Refunds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Refund>> GetRefund(long id)
        {
            var refund = await _context.Refund.FindAsync(id);

            if (refund == null)
            {
                return NotFound();
            }

            return refund;
        }

        // PUT: api/Refunds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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




        //public static async Task<byte[]> GetBytes(this IFormFile formFile)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await formFile.CopyToAsync(memoryStream);
        //        return memoryStream.ToArray();
        //    }
        //}
    }
}
