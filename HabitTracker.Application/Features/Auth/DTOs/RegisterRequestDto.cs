namespace HabitTracker.API.Auth.DTOs
{
    // Request payload for creating a user account
    public class RegisterRequestDto
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string RoleName { get; set; } = "User";
    }
}
