using MicroBeard.Entities.Models;
namespace MicroBeard.Contracts
{
    public interface ILicenseRepository
    {
        IEnumerable<License> GetAllLicenses();
        License GetLicenseByCode(int code, bool expandRelations = false);
        void CreateLicense(License license);
        void UpdateLicense(License license);
        void DeleteLicense(License license);
    }
}
