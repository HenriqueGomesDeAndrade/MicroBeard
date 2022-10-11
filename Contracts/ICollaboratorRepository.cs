using MicroBeard.Entities.Models;

namespace MicroBeard.Contracts
{
    public interface ICollaboratorRepository
    {
        IEnumerable<Collaborator> GetAllCollaborators();
        Collaborator GetCollaboratorByCode(int code);
        Collaborator GetCollaboratorWithDetails(int code);
        void CreateCollaborator(Collaborator collaborator);
        void UpdateCollaborator(Collaborator collaborator);
        void DeleteCollaborator(Collaborator collaborator);
    }
}
