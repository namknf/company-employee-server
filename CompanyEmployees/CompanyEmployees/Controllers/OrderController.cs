using AutoMapper;
using CompanyEmployees.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.DataShaping;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies/{companyId}/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<OrderDto> _dataShaper;

        public OrderController(IRepositoryManager repository, ILoggerService logger, IMapper mapper, IDataShaper<OrderDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        /// <summary>
        /// Запрос информации об опциях соединения, доступных в цепочке запросов/ответов, идентифицируемой запрашиваемым URI
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }

        /// <summary>
        /// Получение заказов / запрос на заголовки
        /// </summary>
        /// <param name="companyId">Идентификатор компании</param>
        /// <param name="parms">Параметры запроса</param>
        /// <returns></returns>
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetOrdersForCompany(Guid companyId, OrderParameters parms)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInformation($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var ordersFromDb = await _repository.Order.GetOrdersAsync(companyId, parms, false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(ordersFromDb.MetaData));
            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(ordersFromDb);
            return Ok(_dataShaper.ShapeData(ordersDto, parms.Fields));
        }

        /// <summary>
        /// Получение конкретного заказа
        /// </summary>
        /// <param name="companyId">Идентификатор компании</param>
        /// <param name="id">Идентификатор заказа</param>
        /// <returns>Заказ</returns>
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

        /// <summary>
        /// Добавление нового заказа
        /// </summary>
        /// <param name="companyId">Идентификатор компании</param>
        /// <param name="order">Модель для создания</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateOrderForCompany(Guid companyId, [FromBody] OrderForCreationDto order)
        {
            if (order == null)
            {
                _logger.LogError("OrderForCreationDto object sent from client is null.");
                return BadRequest("OrderForCreationDto object is null");
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

        /// <summary>
        /// Удаление заказа
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <param name="companyId">Идентификатор компании</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateOrderForCompanyExistsAttribute))]
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

        /// <summary>
        /// Обновление данных о заказе
        /// </summary>
        /// <param name="order">Обновленная модель заказа</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidateOrderForCompanyExistsAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateOrderForCompany([FromBody] OrderForUpdateDto order)
        {
            var orderEntity = HttpContext.Items["order"] as Order;
            _mapper.Map(order, orderEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Обновление конкретных свойств заказа
        /// </summary>
        /// <param name="patchDoc">Параметры запроса</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateOrderForCompanyExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateOrderForCompany([FromBody] JsonPatchDocument<OrderForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var orderEntity = HttpContext.Items["order"] as Order;
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
