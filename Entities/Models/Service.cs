using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroBeard.Entities.Models
{
    public partial class Service
    {
        public Service()
        {
            Schedulings = new HashSet<Scheduling>();
            Collaborators = new HashSet<Collaborator>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? Time { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int? LicenseCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int? DeleterCode { get; set; }
        public bool? Deleted { get; set; }


        public virtual License LicenseCodeNavigation { get; set; }
        public virtual ICollection<Scheduling> Schedulings { get; set; }
        public virtual ICollection<Collaborator> Collaborators { get; set; }

    }
}
