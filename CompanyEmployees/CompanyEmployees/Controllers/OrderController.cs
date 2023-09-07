using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies/{companyId}/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public OrderController(IRepositoryManager repository, ILoggerService logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersForCompany(Guid companyId)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInformation($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var ordersFromDb = await _repository.Order.GetOrdersAsync(companyId, false);
            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(ordersFromDb);
            return Ok(ordersDto);
        }

        [HttpGet("{id}", Name = "OrderById")]
        public async Task<IActionResult> GetOrder(Guid companyId, Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInformation($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var order = await _repository.Order.GetOrderAsync(companyId, id, false);
            if (order == null)
            {
                _logger.LogInformation($"Order with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                return Ok(orderDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderForCompany(Guid companyId, [FromBody] OrderForCreationDto order)
        {
            if (order == null)
            {
                _logger.LogError("OrderForCreationDto object sent from client is null.");
                return BadRequest("OrderForCreationDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the OrderForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInformation($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }
            var orderEntity = _mapper.Map<Order>(order);
            _repository.Order.CreateOrderForCompany(companyId, orderEntity);
            await _repository.SaveAsync();
            var orderToReturn = _mapper.Map<OrderDto>(orderEntity);
            return CreatedAtRoute("CreateOrderForCompany", new
            {
                companyId,
                id = orderToReturn.Id
            }, orderToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id, Guid companyId)
        {
            var order = await _repository.Order.GetOrderAsync(companyId, id, trackChanges: false);
            if (order == null)
            {
                _logger.LogInformation($"Order with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Order.DeleteOrder(order);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderForCompany(Guid companyId, Guid id, [FromBody] OrderForUpdateDto order)
        {
            if (order == null)
            {
                _logger.LogError("OrderForUpdateDto object sent from client is null.");
                return BadRequest("OrderForUpdateDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the OrderForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInformation($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var orderEntity = await _repository.Order.GetOrderAsync(companyId, id, true);
            if (orderEntity == null)
            {
                _logger.LogInformation($"Order with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _mapper.Map(order, orderEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateOrderForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<OrderForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInformation($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var orderEntity = await _repository.Order.GetOrderAsync(companyId, id, true);
            if (orderEntity == null)
            {
                _logger.LogInformation($"Order with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var orderToPatch = _mapper.Map<OrderForUpdateDto>(orderEntity);
            patchDoc.ApplyTo(orderToPatch, ModelState);
            TryValidateModel(orderToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(orderToPatch, orderEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
