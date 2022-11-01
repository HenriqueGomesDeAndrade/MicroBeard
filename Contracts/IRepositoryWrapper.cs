using Microsoft.EntityFrameworkCore;

namespace MicroBeard.Contracts
{
    public interface IRepositoryWrapper
    {
        IContactRepository Contact { get; }
        ICollaboratorRepository Collaborator { get; }
        ILicenseRepository License { get; }
        ISchedulingRepository Scheduling { get; }
        IServiceRepository Service { get; }
        void UnchangeProperty(object target, string propertyName);
        void UnchangeReference(object target, string referenceName);
        void UnchangeCollection(object target, string collectionName);

        void ChangeState<T>(object targets, EntityState state);
        void Save();
    }
}
