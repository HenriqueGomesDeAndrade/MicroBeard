using MicroBeard.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Contact>? Contact { get; set; }
        public DbSet<Scheduling>? Scheduling { get; set; }
        public DbSet<Service>? Service { get; set; }
        public DbSet<Collaborator>? Collaborator { get; set; }
        public DbSet<License>? License { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collaborator>()
                .Property(p => p.Commision)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Collaborator>()
                .Property(p => p.Salary)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Service>()
                .Property(p => p.Price)
                .HasPrecision(9, 2);
        }
    }
}
