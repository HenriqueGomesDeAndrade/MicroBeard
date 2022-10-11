using MicroBeard.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroBeard.Entities.DataTransferObjects.Service
{
    public class ServiceDto
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

        public ICollection<Models.Scheduling>? Schedullings { get; set; }
        public ICollection<Models.Collaborator>? Collaborators { get; set; }
    }
}
