using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public abstract class CompanyForManipulationDto
    {
        [Required(ErrorMessage = "Company name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is a required field.")]
        public AddressForCreationDto Address { get; set; }

        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }

        public IEnumerable<OrderForCreationDto> Orders { get; set; }
    }
}
