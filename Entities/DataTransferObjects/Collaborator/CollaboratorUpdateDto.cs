﻿using MicroBeard.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MicroBeard.Entities.DataTransferObjects.Collaborator
{
    public class CollaboratorUpdateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string? Name { get; set; }

        [DateFormatValidator]
        public DateTime? BirthDate { get; set; }

        [StringLength(15, ErrorMessage = "Phone can't be longer than 15 characters")]
        [RegularExpression(@"(\(?\d{2}\)?\s?)?(9?\d{4}\-?\d{4})", ErrorMessage = "Invalid Phone. Try to follow this pattern with or without DDD: (XX) 9XXXX-XXXX")]
        public string? Phone { get; set; }

        [StringLength(100, ErrorMessage = "Function cannot be longer than 100 characters")]
        public string? Function { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Invalid range. The number must be between 0 and 9999999.99")]
        public decimal? Salary { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Invalid range. The number must be between 0 and 9999999.99")]
        public decimal? Commision { get; set; }
    }
}