using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;

namespace MicroBeard.Contracts
{
    public interface ICollaboratorRepository
    {
        PagedList<Collaborator> GetAllCollaborators(CollaboratorParameters collaboratorParameters);
        Collaborator GetCollaboratorByCode(int? code, bool expandRelations = false);
        public Collaborator GetCollaboratorByEmail(string email);
        void CreateCollaborator(Collaborator collaborator);
        void UpdateCollaborator(Collaborator collaborator);
        void DeleteCollaborator(Collaborator collaborator);
        bool CheckIfIsLastAdminCollaborator(int collaboratorCode);
    }
}
