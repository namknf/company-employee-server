using Entities.Models;

namespace Contracts
{
    public interface IAddressRepository
    {
        Task<Address?> GetAddressAsync(short id, bool trackChanges);
    }
}
