using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBeard.Entities.Models
{
    public class Service
    {
        [Key]
        public int Code { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Time { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleterCode { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Deleted { get; set; }

        public ICollection<Scheduling>? Schedullings { get; set; }
        public ICollection<Collaborator>? Collaborators { get; set; }

    }
}
