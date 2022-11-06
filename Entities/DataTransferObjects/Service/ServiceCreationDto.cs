using MicroBeard.Entities.DataTransferObjects.Collaborator;
using MicroBeard.Entities.DataTransferObjects.Scheduling;
using System.ComponentModel.DataAnnotations;

namespace MicroBeard.Entities.DataTransferObjects.Service
{
    public class ServiceCreationDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Invalid range. The number must be between 0 and 9999999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Time is required")]
        [Range(0, 10000, ErrorMessage = "Invalid range. The number must be between 0 and 10000")]
        public int Time { get; set; }

        [StringLength(50, ErrorMessage = "Type can't be longer than 50 characters")]
        public string Type { get; set; }

        [StringLength(250, ErrorMessage = "Description can't be longer than 250 characters")]
        public string Description { get; set; }

        [Range(0, 2147483647, ErrorMessage = "The code cannot be lesser than zero")]
        public int? LicenseCode { get; set; }

        public ICollection<SimpleCollaboratorDto> Collaborators { get; set; }
        public ICollection<SimpleSchedulingDto> Schedulings { get; set; }
    }
}
