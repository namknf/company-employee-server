using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class AddressForCreationDto
    {
        public short Code { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }
    }
}
