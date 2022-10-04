using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBeard.Entities.Models
{
    public class Collaborator
    {
        [Key]
        public int Code { get; set; }
        public string? Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? CPF { get; set; }
        public int? Phone { get; set; }
        public string? Function { get; set; }
        public decimal? Salary { get; set; }
        public decimal? Commision { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DesactivatorCode { get; set; }
        public DateTime? DesactivationDate { get; set; }
        public bool Desactivated { get; set; }

        public ICollection<Service>? Services { get; set; }
        public ICollection<License>? Licenses { get; set; }

    }
}
