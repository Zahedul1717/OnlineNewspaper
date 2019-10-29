using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Added
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineNewspaper.Models;

namespace OnlineNewspaper.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<OnlineNewspaper.Models.NewsCategory> NewsCategory { get; set; }

        public DbSet<OnlineNewspaper.Models.NewsDetails> NewsDetails { get; set; }
    }
}
