using MicroBeard.Entities.DataTransferObjects.Collaborator;
using System.ComponentModel.DataAnnotations;

namespace MicroBeard.Entities.DataTransferObjects.License
{
    public class LicenseCreationDto
    {
        [StringLength(250, ErrorMessage = "Description can't be longer than 250 characters")]
        public string Description { get; set; }

        public ICollection<SimpleCollaboratorDto> Collaborators { get; set; }
    }
}
