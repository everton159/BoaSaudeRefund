using BoaSaudeRefund.Data;
using BoaSaudeRefund.DTO;
using BoaSaudeRefund.Enum;
using BoaSaudeRefund.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BoaSaudeRefund.Controllers
{
    [Route("refund")]
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


        [HttpGet]
        [Authorize(Roles = "Prestador")]
        public async Task<ActionResult<IEnumerable<Refund>>> GetRefund()
        {
            var refunds = await _context.Refund.ToListAsync();
            if (refunds == null)
                return NotFound();
            else
                return Ok(refunds);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Prestador")]
        public async Task<ActionResult<Refund>> GetRefund(long id)
        {
            var refund = await _context.Refund.FindAsync(id);
            if (refund == null)
                return NotFound();
            return Ok(refund);
        }

        [HttpGet("user/{id}")]
        [Authorize(Roles = "Associado")]
        public async Task<ActionResult<IEnumerable<Refund>>> GetRefundByUser(string id)
        {
            var refunds = await _context.Refund.Where(r => r.UserId == id).ToListAsync();
            if (refunds == null)
                return NotFound();
            return Ok(refunds);
        }

        [HttpPost]
        [Authorize(Roles = "Associado")]
        public async Task<ActionResult<RefundRegisterDto>> PostRefund(RefundRegisterDto model)
        {
            var modelToEntity = GenerateRefundByRefundCreateDto(model);
            var refundNew = _context.Refund.Add(modelToEntity);

            try
            {
                var newRefundId = await _context.SaveChangesAsync();
                var refundmessage = GenerateRefundMessageByRefund(refundNew.Entity);
                _producer.SendMessage(JsonConvert.SerializeObject(refundmessage), "New-Refund");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction("GetRefund", new { id = refundNew.Entity.Id });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Prestador")]
        public async Task<IActionResult> PutRefund(long id, RefundEditDto model)
        {
            if (id != model.Id)
                return BadRequest();

            try
            {
                var userInfo = GetTokenUserInfo();
                var entityDb = await _context.Refund.FindAsync(id);
                if (entityDb != null)
                {
                    entityDb.Status = model.Status;
                    entityDb.UpdatedAt = DateTime.Now;
                    entityDb.UserNameAnalyst = userInfo.UserName;
                    _context.Entry(entityDb).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!RefundExists(id))
                    return NotFound();
                else
                    return BadRequest(e.Message);
            }

            return Ok(model);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRefund(long id)
        {
            var refund = await _context.Refund.FindAsync(id);
            if (refund == null)
                return NotFound();

            _context.Refund.Remove(refund);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #region Auxiliary Methods
        private bool RefundExists(long id)
        {
            return _context.Refund.Any(e => e.Id == id);
        }

        private TokenUserInfo GetTokenUserInfo()
        {
            string token = HttpContext.Request.Headers.First(x => x.Key == "Authorization").Value;
            token = token.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var userId = jwtSecurityToken.Claims.First(c => c.Type.Contains("sub")).Value;
            var role = jwtSecurityToken.Claims.First(c => c.Type.Contains("role")).Value;
            var user = jwtSecurityToken.Claims.First(c => c.Type.Contains("name")).Value;

            return new TokenUserInfo { UserId = userId, UserName = user, Role = role };
        }

        private Refund GenerateRefundByRefundCreateDto(RefundRegisterDto dto)
        {
            var refund = new Refund
            {
                Type = dto.Type,
                Description = dto.Description,
                Date = DateTime.ParseExact(dto.Date, "dd/MM/yyyy", null),
                NFeNumber = dto.NFeNumber,
                NFeLink = dto.NFeLink,
                Price = Double.Parse(dto.Price),
                CNPJProvider = dto.CNPJProvider,
                UserId = GetTokenUserInfo().UserId,
                CreatedAt = DateTime.Now,
                Status = EnumStateRefund.Novo
            };

            return refund;
        }

        private RefundMessage GenerateRefundMessageByRefund(Refund refundDb)
        {
            var refundMessage = new RefundMessage
            {
                Id = refundDb.Id,
                User = refundDb.UserId,
                Date = refundDb.Date,
                Price = refundDb.Price,
                NFeNumber = refundDb.NFeNumber,
                NFeLink = refundDb.NFeLink,
                Status = refundDb.Status.ToString(),
                CreatedAt = refundDb.CreatedAt,
            };

            return refundMessage;
        }
        #endregion
    }

}
