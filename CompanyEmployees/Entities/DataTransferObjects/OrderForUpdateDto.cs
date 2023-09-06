namespace Entities.DataTransferObjects
{
    public class OrderForUpdateDto
    {
        public DateTime OrderedAt { get; set; }

        public DateTime? ShippedAt { get; set; }
    }
}
