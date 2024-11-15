using Farven.Data;
using Farven.Models;
using Farven.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Security.Claims;

namespace Farven.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string ErrorMessage { get; set; } // Para exibir mensagens de erro

        public LoginModel(ApplicationDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = _context.Clientes.SingleOrDefault(u => u.Username == Username);

            if (user == null || !_authService.VerifyPassword(Password, user.PasswordHash))
            {
                ErrorMessage = "Username ou senha inválidos.";
                return Page();
            }

            // Definir o cookie de autenticação
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim("Id", user.Id.ToString()) // Adicionando o ID do cliente aos claims
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToPage("/Index");
        }
    }
}
