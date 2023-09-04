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

        public void AnyMethodFromCompanyRepository()
        {
            throw new NotImplementedException();
        }
    }
}
