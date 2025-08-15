using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Auth.DTOs;
using MediatR;

namespace HabitTracker.Application.Features.Auth.Commands
{
    // Command that carries a login request through MediatR
    public class LoginCommand : IRequest<OperationResult<AuthResponseDto>>
    {
        public LoginRequestDto Request { get; }

        public LoginCommand(LoginRequestDto request)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }
    }
}
