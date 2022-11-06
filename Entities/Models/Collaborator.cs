using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroBeard.Entities.Models
{
    public partial class Collaborator
    {
        public Collaborator()
        {
            Licenses = new HashSet<License>();
            Services = new HashSet<Service>();
            Schedulings = new HashSet<Scheduling>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }
        public string Name { get; set; } = null!;
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSaltGUID { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Phone { get; set; }
        public string Function { get; set; }
        public decimal? Salary { get; set; }
        public decimal? Commision { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? DesactivationDate { get; set; }
        public int? DesactivatorCode { get; set; }
        public bool? Desactivated { get; set; }

        public string Token { get; set; }
        public bool? IsAdmin { get; set; }

        public virtual ICollection<License> Licenses { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Scheduling> Schedulings { get; set; }
    }
}
