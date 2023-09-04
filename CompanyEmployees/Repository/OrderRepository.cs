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

        public IEnumerable<Order> GetAllOrders(bool trackChanges)
        {
            throw new NotImplementedException();
        }
    }
}
