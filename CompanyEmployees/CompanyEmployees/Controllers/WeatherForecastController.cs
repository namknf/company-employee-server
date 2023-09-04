using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherForecastController : Controller
    {
        private ILoggerService _logger;

        private readonly IRepositoryManager _repository;
        public WeatherForecastController(IRepositoryManager repository, ILoggerService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _repository.Company.AnyMethodFromCompanyRepository();
            _repository.Employee.AnyMethodFromEmployeeRepository();
            _repository.Order.AnyMethodFromOrderRepository();
            _repository.Address.AnyMethodFromAddressRepository();
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public IEnumerable<string> GetLog()
        {
            _logger.LogInformation("Вот информационное сообщение от нашего контроллера значений.");
           
            _logger.LogDebug("Вот отладочное сообщение от нашего контроллера значений.");
           
            _logger.LogWarning("Вот сообщение предупреждения от нашего контроллера значений.");
           
            _logger.LogError("Вот сообщение об ошибке от нашего контроллера значений.");

            return new string[] { "value1", "value2" };
        }
    }
}
