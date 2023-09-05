using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<Address> GetAddresses(bool trackChanges) =>
               FindAll(trackChanges).OrderBy(c => c.Country).ToList();

        public Address? GetAddress(short id, bool trackChanges) =>
            FindByCondition(c => c.Code.Equals(id), trackChanges).SingleOrDefault();
    }
}
