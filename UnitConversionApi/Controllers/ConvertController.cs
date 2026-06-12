using Microsoft.AspNetCore.Mvc;
using UnitConversionApi.Models;
using UnitConversionApi.Services;

namespace UnitConversionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConvertController : ControllerBase
    {
        private readonly IUnitConversionService _service;
        private readonly ILogger<ConvertController> _logger;

        public ConvertController(IUnitConversionService service, ILogger<ConvertController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post(ConversionRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (!_service.TryConvert(request, out var response, out var error))
            {
                _logger.LogWarning("Conversion failed: {Error}", error);
                var pd = new ProblemDetails
                {
                    Title = "Conversion failed",
                    Detail = error ?? "Unknown error",
                    Status = StatusCodes.Status400BadRequest
                };
                return BadRequest(pd);
            }

            return Ok(response);
        }

        [HttpGet("units")]
        public IActionResult GetUnits()
        {
            var data = _service.GetSupportedUnits()
                .ToDictionary(kv => kv.Key.ToString(), kv => kv.Value.OrderBy(u => u).ToArray());
            return Ok(data);
        }
    }
}
