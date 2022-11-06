using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.DataTransferObjects.Contact;
using MicroBeard.Entities.DataTransferObjects.Service;
using MicroBeard.Entities.DataTransferObjects.Collaborator;

namespace MicroBeard.Entities.DataTransferObjects.Scheduling
{
    public class SchedulingDto
    {
        public int Code { get; set; }
        public string Title { get; set; }
        public int? ServiceCode { get; set; }
        public int? ContactCode { get; set; }
        public int? CollaboratorCode { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? CancellationDate { get; set; }
        public int? CancellerCode { get; set; }
        public bool? Cancelled { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int? DeleterCode { get; set; }


        public virtual SimpleContactDto Contact { get; set; }
        public virtual SimpleServiceDto Service { get; set; }
        public virtual SimpleCollaboratorDto Collaborator { get; set; }
    }
}
