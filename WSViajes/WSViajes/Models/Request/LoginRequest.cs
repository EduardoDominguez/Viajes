namespace WSViajes.Models.Request
{
    public class LoginRequest : Respuesta
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public byte TipoUsuario { get; set; }

        public LoginRequest()
        {
            this.Email = string.Empty;
            this.Password = string.Empty;
        }
    }
}