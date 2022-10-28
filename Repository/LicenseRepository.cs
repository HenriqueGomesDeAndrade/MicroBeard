using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using System.Data.Entity;

namespace MicroBeard.Repository
{
    public class LicenseRepository : ILicenseRepository
    {
        private MicroBeardContext _repositoryContext { get; set; }

        public LicenseRepository(MicroBeardContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public PagedList<License> GetAllLicenses(LicenseParameters licenseParameters)
        {
            return PagedList<License>.ToPagedList(
                _repositoryContext.Licenses.AsNoTracking()
                .Where(c => c.Desactivated != true)
                .OrderBy(c => c.Description),
                licenseParameters.PageNumber,
                licenseParameters.PageSize);
        }

        public License GetLicenseByCode(int code, bool expandRelations = true)
        {
            License license = _repositoryContext.Licenses.AsNoTracking().Where(c => c.Desactivated != true && c.Code.Equals(code)).FirstOrDefault();

            if(license != null && expandRelations)
                _repositoryContext.Entry(license).Collection(c => c.Collaborators).Load();

            return license;
        }

        public void CreateLicense(License license)
        {
            _repositoryContext.Licenses.Add(license);
        }

        public void UpdateLicense(License license)
        {
            _repositoryContext.Licenses.Update(license);
        }

        public void DeleteLicense(License license)
        {
            _repositoryContext.Licenses.Remove(license);
        }
    }
}
