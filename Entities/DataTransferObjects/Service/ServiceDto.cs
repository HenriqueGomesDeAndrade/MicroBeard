using System;
using System.Collections.Generic;
using MicroBeard.Entities.DataTransferObjects.Scheduling;
using MicroBeard.Entities.DataTransferObjects.Collaborator;
using MicroBeard.Entities.DataTransferObjects.Contact;
using MicroBeard.Entities.DataTransferObjects.License;

namespace MicroBeard.Entities.DataTransferObjects.Service
{
    public class ServiceDto
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Time { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int? LicenseCode { get; set; }
        public int? CreatorCode { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdaterCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleterCode { get; set; }
        public DateTime? DeleteDate { get; set; }

        public virtual SimpleLicenseDto License { get; set; }

        public ICollection<SimpleSchedulingDto> Schedulings { get; set; }
        public ICollection<SimpleCollaboratorDto> Collaborators { get; set; }
    }
}
