namespace API.Dtos
{
    public class UserForLoginConfirmation
    {
        public byte[] PasswordHash { get; set; } = [];
        public byte[] PasswordSalt { get; set; } = [];
    }
}