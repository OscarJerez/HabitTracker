using HabitTracker.Application.Common.Responses;
using MediatR;

namespace HabitTracker.Application.Features.Habits.Commands
{
    public class DeleteHabitCommand : IRequest<OperationResult<bool>>
    {
        public Guid HabitId { get; set; }
        public Guid UserId { get; }

        public DeleteHabitCommand(Guid habitId, Guid userId)
        {
            HabitId = habitId;
            UserId = userId;
        }
    }
}
