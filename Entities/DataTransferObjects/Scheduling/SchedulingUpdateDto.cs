using MicroBeard.Attributes;
using MicroBeard.Entities.DataTransferObjects.Contact;
using MicroBeard.Entities.DataTransferObjects.Service;
using System.ComponentModel.DataAnnotations;

namespace MicroBeard.Entities.DataTransferObjects.Scheduling
{
    public class SchedulingUpdateDto
    {
        [DateFormatValidator]
        [Required]
        public DateTime Date { get; set; }

        [Range(0, 2147483647, ErrorMessage = "The code cannot be lesser than zero")]
        public int? ContactCode { get; set; }

        [Range(0, 2147483647, ErrorMessage = "The code cannot be lesser than zero")]
        public int? ServiceCode { get; set; }

        public bool Cancelled { get; set; }
        public SimpleContactDto Contact { get; set; }
        public SimpleServiceDto Service { get; set; }
    }
}
