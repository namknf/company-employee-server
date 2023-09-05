using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class CompanyForCreationDto
    {
        public string Name { get; set; }
        public Address Address { get; set; }
    }
}
