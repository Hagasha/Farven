using Farven.Data;
using Farven.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace Farven.Pages
{
    public class UserHistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<ConversionHistory> ConversionHistories { get; set; }

        public UserHistoryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGet()
        {
            var clienteIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (clienteIdClaim == null)
            {
                // Redirecionar para a página de login ou exibir uma mensagem de erro
                return;
            }

            var clienteId = int.Parse(clienteIdClaim.Value);

            ConversionHistories = await _context.ConversionHistories
                .Where(ch => ch.ClienteId == clienteId)
                .ToListAsync();
        }
    }
}