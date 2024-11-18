using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Farven.Data;
using Farven.Models;

namespace Farven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrendsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrendsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview()
        {
            // Total de conversões
            var totalConversions = _context.ConversionHistories.Count();

            // Valor total convertido
            var totalConvertedAmount = _context.ConversionHistories.Sum(c => c.ConvertedAmount);

            // Moeda mais convertida (moeda de origem mais frequente)
            var mostConvertedCurrency = _context.ConversionHistories
                .GroupBy(c => c.FromCurrency)
                .OrderByDescending(g => g.Count())
                .Select(g => new { Currency = g.Key, Count = g.Count() })
                .FirstOrDefault();

            var overview = new
            {
                TotalConversions = totalConversions,
                TotalConvertedAmount = totalConvertedAmount,
                MostConvertedCurrency = mostConvertedCurrency?.Currency,
                MostConvertedCurrencyCount = mostConvertedCurrency?.Count
            };

            return Ok(overview);
        }

        [HttpGet("conversions-by-date")]
        public async Task<IActionResult> GetConversionsByDate()
        {
            var conversionsByDate = _context.ConversionHistories
                .GroupBy(c => c.ConversionDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count(), TotalAmount = g.Sum(c => c.Amount) })
                .OrderBy(g => g.Date)
                .ToList();

            return Ok(conversionsByDate);
        }
    }
}