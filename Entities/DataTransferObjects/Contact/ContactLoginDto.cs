using System.ComponentModel.DataAnnotations;

namespace MicroBeard.Entities.DataTransferObjects.Contact
{
    public class ContactLoginDto
    {
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
    }
}
