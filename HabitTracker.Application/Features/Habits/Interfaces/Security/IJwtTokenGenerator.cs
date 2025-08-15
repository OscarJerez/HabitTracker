using System;

namespace HabitTracker.Application.Interfaces.Security
{
    // Abstraction used by the Application layer to request JWT creation.
    // Keep the interface technology-agnostic so the Application does not depend on JWT libraries.
    public interface IJwtTokenGenerator
    {
        // Generates a signed JWT for the authenticated principal.
        // Returns the token string and outputs the UTC expiration time.
        string GenerateToken(string userId, string emailAddress, string roleName, out DateTime expiresAtUtc);
    }
}
