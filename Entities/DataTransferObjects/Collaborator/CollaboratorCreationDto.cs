using MicroBeard.Attributes;
using MicroBeard.Entities.DataTransferObjects.License;
using MicroBeard.Entities.DataTransferObjects.Service;
using System.ComponentModel.DataAnnotations;

namespace MicroBeard.Entities.DataTransferObjects.Collaborator
{
    public class CollaboratorCreationDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }

        //[DateFormatValidatorAttribute]
        public DateTime? BirthDate { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF must have 11 numbers")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid CPF. It must be only numbers")]
        public string CPF { get; set; }

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

        [Required(ErrorMessage = "Password is required")]
        [StringLength(80, ErrorMessage = "The password must be bigger than 8 characters", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
            ErrorMessage = "Invalid Password. It must have more than 8 characters, at least one uppercase, one lowercase, one digit and one special character")]
        public string Password { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Invalid range. The number must be between 0 and 9999999.99")]
        public decimal? Salary { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Invalid range. The number must be between 0 and 9999999.99")]
        public decimal? Commision { get; set; }

        public bool IsAdmin { get; set; }

        public ICollection<SimpleLicenseDto> Licenses { get; set; }
        public ICollection<SimpleServiceDto> Services { get; set; }
    }
}
