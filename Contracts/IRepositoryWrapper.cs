namespace MicroBeard.Contracts
{
    public interface IRepositoryWrapper
    {
        IContactRepository Contact { get; }
        ICollaboratorRepository Collaborator { get; }
        ILicenseRepository License { get; }
        ISchedulingRepository Scheduling { get; }
        IServiceRepository Service { get; }

        void Save();
    }
}
