using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Habits.DTOs;
using MediatR;

namespace HabitTracker.Application.Features.Habits.Commands
{
    // Command to update an existing habit (uses a DTO for the HTTP body)
    public class UpdateHabitCommand : IRequest<OperationResult<HabitDto>>
    {
        public Guid HabitId { get; }                  // Id from route
        public UpdateHabitDto UpdateHabitDto { get; } // Body
        public Guid UserId { get; }                   // From query/claims

        public UpdateHabitCommand(Guid habitId, UpdateHabitDto updateHabitDto, Guid userId)
        {
            HabitId = habitId;
            UpdateHabitDto = updateHabitDto;
            UserId = userId;
        }
    }
}
