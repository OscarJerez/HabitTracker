namespace HabitTracker.Application.Features.Auth.DTOs
{
    // DTO for user login requests
    public class LoginRequestDto
    {
        public string EmailAddress { get; set; } = string.Empty; // User email
        public string Password { get; set; } = string.Empty;     // Plain text password from client
    }
}
