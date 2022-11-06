using MicroBeard.Attributes;
using MicroBeard.Entities.DataTransferObjects.License;
using MicroBeard.Entities.DataTransferObjects.Service;
using System.ComponentModel.DataAnnotations;

namespace MicroBeard.Entities.DataTransferObjects.Collaborator
{
    public class CollaboratorUpdateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }

        [DateFormatValidator]
        public DateTime? BirthDate { get; set; }

        [StringLength(15, ErrorMessage = "Phone can't be longer than 15 characters")]
        [RegularExpression(@"(\(?\d{2}\)?\s?)?(9?\d{4}\-?\d{4})", ErrorMessage = "Invalid Phone. Try to follow this pattern with or without DDD: (XX) 9XXXX-XXXX")]
        public string Phone { get; set; }

        [StringLength(100, ErrorMessage = "Function cannot be longer than 100 characters")]
        public string Function { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(80, ErrorMessage = "Email can't be longer than 80 characters")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
            ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Invalid range. The number must be between 0 and 9999999.99")]
        public decimal? Salary { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Invalid range. The number must be between 0 and 9999999.99")]
        public decimal? Commision { get; set; }

        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
            ErrorMessage = "Invalid Password. It must have more than 8 characters, at least one uppercase, one lowercase, one digit and one special character")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        [Required(ErrorMessage = "Licenses is required")]
        public ICollection<SimpleLicenseDto> Licenses { get; set; }

        [Required(ErrorMessage = "Services is required")]
        public ICollection<SimpleServiceDto> Services { get; set; }

    }
}
