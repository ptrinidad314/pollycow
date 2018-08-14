using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollingApp.Entities
{
    public class PollDataContext:DbContext
    {
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<AccountRequest> AccountRequests { get; set; }

        public PollDataContext(DbContextOptions<PollDataContext> options):base(options)
        {
            Database.Migrate();
        }
    }
}
