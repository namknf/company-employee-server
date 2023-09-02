using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherForecastController : Controller
    {
        private ILoggerService _logger;

        public WeatherForecastController(ILoggerService logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInformation("Вот информационное сообщение от нашего контроллера значений.");
           
            _logger.LogDebug("Вот отладочное сообщение от нашего контроллера значений.");
           
            _logger.LogWarning("Вот сообщение предупреждения от нашего контроллера значений.");
           
            _logger.LogError("Вот сообщение об ошибке от нашего контроллера значений.");

            return new string[] { "value1", "value2" };
        }
    }
}
