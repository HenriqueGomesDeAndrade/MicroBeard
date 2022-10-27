using System;
using System.Collections.Generic;

namespace MicroBeard.Entities.Models
{
    public partial class Scheduling
    {
        public int Code { get; set; }
        public int? ServiceCode { get; set; }
        public int? ContactCode { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? CancellationDate { get; set; }
        public int? CancellerCode { get; set; }
        public bool? Cancelled { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int? DeleterCode { get; set; }
        public bool? Deleted { get; set; }

        public virtual Contact ContactCodeNavigation { get; set; }
        public virtual Service ServiceCodeNavigation { get; set; }
    }
}
