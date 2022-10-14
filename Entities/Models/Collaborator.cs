using System;
using System.Collections.Generic;

namespace MicroBeard.Entities.Models
{
    public partial class Collaborator
    {
        public Collaborator()
        {
            Licenses = new HashSet<LicencedCollaborator>();
            Services = new HashSet<CollaboratorService>();
        }

        public int Code { get; set; }
        public string Name { get; set; } = null!;
        public string? Cpf { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Phone { get; set; }
        public string? Function { get; set; }
        public decimal? Salary { get; set; }
        public decimal? Commision { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? DesactivationDate { get; set; }
        public int? DesactivatorCode { get; set; }
        public bool? Desactivated { get; set; }

        public virtual ICollection<LicencedCollaborator> Licenses { get; set; }
        public virtual ICollection<CollaboratorService> Services { get; set; }

    }
}
