using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BoaSaudeRefund;

namespace BoaSaudeRefund.Data
{
    public class BoaSaudeRefundContext : DbContext
    {
        public BoaSaudeRefundContext (DbContextOptions<BoaSaudeRefundContext> options)
            : base(options)
        {
            Database.EnsureCreated();

       }

        public DbSet<BoaSaudeRefund.Refund> Refund { get; set; }
    }
}
