using MicroBeard.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MicroBeard.Entities.DataTransferObjects.Scheduling
{
    public class SchedulingUpdateDto
    {
        [DateFormatValidator]
        [Required]
        public DateTime Date { get; set; }

        [Range(0, 2147483647, ErrorMessage = "The code cannot be lesser than zero")]
        public int ContactCode { get; set; }

        [Range(0, 2147483647, ErrorMessage = "The code cannot be lesser than zero")]
        public int ServiceCode { get; set; }

        public bool Cancelled { get; set; }
    }
}
