using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farven.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; private set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Adicione a lógica de registro aqui (por exemplo, salvar no banco de dados)
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "All fields are required.";
                return Page();
            }

            // Simulação de sucesso de cadastro
            return RedirectToPage("/Login");
        }
    }
}
