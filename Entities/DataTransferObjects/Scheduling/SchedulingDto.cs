using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MicroBeard.Entities.Models;


namespace MicroBeard.Entities.DataTransferObjects.Scheduling
{
    public class SchedulingDto
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


        [ForeignKey(nameof(Contact))]
        public int ContactCode { get; set; }
        public Models.Contact? Contact { get; set; }

        [ForeignKey(nameof(Service))]
        public int ServiceCode { get; set; }
        public Models.Service? Service { get; set; }
    }
}
