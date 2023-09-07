using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class AddressForCreationDto
    {
        [Required(ErrorMessage = "City is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the City is 30 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Region is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Region is 30 characters.")]
        public string Region { get; set; }

        [Required(ErrorMessage = "PostalCode is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the PostalCode is 30 characters.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Country is 30 characters.")]
        public string Country { get; set; }
    }
}
