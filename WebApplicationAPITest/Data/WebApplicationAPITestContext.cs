using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPITest;

namespace WebApplicationAPITest.Data
{
    public class WebApplicationAPITestContext : DbContext
    {
        public WebApplicationAPITestContext (DbContextOptions<WebApplicationAPITestContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<WebApplicationAPITest.Users> Users { get; set; } = default!;
    }
}
