using DW.EnterpriseAPI.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DW.EnterpriseAPI.Persistence
{
    //public class EnterpriseContext : DbContext
    public class EnterpriseContext : IdentityDbContext
    {
        public EnterpriseContext()
        {
        }

        public EnterpriseContext(DbContextOptions<EnterpriseContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=DWEnterpriseAPI008;" +
               "Trusted_Connection=True;");
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

    }
}
