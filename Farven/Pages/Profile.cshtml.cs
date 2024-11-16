using Farven.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Farven.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public string Email { get; set; }

        public ProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // Obtenha o ID do cliente dos claims
            var clienteIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (clienteIdClaim != null)
            {
                int clienteId = int.Parse(clienteIdClaim.Value);
                var cliente = _context.Clientes.SingleOrDefault(c => c.Id == clienteId);

                if (cliente != null)
                {
                    Email = cliente.Email;
                }
            }
        }
    }
}