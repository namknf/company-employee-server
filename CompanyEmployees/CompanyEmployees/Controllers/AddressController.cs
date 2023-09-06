﻿using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
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
        public IActionResult GetAdresses()
        {
            try
            {
                var adresses = _repository.Address.GetAddresses(false);
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
        public IActionResult GetAddress(short id)
        {
            var address = _repository.Address.GetAddress(id, false);
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
        public IActionResult CreateAddress([FromBody] AddressForCreationDto address)
        {
            if (address == null)
            {
                _logger.LogError("AddressForCreationDto object sent from client is null.");
                return BadRequest("AddressForCreationDto object is null");
            }

            var addressEntity = _mapper.Map<Address>(address);
            _repository.Address.CreateAddress(addressEntity);
            _repository.Save();
            var addressToReturn = _mapper.Map<AddressDto>(addressEntity);
            return CreatedAtRoute("AddressById", new { id = addressToReturn.Code }, addressToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAddress(short id)
        {
            var address = _repository.Address.GetAddress(id, trackChanges: false);
            if (address == null)
            {
                _logger.LogInformation($"Address with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Address.DeleteAddress(address);
            _repository.Save();
            return NoContent();
        }
    }
}
