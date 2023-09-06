namespace Entities.DataTransferObjects
{
    public class OrderForCreationDto
    {
        public DateTime OrderedAt { get; set; }

        public DateTime? ShippedAt { get; set; }
    }
}
