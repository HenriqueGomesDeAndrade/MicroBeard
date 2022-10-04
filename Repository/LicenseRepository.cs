using Entities;
using MicroBeard.Contracts;
using MicroBeard.Entities;
using MicroBeard.Entities.Models;

namespace MicroBeard.Repository
{
    public class LicenseRepository : RepositoryBase<License>, ILicenseRepository
    {
        public LicenseRepository(RepositoryContext respositoryContext)
            :base(respositoryContext)
        {
            
        }
    }
}
