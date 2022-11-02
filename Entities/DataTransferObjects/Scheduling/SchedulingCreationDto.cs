using MicroBeard.Attributes;
using MicroBeard.Entities.DataTransferObjects.Contact;
using MicroBeard.Entities.DataTransferObjects.Service;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace MicroBeard.Entities.DataTransferObjects.Scheduling
{
    public class SchedulingCreationDto
    {
        [Required]
        [DateFormatValidator]
        public DateTime Date { get; set; }

        [Range(0, 2147483647, ErrorMessage = "The code cannot be lesser than zero")]
        public int? ContactCode { get; set; }

        [Range(0, 2147483647, ErrorMessage = "The code cannot be lesser than zero")]
        public int? ServiceCode { get; set; }
    }
}
