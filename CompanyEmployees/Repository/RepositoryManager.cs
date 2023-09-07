using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private DataContext _dataContext;
        private ICompanyRepository _companyRepository;
        private IEmployeeRepository _employeeRepository;
        private IOrderRepository _orderRepository;
        private IAddressRepository _addressRepository;

        public RepositoryManager(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public ICompanyRepository Company
        {
            get
            {
                if (_companyRepository == null)
                    _companyRepository = new CompanyRepository(_dataContext);
                return _companyRepository;
            }
        }
        public IEmployeeRepository Employee
        {
            get
            {
                if (_employeeRepository == null)
                        _employeeRepository = new EmployeeRepository(_dataContext);
                    return _employeeRepository;
            }
        }

        public IOrderRepository Order
        {
            get
            {
                if (_orderRepository == null)
                    _orderRepository = new OrderRepository(_dataContext);
                return _orderRepository;
            }
        }
        public IAddressRepository Address
        {
            get
            {
                if (_addressRepository == null)
                    _addressRepository = new AddressRepository(_dataContext);
                return _addressRepository;
            }
        }

        public Task SaveAsync() => _dataContext.SaveChangesAsync();
    }
}
