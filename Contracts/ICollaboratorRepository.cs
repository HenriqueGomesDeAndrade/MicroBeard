﻿using MicroBeard.Entities.Models;

namespace MicroBeard.Contracts
{
    public interface ICollaboratorRepository
    {
        IEnumerable<Collaborator> GetAllCollaborators();
        Collaborator GetCollaboratorByCode(int code, bool expandRelations = false);
        void CreateCollaborator(Collaborator collaborator);
        void UpdateCollaborator(Collaborator collaborator);
        void DeleteCollaborator(Collaborator collaborator);
    }
}
