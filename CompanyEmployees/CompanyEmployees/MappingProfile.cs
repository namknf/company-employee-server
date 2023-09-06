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
                opt => opt.MapFrom(x => string.Join(' ', x.Address.Country, x.Address.City, x.Address.Region).Trim()));

            CreateMap<Employee, EmployeeDto>();

            CreateMap<Order, OrderDto>();

            CreateMap<Address, AddressDto>()
                .ForMember(c => c.FullAddress,
                opt => opt.MapFrom(x => string.Join(' ', x.Country, x.City, x.Region).Trim()));

            CreateMap<CompanyForCreationDto, Company>();

            CreateMap<EmployeeForCreationDto, Employee>();

            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();

            CreateMap<CompanyForUpdateDto, Company>();

            CreateMap<AddressForCreationDto, Address>();

            CreateMap<OrderForCreationDto, Order>();
        }
    }
}
