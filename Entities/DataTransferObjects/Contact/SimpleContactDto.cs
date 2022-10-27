using MicroBeard.Entities.Models;
using System.ComponentModel.DataAnnotations;
using MicroBeard.Entities.DataTransferObjects.Scheduling;

namespace MicroBeard.Entities.DataTransferObjects.Contact
{
    public class SimpleContactDto
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleterCode { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
