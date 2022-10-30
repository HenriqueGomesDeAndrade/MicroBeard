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
            var licenses = _repositoryContext.Licenses.AsNoTracking()
                .Where(c => c.Desactivated != true);

            SearchByDescription(ref licenses, licenseParameters.Description);

            return PagedList<License>.ToPagedList(
                licenses.OrderBy(c => c.Description),
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

        private void SearchByDescription(ref IQueryable<License> licenses, string licenseDescription)
        {
            if (!licenses.Any() || string.IsNullOrWhiteSpace(licenseDescription))
                return;

            licenses = licenses.Where(c => c.Description.ToLower().Contains(licenseDescription.Trim().ToLower()));
        }
    }
}
