using System;
using System.Collections.Generic;

namespace MicroBeard.Entities.Models
{
    public partial class LicencedCollaborator
    {
        public int? LicenceCode { get; set; }
        public int? CollaboratorCode { get; set; }

        public virtual Collaborator CollaboratorCodeNavigation { get; set; }
        public virtual License LicenceCodeNavigation { get; set; }
    }
}
