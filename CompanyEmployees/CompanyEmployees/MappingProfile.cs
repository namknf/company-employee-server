using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace CompanyEmployees
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress,
                opt => opt.MapFrom(x => string.Join(' ', x.Address.Country, x.Address.City, x.Address.Region)));

            CreateMap<Employee, EmployeeDto>();

            CreateMap<Order, OrderDto>();
        }
    }
}
