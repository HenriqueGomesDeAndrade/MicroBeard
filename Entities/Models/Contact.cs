﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBeard.Entities.Models
{
    public class Contact
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public int? CPF { get; set; }
        public int? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleterCode { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Deleted { get; set; }

        public ICollection<Scheduling>? Schedulings { get; set; }
    }
}
    