using AutoMapper;
using CompanyEmployees.ActionFilters;
using CompanyEmployees.ModelBinders;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CompanyEmployees.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<CompanyDto> _dataShaper;

        public CompanyController(IRepositoryManager repository, ILoggerService logger, IMapper mapper, IDataShaper<CompanyDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }

        /// <summary>
        /// Получение всех компаний
        /// </summary>
        /// <param name="parms">Доп. параметры для пейджинга, сортировки</param>
        /// <returns>Компании</returns>
        [HttpGet(Name = "GetCompanies"), Authorize]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyParameters parms)
        {
            try
            {
                var companies = await _repository.Company.GetAllCompaniesAsync(parms, false);
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(companies.MetaData));
                var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
                return Ok(_dataShaper.ShapeData(companiesDto, parms.Fields));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the{nameof(GetCompanies)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Получение компании по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор компании</param>
        /// <returns>Компания</returns>
        [HttpGet("{id}", Name = "CompanyById"), Authorize]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges: false);
            if (company == null)
            {
                _logger.LogInformation($"Company with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var companyDto = _mapper.Map<CompanyDto>(company);
                return Ok(companyDto);
            }
        }

        /// <summary>
        /// Создание компании
        /// </summary>
        /// <param name="company">Модель для создания компании</param>
        /// <returns>Созданная компания</returns>
        [HttpPost, Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (company == null)
            {
                _logger.LogError("CompanyForCreationDto object sent from client is null.");
                return BadRequest("CompanyForCreationDto object is null");
            }

            var companyEntity = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id }, companyToReturn);
        }

        /// <summary>
        /// Получение коллекции компаний
        /// </summary>
        /// <param name="ids">Идентификаторы искомых компаний</param>
        /// <returns>Найденный компании</returns>
        [HttpGet("collection/({ids})", Name = "CompanyCollection"), Authorize]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var companyEntities = await _repository.Company.GetByIdsAsync(ids, trackChanges: false);
            if (ids.Count() != companyEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return Ok(companiesToReturn);
        }

        /// <summary>
        /// Создание нескольких компаний сразу
        /// </summary>
        /// <param name="companyCollection">Массив компаний, которые необходимо создать</param>
        /// <returns>Массив созданных компаний</returns>
        [HttpPost("collection"), Authorize]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection == null)
            {
                _logger.LogError("Company collection sent from client is null.");
                return BadRequest("Company collection is null");
            }
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            await _repository.SaveAsync();
            var companyCollectionToReturn =
            _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("CompanyCollection", new { ids }, companyCollectionToReturn);
        }

        /// <summary>
        /// Удаление компании
        /// </summary>
        /// <returns>Удалено (успешно)</returns>
        [HttpDelete("{id}"), Authorize]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteCompany()
        {
            var company = HttpContext.Items["company"] as Company;
            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Редактирование компании
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="company">Модель с измененными свойствами</param>
        /// <returns>Измененная модель</returns>
        [HttpPut("{id}"), Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            var companyEntity = HttpContext.Items["company"] as Company;
            _mapper.Map(company, companyEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
