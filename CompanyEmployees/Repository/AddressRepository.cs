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

        public async Task<IEnumerable<Address>> GetAddressesAsync(bool trackChanges) =>
               await FindAll(trackChanges).OrderBy(c => c.Country).ToListAsync();

        public async Task<Address?> GetAddressAsync(short id, bool trackChanges) =>
            await FindByCondition(c => c.Code.Equals(id), trackChanges).SingleOrDefaultAsync();

        public void DeleteAddress(Address address)
        {
            Delete(address);
        }

        public void CreateAddress(Address address)
        {
            Create(address);
        }
    }
}
