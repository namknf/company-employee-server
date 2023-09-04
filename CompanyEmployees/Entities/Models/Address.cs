using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Address
    {
        [Column("Code")]
        [Key]
        public short Code { get; set; }

        [Required(ErrorMessage = "City is a required field.")]
        public string City { get; set; }

        public string Region { get; set; }

        [Required(ErrorMessage = "PostalCode is a required field.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is a required field.")]
        public string Country { get; set; }
    }
}
