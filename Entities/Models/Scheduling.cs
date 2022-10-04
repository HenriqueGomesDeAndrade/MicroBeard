using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroBeard.Entities.Models
{
    public class Scheduling
    {
        [Key]
        public int Code { get; set; }
        public DateTime Date { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public int CancellerCode { get; set; }
        public bool Cancelled { get; set; }
        public int? DeleterCode { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Deleted { get; set; }


        [ForeignKey(nameof(Contact))]
        public int ContactCode { get; set; }
        public Contact? Contact { get; set; }

        [ForeignKey(nameof(Service))]
        public int ServiceCode { get; set; }
        public Service? Service { get; set; }
    }
}
