using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroBeard.Entities.Models
{
    public partial class Contact
    {
        public Contact()
        {
            Schedulings = new HashSet<Scheduling>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSaltGUID { get; set; }
        public string Cpf { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleterCode { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool? Deleted { get; set; }
        public string Token { get; set; }


        public virtual ICollection<Scheduling> Schedulings { get; set; }
    }
}
