using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;

namespace MicroBeard.Contracts
{
    public interface ILicenseRepository
    {
        PagedList<License> GetAllLicenses(LicenseParameters licenseParameters);
        License GetLicenseByCode(int? code, bool expandRelations = false);
        void CreateLicense(License license);
        void UpdateLicense(License license);
        void DeleteLicense(License license);
    }
}
