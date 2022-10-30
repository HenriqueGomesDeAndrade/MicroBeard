using MicroBeard.Entities.Models;
using System.ComponentModel.DataAnnotations;
using MicroBeard.Entities.DataTransferObjects.Service;
using MicroBeard.Entities.DataTransferObjects.License;


namespace MicroBeard.Entities.DataTransferObjects.Collaborator
{
    public class SimpleCollaboratorDto
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Function { get; set; }
        public decimal Salary { get; set; }
        public decimal Commision { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DesactivatorCode { get; set; }
        public DateTime? DesactivationDate { get; set; }
        public bool Desactivated { get; set; }
        public bool IsAdmin { get; set; }

    }
}
