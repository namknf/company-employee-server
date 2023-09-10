using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
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

        public async Task<PagedList<Order>> GetOrdersAsync(Guid companyId, OrderParameters parms, bool trackChanges)
        {
            var orders = await FindByCondition(o => o.CompanyId.Equals(companyId), trackChanges).OrderBy(o => o.ShippedAt).ToListAsync();
            return PagedList<Order>.ToPagedList(orders, parms.PageNumber, parms.PageSize);
        }
    }
}
