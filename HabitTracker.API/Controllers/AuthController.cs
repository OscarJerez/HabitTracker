using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Auth.Commands;
using HabitTracker.Application.Features.Auth.DTOs;
using HabitTracker.API.Auth.DTOs;                 // RegisterRequestDto
using HabitTracker.Application.Interfaces.Identity; // IUserService
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        public AuthController(IMediator mediator, IUserService userService)
        {
            _mediator = mediator;
            _userService = userService;
        }

        // POST: /api/auth/register
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(OperationResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<Guid>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<Guid>>> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _userService.CreateAsync(
                request.EmailAddress,
                request.Password,
                request.FullName,
                request.RoleName ?? "User");

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result); // result.Data should be the new user's Guid
        }

        // POST: /api/auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(OperationResult<AuthResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<AuthResponseDto>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OperationResult<AuthResponseDto>>> Login([FromBody] LoginRequestDto request)
        {
            var command = new LoginCommand(request);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}
