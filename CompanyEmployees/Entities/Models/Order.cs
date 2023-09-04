using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Order
    {
        [Column("OrderId")]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }

        public Company Company { get; set; }

        public Employee Employee { get; set; }

        [Required(ErrorMessage = "OrderedAt is a required field.")]
        public DateTime OrderedAt { get; set; }

        public DateTime? ShippedAt { get; set; }

        [ForeignKey(nameof(Address))]
        public short AddressId { get; set; }

        public Address ShipTo { get; set; }
    }
}
