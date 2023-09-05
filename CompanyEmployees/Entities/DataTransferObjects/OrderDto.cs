namespace Entities.DataTransferObjects
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public DateTime OrderedAt { get; set; }

        public DateTime? ShippedAt { get; set; }
    }
}
