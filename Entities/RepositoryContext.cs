using Entities.Models;
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

        public DbSet<Contact>? Contacts { get; set; }
        public DbSet<Scheduling>? Schedulings { get; set; }
        public DbSet<Service>? Services { get; set; }
        public DbSet<Collaborator>? Collaborators { get; set; }
        public DbSet<License>? Licenses { get; set; }

    }
}
