using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoaSaudeRefund;
using BoaSaudeRefund.Data;

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
        public async Task<ActionResult<Refund>> GetRefund(string id)
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
        public async Task<IActionResult> PutRefund(string id, Refund refund)
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
        public async Task<ActionResult<Refund>> PostRefund(Refund refund)
        {
            _context.Refund.Add(refund);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RefundExists(refund.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRefund", new { id = refund.Id }, refund);
        }

        // DELETE: api/Refunds/5
   
        private bool RefundExists(string id)
        {
            return _context.Refund.Any(e => e.Id == id);
        }
    }
}
