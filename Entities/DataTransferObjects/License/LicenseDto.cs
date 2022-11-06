using MicroBeard.Entities.Models;
using System.ComponentModel.DataAnnotations;
using MicroBeard.Entities.DataTransferObjects.Collaborator;
using MicroBeard.Entities.DataTransferObjects.Service;

namespace MicroBeard.Entities.DataTransferObjects.License
{
    public class LicenseDto
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DesactivatorCode { get; set; }
        public DateTime? DesactivationDate { get; set; }

        public ICollection<SimpleCollaboratorDto> Collaborators { get; set; }
        public ICollection<SimpleServiceDto> Services { get; set; }

    }
}
