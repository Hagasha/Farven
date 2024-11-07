using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farven.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; private set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Simulação de autenticação
            if (Username == "admin" && Password == "password")
            {
                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Invalid username or password";
                return Page();
            }
        }
    }
}
