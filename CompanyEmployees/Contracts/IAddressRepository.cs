using Entities.Models;

namespace Contracts
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAddressesAsync(bool trackChanges);

        Task<Address?> GetAddressAsync(short id, bool trackChanges);

        void DeleteAddress(Address address);

        void CreateAddress(Address address);
    }
}
