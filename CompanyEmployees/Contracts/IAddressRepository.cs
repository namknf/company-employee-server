using Entities.Models;

namespace Contracts
{
    public interface IAddressRepository
    {
        IEnumerable<Address> GetAllAddresses(bool trackChanges);
    }
}
