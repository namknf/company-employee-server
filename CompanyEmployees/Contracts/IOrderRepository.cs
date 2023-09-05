using Entities.Models;

namespace Contracts
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders(Guid companyId, bool trackChanges);

        Order? GetOrder(Guid companyId, Guid id, bool trackChanges);
    }
}
