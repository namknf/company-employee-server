using Entities.Models;

namespace Contracts
{
    public interface IAddressRepository
    {
        IEnumerable<Address> GetAddresses(bool trackChanges);

        Address? GetAddress(short id, bool trackChanges);

        void DeleteAddress(Address address);

        void CreateAddress(Address address);
    }
}
