using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
               FindAll(trackChanges).OrderBy(c => c.Name).ToList();
    }
}
