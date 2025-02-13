namespace HRManagement.Models
{
    public class RefreshTokenModel
    {
        public required string Access_Token { get; set; }
        public required string Refresh_Token { get; set; }
        public required string Device { get; set; }
    }
}
