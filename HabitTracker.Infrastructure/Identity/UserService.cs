using System;
using System.Threading.Tasks;
using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Auth.DTOs;
using HabitTracker.Application.Interfaces.Identity;
using HabitTracker.Application.Interfaces.Security;
using HabitTracker.Domain.Entities;
using HabitTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Infrastructure.Identity
{
    public sealed class UserService : IUserService
    {
        // Database context for saving and retrieving users
        private readonly AppDbContext dbContext;

        // JWT token generator for creating signed tokens
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        // Constructor - sets the database context and token generator through dependency injection
        public UserService(AppDbContext dbContext, IJwtTokenGenerator jwtTokenGenerator)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        }

        // Validates user credentials and returns JWT token if login is successful
        public async Task<OperationResult<AuthResponseDto>> LoginAsync(LoginRequestDto request)
        {
            // Check if request is null
            if (request is null)
                return OperationResult<AuthResponseDto>.Fail("Invalid request.");

            // Step 1: Find user by email (and not soft deleted)
            HabitUser? user = await dbContext.HabitUsers
                .FirstOrDefaultAsync(u => u.EmailAddress == request.EmailAddress && !u.IsDeleted);

            if (user is null)
                return OperationResult<AuthResponseDto>.Fail("Invalid credentials.");

            // Step 2: Verify password using BCrypt (compare plain text with stored hash)
            bool passwordOk;
            try
            {
                passwordOk = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            }
            catch
            {
                // If hash is invalid or comparison fails
                return OperationResult<AuthResponseDto>.Fail("Invalid credentials.");
            }

            // If password doesn't match, fail login
            if (!passwordOk)
                return OperationResult<AuthResponseDto>.Fail("Invalid credentials.");

            // Step 3: Generate JWT token
            string token = jwtTokenGenerator.GenerateToken(
                userId: user.Id.ToString(),
                emailAddress: user.EmailAddress,
                roleName: user.RoleName,
                expiresAtUtc: out DateTime expiresAtUtc
            );

            // Step 4: Build response object with token and user info
            AuthResponseDto response = new AuthResponseDto
            {
                Token = token,
                ExpiresAtUtc = expiresAtUtc,
                EmailAddress = user.EmailAddress,
                FullName = user.FullName,
                RoleName = user.RoleName
            };

            // Step 5: Return success with token
            return OperationResult<AuthResponseDto>.Success(response);
        }

        // Creates a new user in the database with hashed password
        public async Task<OperationResult<Guid>> CreateAsync(string emailAddress, string password, string fullName, string roleName)
        {
            // Step 1: Basic validation for email and password
            if (string.IsNullOrWhiteSpace(emailAddress) || string.IsNullOrWhiteSpace(password))
                return OperationResult<Guid>.Fail("Email and password are required.");

            // Step 2: Clean up input values
            emailAddress = emailAddress.Trim();
            fullName = (fullName ?? string.Empty).Trim();
            roleName = string.IsNullOrWhiteSpace(roleName) ? "User" : roleName.Trim();

            // Step 3: Check if email already exists
            bool exists = await dbContext.HabitUsers.AnyAsync(u => u.EmailAddress == emailAddress && !u.IsDeleted);
            if (exists)
                return OperationResult<Guid>.Fail("Email is already registered.");

            // Step 4: Hash password using BCrypt
            string passwordHash;
            try
            {
                passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            }
            catch
            {
                return OperationResult<Guid>.Fail("Failed to hash password.");
            }

            // Step 5: Create new user object
            HabitUser user = new HabitUser
            {
                Id = Guid.NewGuid(),
                EmailAddress = emailAddress,
                PasswordHash = passwordHash,
                FullName = fullName,
                RoleName = roleName,
                CreatedAtUtc = DateTime.UtcNow,
                IsDeleted = false
            };

            // Step 6: Add user to database and save changes
            dbContext.HabitUsers.Add(user);
            await dbContext.SaveChangesAsync();

            // Step 7: Return success with new user's Id
            return OperationResult<Guid>.Success(user.Id);
        }
    }
}
