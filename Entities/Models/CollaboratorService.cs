using System;
using System.Collections.Generic;

namespace MicroBeard.Entities.Models
{
    public partial class CollaboratorService
    {
        public int? ServiceCode { get; set; }
        public int? CollaboratorCode { get; set; }

        public virtual Collaborator CollaboratorCodeNavigation { get; set; }
        public virtual Service ServiceCodeNavigation { get; set; }
    }
}
