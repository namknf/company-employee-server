using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    internal class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public Order? GetOrder(Guid companyId, Guid id, bool trackChanges) =>
            FindByCondition(o => o.CompanyId.Equals(companyId) && o.Id.Equals(id), trackChanges).SingleOrDefault();

        public IEnumerable<Order> GetOrders(Guid companyId, bool trackChanges) =>
               FindByCondition(o => o.CompanyId.Equals(companyId), trackChanges).OrderBy(o => o.ShippedAt);
    }
}
