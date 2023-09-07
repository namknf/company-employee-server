using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public abstract class OrderForManipulationDto
    {
        [Required(ErrorMessage = "OrderedAt is a required field.")]
        public DateTime OrderedAt { get; set; }

        public DateTime? ShippedAt { get; set; }
    }
}
