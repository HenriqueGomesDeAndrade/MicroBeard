using System.ComponentModel.DataAnnotations;

namespace MicroBeard.Entities.DataTransferObjects.Contact
{
    public class ContactUpdateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
        public string? Name { get; set; }

        [StringLength(50, ErrorMessage = "Address can't be longer than 100 characters")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "Email can't be longer than 50 characters")]
        public string? Email { get; set; }
        public int? CPF { get; set; }
        public int? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
