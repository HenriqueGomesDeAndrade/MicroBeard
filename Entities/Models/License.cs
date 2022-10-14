using System;
using System.Collections.Generic;

namespace MicroBeard.Entities.Models
{
    public partial class License
    {
        public License()
        {
            Collaborators = new HashSet<LicencedCollaborator>();
        }

        public int Code { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? DesactivationDate { get; set; }
        public int? DesactivatorCode { get; set; }
        public bool? Desactivated { get; set; }

        public virtual ICollection<LicencedCollaborator> Collaborators { get; set; }
    }
}
