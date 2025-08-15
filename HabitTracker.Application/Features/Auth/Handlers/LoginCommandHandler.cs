using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Auth.Commands;
using HabitTracker.Application.Features.Auth.DTOs;
using HabitTracker.Application.Interfaces.Identity;
using MediatR;

namespace HabitTracker.Application.Features.Auth.Handlers
{
    // Handles LoginCommand by delegating to a user service that performs validation + token issuance
    public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<AuthResponseDto>>
    {
        private readonly IUserService _userService;

        public LoginCommandHandler(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<OperationResult<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Delegate to domain-aware service (checks credentials, builds JWT)
            return await _userService.LoginAsync(request.Request);
        }
    }
}
