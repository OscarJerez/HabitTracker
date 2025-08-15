using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Auth.DTOs;

namespace HabitTracker.Application.Interfaces.Identity
{
    public interface IUserService
    {
        Task<OperationResult<AuthResponseDto>> LoginAsync(LoginRequestDto request);

        
        Task<OperationResult<Guid>> CreateAsync(string emailAddress, string password, string fullName, string roleName);
    }
}
