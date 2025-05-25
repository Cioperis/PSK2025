namespace PSK.ServiceDefaults.Models
{
    public class User : BaseClass
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required bool IsActive { get; set; }
        // issubscribedtopositive
        public required bool IsSubscribedPositiveMsg { get; set; } = false;
        public DateTime LastLogin { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public ICollection<UserMessage> UserMessages { get; set; } = [];
    }
}