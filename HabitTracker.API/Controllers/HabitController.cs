using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Habits.Commands;
using HabitTracker.Application.Features.Habits.DTOs;
using HabitTracker.Application.Features.Habits.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class HabitController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HabitController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Create habit
        [HttpPost]
        public async Task<IActionResult> CreateHabit([FromBody] CreateHabitDto createHabitDto, [FromQuery] Guid userId)
        {
            CreateHabitCommand command = new CreateHabitCommand(createHabitDto, userId);
            OperationResult<HabitDto> result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(OperationResult<HabitDto>.Fail(result.ErrorMessage!));

            return Ok(OperationResult<HabitDto>.Ok(result.Data!));
        }

        // Get habits for a user
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetHabitsForUser(Guid userId)
        {
            OperationResult<List<HabitDto>> result = await _mediator.Send(new GetHabitsForUserQuery(userId));

            if (!result.IsSuccess)
                return NotFound(OperationResult<List<HabitDto>>.Fail(result.ErrorMessage!));

            return Ok(OperationResult<List<HabitDto>>.Ok(result.Data!));
        }

        // Update habit
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateHabit(Guid id, [FromBody] UpdateHabitDto updateHabitDto, [FromQuery] Guid userId)
        {
            UpdateHabitCommand command = new UpdateHabitCommand(id, updateHabitDto, userId);
            OperationResult<HabitDto> result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // Delete habit
        [HttpDelete("{habitId:guid}")]
        public async Task<IActionResult> DeleteHabit(Guid habitId, [FromQuery] Guid userId)
        {
            DeleteHabitCommand command = new DeleteHabitCommand(habitId, userId);
            OperationResult<bool> result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return NotFound(OperationResult<bool>.Fail(result.ErrorMessage!));

            return NoContent();
        }
    }
}
