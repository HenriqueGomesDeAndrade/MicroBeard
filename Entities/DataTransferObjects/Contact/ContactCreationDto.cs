﻿using MicroBeard.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace MicroBeard.Entities.DataTransferObjects.Contact
{
    public class ContactCreationDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string? Name { get; set; }

        [StringLength(200, ErrorMessage = "Address can't be longer than 200 characters")]
        public string? Address { get; set; }

        [StringLength(80, ErrorMessage = "Email can't be longer than 80 characters")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"+ "@"+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
            ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }

        [StringLength(15, ErrorMessage = "CPF can't be longer than 15 characters")]
        [RegularExpression(@"^\d{3}.?\d{3}.?\d{3}-?\d{2}$", ErrorMessage = "Invalid CPF. It must be only numbers or XXX.XXX.XXX-XX format")]
        public string? CPF { get; set; }

        [StringLength(15, ErrorMessage = "Phone can't be longer than 15 characters")]
        [RegularExpression(@"(\(?\d{2}\)?\s?)?(9?\d{4}\-?\d{4})")]
        public string? Phone { get; set; }

        [StringLength(1, ErrorMessage = "Gender can't be longer than 1 characters")]
        [RegularExpression(@"[MFmf]", ErrorMessage = "Gender must be 'M' or 'F'")]
        public string? Gender { get; set; }

        [DateFormatValidatorAttribute]
        public DateTime? BirthDate { get; set; }
    }
}
