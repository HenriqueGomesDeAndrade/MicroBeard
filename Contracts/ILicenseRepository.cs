using MicroBeard.Entities.Models;
namespace MicroBeard.Contracts
{
    public interface ILicenseRepository : IRepositoryBase<License>
    {
        IEnumerable<License> GetAllLicenses();
        License GetLicenseByCode(int code);
        License GetLicenseWithDetails(int code);
        void CreateLicense(License license);
        void UpdateLicense(License license);
        void DeleteLicense(License license);
    }
}
