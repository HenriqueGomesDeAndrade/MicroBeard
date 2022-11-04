using MicroBeard.Attributes;
using MicroBeard.Entities.DataTransferObjects.Scheduling;
using System.ComponentModel.DataAnnotations;

namespace MicroBeard.Entities.DataTransferObjects.Contact
{
    public class ContactUpdateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Address can't be longer than 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(80, ErrorMessage = "Email can't be longer than 80 characters")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
            ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [StringLength(15, ErrorMessage = "Phone can't be longer than 15 characters")]
        [RegularExpression(@"(\(?\d{2}\)?\s?)?(9?\d{4}\-?\d{4})")]
        public string Phone { get; set; }

        [StringLength(1, ErrorMessage = "Gender can't be longer than 1 characters")]
        [RegularExpression(@"[MFmf]", ErrorMessage = "Gender must be 'M' or 'F'")]
        public string Gender { get; set; }

        [DateFormatValidator]
        public DateTime? BirthDate { get; set; }

        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
            ErrorMessage = "Invalid Password. It must have more than 8 characters, at least one uppercase, one lowercase, one digit and one special character")]
        public string Password { get; set; }
    }
}
