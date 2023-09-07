using AutoMapper;
using CompanyEmployees.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        public async Task<IActionResult> GetAdresses()
        {
            try
            {
                var adresses = await _repository.Address.GetAddressesAsync(false);
                var adressesDto = _mapper.Map<IEnumerable<AddressDto>>(adresses);
                return Ok(adressesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the{nameof(GetAdresses)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "AddressById")]
        public async Task<IActionResult> GetAddress(short id)
        {
            var address = await _repository.Address.GetAddressAsync(id, false);
            if (address == null)
            {
                _logger.LogInformation($"Address with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var addressDto = _mapper.Map<AddressDto>(address);
                return Ok(addressDto);
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateAddress([FromBody] AddressForCreationDto address)
        {
            if (address == null)
            {
                _logger.LogError("AddressForCreationDto object sent from client is null.");
                return BadRequest("AddressForCreationDto object is null");
            }

            var addressEntity = _mapper.Map<Address>(address);
            _repository.Address.CreateAddress(addressEntity);
            await _repository.SaveAsync();
            var addressToReturn = _mapper.Map<AddressDto>(addressEntity);
            return CreatedAtRoute("AddressById", new { id = addressToReturn.Code }, addressToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(short id)
        {
            var address = await _repository.Address.GetAddressAsync(id, trackChanges: false);
            if (address == null)
            {
                _logger.LogInformation($"Address with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Address.DeleteAddress(address);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PartiallyUpdateAddress(short id, [FromBody] JsonPatchDocument<AddressForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var addressEntity = await _repository.Address.GetAddressAsync(id, true);
            if (addressEntity == null)
            {
                _logger.LogInformation($"Address with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var addressToPatch = _mapper.Map<AddressForUpdateDto>(addressEntity);
            patchDoc.ApplyTo(addressToPatch, ModelState);
            TryValidateModel(addressToPatch);

            _mapper.Map(addressToPatch, addressEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
