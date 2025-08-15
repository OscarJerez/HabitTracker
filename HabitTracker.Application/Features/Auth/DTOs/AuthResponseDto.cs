namespace HabitTracker.Application.Features.Auth.DTOs
{
    // DTO returned after successful authentication
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;      // JWT token
        public DateTime ExpiresAtUtc { get; set; }             // UTC expiration time
        public string EmailAddress { get; set; } = string.Empty; // Authenticated user's email
        public string FullName { get; set; } = string.Empty;   // Authenticated user's display name
        public string RoleName { get; set; } = string.Empty;   // User role for authorization
    }
}
