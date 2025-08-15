namespace HabitTracker.Domain.Entities
{
    // Domain entity representing an authenticated application user
    public class HabitUser
    {
        // Primary key (GUID for global uniqueness)
        public Guid Id { get; set; } = Guid.NewGuid();

        // Email used for login and contact
        public string EmailAddress { get; set; } = string.Empty;

        // Hashed password (never store plain text)
        public string PasswordHash { get; set; } = string.Empty;

        // Display name for UI and claims
        public string FullName { get; set; } = string.Empty;

        // Simple role name for authorization (e.g., "Member", "Admin", "Owner")
        public string RoleName { get; set; } = "Member";

        // Auditing fields
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}
