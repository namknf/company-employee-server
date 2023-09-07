using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class OrderForCreationDto
    {
        [Required(ErrorMessage = "OrderedAt is a required field.")]
        public DateTime OrderedAt { get; set; }

        public DateTime? ShippedAt { get; set; }
    }
}
