namespace HRManagement.Models
{
    public class LoginModel
    {
        public required string Username { get; set; }

        public required string Password { get; set; }

        public required string Device { get; set; }
    }
}
