using Farven.Data;
using Farven.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Farven.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;
        private readonly EmailService _emailService; // Supondo que voc� tenha um servi�o de email configurado

        [BindProperty]
        public string Email { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public ForgotPasswordModel(ApplicationDbContext context, AuthService authService, EmailService emailService)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _context.Clientes.SingleOrDefault(c => c.Email == Email);

            if (user == null)
            {
                ErrorMessage = "E-mail n�o encontrado.";
                return Page();
            }

            // Gerar um token de redefini��o de senha
            var token = Guid.NewGuid().ToString(); // Voc� pode usar um sistema mais seguro de token se preferir
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1); // Token expira em 1 hora
            await _context.SaveChangesAsync();

            // Enviar o e-mail com o link de redefini��o
            var resetLink = Url.Page("/ResetPassword", null, new { token = token }, Request.Scheme);
            await _emailService.SendEmailAsync(Email, "Redefini��o de Senha", $"Clique aqui para redefinir sua senha: {resetLink}");

            SuccessMessage = "Um link de redefini��o de senha foi enviado para o seu e-mail.";
            return Page();
        }
    }
}