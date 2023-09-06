namespace Entities.DataTransferObjects
{
    public class CompanyForCreationDto
    {
        public string Name { get; set; }

        public AddressForCreationDto Address { get; set; }

        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }

        public IEnumerable<OrderForCreationDto> Orders { get; set; }
    }
}
