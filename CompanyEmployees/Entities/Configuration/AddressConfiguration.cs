using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasData(
             new Address
             {
                 Code = 1,
                 City = "Moscow",
                 Country = "Russia",
                 PostalCode = "12345",
                 Region = ""
             },
             new Address
             {
                 Code = 2,
                 City = "Saransk",
                 Country = "Russia",
                 PostalCode = "12365",
                 Region = "Mordovia"
             });
        }
    }
}
