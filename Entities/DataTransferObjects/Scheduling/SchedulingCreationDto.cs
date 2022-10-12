using MicroBeard.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace MicroBeard.Entities.DataTransferObjects.Scheduling
{
    public class SchedulingCreationDto
    {
        [DateFormatValidator]
        public DateTime Date { get; set; }

        [Range(0, 2147483647, ErrorMessage = "The code cannot be lesser than zero")]
        public int ContactCode { get; set; }

        [Range(0, 2147483647, ErrorMessage = "The code cannot be lesser than zero")]
        public int ServiceCode { get; set; }

        public Models.Contact? Contact { get; set; }

        public Models.Service? Service { get; set; }
    }
}
