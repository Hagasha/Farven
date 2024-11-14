using Farven.Data;
using Farven.Models;
using Farven.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farven.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public RegisterModel(ApplicationDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Adicionando um novo Cliente (ao invés de User)
            var cliente = new Cliente
            {
                Username = Username,
                Email = Email,
                PasswordHash = _authService.HashPassword(Password)
            };

            // Adicionando o Cliente ao DbContext
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Login");
        }
    }
}
