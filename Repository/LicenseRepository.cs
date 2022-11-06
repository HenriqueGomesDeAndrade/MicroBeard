using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using MicroBeard.Helpers.Sort;
using System.Data.Entity;

namespace MicroBeard.Repository
{
    public class LicenseRepository : ILicenseRepository
    {
        private MicroBeardContext _repositoryContext { get; set; }
        private ISortHelper<License> _sortHelper { get; set; }

        public LicenseRepository(MicroBeardContext repositoryContext, ISortHelper<License> sortHelper)
        {
            _repositoryContext = repositoryContext;
            _sortHelper = sortHelper;
        }

        public PagedList<License> GetAllLicenses(LicenseParameters licenseParameters)
        {
            var licenses = _repositoryContext.Licenses.AsNoTracking()
                .Where(c => c.Desactivated != true);

            SearchByDescription(ref licenses, licenseParameters.Description);

            var sortedLicenses = _sortHelper.ApplySort(licenses, licenseParameters.OrderBy);

            return PagedList<License>.ToPagedList(
                sortedLicenses,
                licenseParameters.PageNumber,
                licenseParameters.PageSize);
        }

        public License GetLicenseByCode(int? code, bool expandRelations = true)
        {
            if (code == null)
                return null;

            License license = _repositoryContext.Licenses.Where(c => c.Desactivated != true && c.Code.Equals(code)).FirstOrDefault();

            if (license != null && expandRelations)
            {
                _repositoryContext.Entry(license).Collection(c => c.Collaborators).Load();
                if (license.Collaborators != null)
                    foreach (var collaborator in license.Collaborators)
                        if (collaborator.Desactivated == true)
                            license.Collaborators.Remove(collaborator);

                _repositoryContext.Entry(license).Collection(c => c.Services).Load();
                if (license.Services != null)
                    foreach (var service in license.Services)
                        if (service.Deleted == true)
                            license.Services.Remove(service);
            }
                

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
