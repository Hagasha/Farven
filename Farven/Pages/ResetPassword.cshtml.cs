using Farven.Data;
using Farven.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace Farven.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        [FromRoute]
        public string Token { get; set; }

        public ResetPasswordModel(ApplicationDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public IActionResult OnGet()
        {
            // Verificar se o token é válido
            var user = _context.Clientes.SingleOrDefault(c => c.PasswordResetToken == Token && c.PasswordResetTokenExpires > DateTime.UtcNow);

            if (user == null)
            {
                ErrorMessage = "Token inválido ou expirado.";
                return Page();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "As senhas não coincidem.";
                return Page();
            }

            var user = _context.Clientes.SingleOrDefault(c => c.PasswordResetToken == Token && c.PasswordResetTokenExpires > DateTime.UtcNow);

            if (user == null)
            {
                ErrorMessage = "Token inválido ou expirado.";
                return Page();
            }

            // Atualizar a senha
            user.PasswordHash = _authService.HashPassword(NewPassword);

            // Invalidar o token após a redefinição da senha
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;

            // Atualizar o usuário no banco de dados
            _context.Clientes.Update(user);
            await _context.SaveChangesAsync();

            SuccessMessage = "Sua senha foi redefinida com sucesso.";
            return RedirectToPage("/Login");
        }
    }
}