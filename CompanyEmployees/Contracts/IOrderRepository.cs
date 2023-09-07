using Entities.Models;

namespace Contracts
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersAsync(Guid companyId, bool trackChanges);

        Task<Order?> GetOrderAsync(Guid companyId, Guid id, bool trackChanges);

        void CreateOrderForCompany(Guid companyId, Order order);

        void DeleteOrder(Order order);
    }
}
