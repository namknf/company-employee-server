using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasData(
             new Order
             {
                 Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                 AddressId = 1,
                 CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
             },
             new Order
             {
                 Id = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"),
                 AddressId = 2,
                 CompanyId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
             });
        }
    }
}
