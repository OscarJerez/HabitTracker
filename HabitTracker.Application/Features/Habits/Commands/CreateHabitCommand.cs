using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Habits.DTOs;
using MediatR;

public class CreateHabitCommand : IRequest<OperationResult<HabitDto>>
{
    public CreateHabitDto CreateHabitDto { get; init; } = default!;
    public Guid UserId { get; init; }

    public CreateHabitCommand(CreateHabitDto createHabitDto, Guid userId)
    {
        CreateHabitDto = createHabitDto;
        UserId = userId;
    }
}
