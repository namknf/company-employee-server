using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<Address?> GetAddressAsync(short id, bool trackChanges) =>
            await FindByCondition(c => c.Code.Equals(id), trackChanges).SingleOrDefaultAsync();
    }
}
