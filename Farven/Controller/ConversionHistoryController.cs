using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Farven.Models;
using Microsoft.AspNetCore.Authorization;

namespace Farven.Controller
{
    [Route("api/[controller]")]
    [Authorize]
    public class ConversionHistoryController : ControllerBase
    {
        private readonly ConversionService _conversionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConversionHistoryController(ConversionService conversionService, IHttpContextAccessor httpContextAccessor)
        {
            _conversionService = conversionService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ConversionHistoryDto conversionDto)
        {
            try
            {
                var clienteIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id");
                if (clienteIdClaim == null)
                {
                    return Unauthorized();
                }

                var clienteId = int.Parse(clienteIdClaim.Value);

                await _conversionService.SaveConversionAsync(clienteId, conversionDto.FromCurrency, conversionDto.ToCurrency, conversionDto.Amount, conversionDto.ConvertedAmount);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar a solicitação: {ex.Message}");
            }
        }
    }
}
public class ConversionHistoryDto
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
    }
