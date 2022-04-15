using Microsoft.EntityFrameworkCore;

namespace BoaSaudeRefund.Data
{
    public class BoaSaudeRefundContext : DbContext
    {
        public BoaSaudeRefundContext(DbContextOptions<BoaSaudeRefundContext> options)
            : base(options)
        {
            Database.EnsureCreated();

        }

        public DbSet<BoaSaudeRefund.Refund> Refund { get; set; }
    }
}
