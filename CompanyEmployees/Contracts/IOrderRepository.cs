using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts
{
    public interface IOrderRepository
    {
        Task<PagedList<Order>> GetOrdersAsync(Guid companyId, OrderParameters parms, bool trackChanges);

        Task<Order?> GetOrderAsync(Guid companyId, Guid id, bool trackChanges);

        void CreateOrderForCompany(Guid companyId, Order order);

        void DeleteOrder(Order order);
    }
}
