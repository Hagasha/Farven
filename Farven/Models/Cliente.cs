namespace Farven.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Token de redefinição de senha e expiração
        public string? PasswordResetToken { get; set; }  // Permitir nulo
        public DateTime? PasswordResetTokenExpires { get; set; }  // Permitir nulo
    }
}
