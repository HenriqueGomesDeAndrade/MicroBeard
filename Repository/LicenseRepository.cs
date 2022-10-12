using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using System.Data.Entity;

namespace MicroBeard.Repository
{
    public class LicenseRepository : RepositoryBase<License>, ILicenseRepository
    {
        public LicenseRepository(MicroBeardContext respositoryContext)
            :base(respositoryContext)
        {
            
        }

        public IEnumerable<License> GetAllLicenses()
        {
            return FindAll()
                .Where(c => c.Desactivated != true)
                .OrderBy(c => c.Description)
                .ToList();
        }

        public License GetLicenseByCode(int code)
        {
            return FindByCondition(c => c.Desactivated != true && c.Code.Equals(code)).FirstOrDefault();
        }

        public License GetLicenseWithDetails(int code)
        {
            return FindByCondition(c => c.Desactivated != true && c.Code.Equals(code)).FirstOrDefault();
        }

        public void CreateLicense(License license)
        {
            Create(license);
        }

        public void UpdateLicense(License license)
        {
            Update(license);
        }

        public void DeleteLicense(License license)
        {
            Delete(license);
        }
    }
}
