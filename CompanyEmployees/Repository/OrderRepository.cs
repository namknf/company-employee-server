using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    internal class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public void CreateOrderForCompany(Guid companyId, Order order)
        {
            order.CompanyId = companyId;
            Create(order);
        }

        public void DeleteOrder(Order order)
        {
            Delete(order);
        }

        public async Task<Order?> GetOrderAsync(Guid companyId, Guid id, bool trackChanges) =>
            await FindByCondition(o => o.CompanyId.Equals(companyId) && o.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Order>> GetOrdersAsync(Guid companyId, bool trackChanges) =>
               await FindByCondition(o => o.CompanyId.Equals(companyId), trackChanges).OrderBy(o => o.ShippedAt).ToListAsync();
    }
}
