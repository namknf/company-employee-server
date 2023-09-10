using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters parms, bool trackChanges)
        {
            var companies = await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();
            return PagedList<Company>.ToPagedList(companies, parms.PageNumber, parms.PageSize);
        }

        public async Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges) => 
            await FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();

        public void CreateCompany(Company company) => Create(company);

        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

        public void DeleteCompany(Company company)
        {
            Delete(company);
        }
    }
}
