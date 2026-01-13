namespace API.Dtos
{
    public partial class AuthForLoginDto
    {
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = [];
        public byte[] PasswordSalt { get; set; } = [];
    }
}