namespace Contracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }

        IEmployeeRepository Employee { get; }

        IOrderRepository Order { get; }

        IAddressRepository Address { get; }

        void Save();
    }
}
