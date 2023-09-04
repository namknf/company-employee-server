using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    internal class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public void AnyMethodFromEmployeeRepository()
        {
            throw new NotImplementedException();
        }
    }
}
