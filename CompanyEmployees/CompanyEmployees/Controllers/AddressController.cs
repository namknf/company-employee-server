using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies/{companyId}/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public AddressController(IRepositoryManager repository, ILoggerService logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение адреса компании
        /// </summary>
        /// <param name="companyId">Идентификатор компании</param>
        /// <returns>Адрес компании</returns>
        [HttpGet(Name = "AddressByCompany")]
        public async Task<IActionResult> GetAddressByCompany(Guid companyId)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, false);
            if (company == null)
            {
                _logger.LogInformation($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var addressId = company.AddressId;
            var address = await _repository.Address.GetAddressAsync(addressId, false);
            
            var addressDto = _mapper.Map<AddressDto>(address);
            return Ok(addressDto);
        }
    }
}
