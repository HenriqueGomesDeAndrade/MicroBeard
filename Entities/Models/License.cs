using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class License
    {
        [Key]
        public int Code { get; set; }
        public string? Description { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DesactivatorCode { get; set; }
        public DateTime? DesactivationDate { get; set; }
        public bool Desactivated { get; set; }

        public ICollection<Collaborator>? Collaborators { get; set; }
    }
}
