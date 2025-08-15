using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Habits.Commands;
using HabitTracker.Application.Features.Habits.Interfaces.Repositories;
using MediatR;

namespace HabitTracker.Application.Features.Habits.Handlers
{
    public sealed class DeleteHabitCommandHandler : IRequestHandler<DeleteHabitCommand, OperationResult<bool>>
    {
        private readonly IHabitRepository _habitRepository;

        public DeleteHabitCommandHandler(IHabitRepository habitRepository)
        {
            _habitRepository = habitRepository;
        }

        public async Task<OperationResult<bool>> Handle(DeleteHabitCommand request, CancellationToken cancellationToken)
        {
            var habit = await _habitRepository.GetHabitByIdAsync(request.HabitId);
            if (habit is null)
                return OperationResult<bool>.Fail("Habit not found.");

            if (habit.UserId != request.UserId)
                return OperationResult<bool>.Fail("Access denied.");

            await _habitRepository.DeleteHabitAsync(habit.Id);
            return OperationResult<bool>.Success(true);
        }
    }
}
