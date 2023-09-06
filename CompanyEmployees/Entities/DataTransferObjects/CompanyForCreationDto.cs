using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class CompanyForCreationDto
    {
        public string Name { get; set; }

        public Address Address { get; set; }

        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }

        public IEnumerable<OrderDto> Orders { get; set; }
    }
}
