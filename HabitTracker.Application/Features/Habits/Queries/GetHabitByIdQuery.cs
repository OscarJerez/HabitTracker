using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Habits.DTOs;
using MediatR;

namespace HabitTracker.Application.Features.Habits.Queries;

public class GetHabitByIdQuery : IRequest<OperationResult<HabitDto>>
{
    public Guid HabitId { get; set; }

    public GetHabitByIdQuery(Guid habitId)
    {
        HabitId = habitId;
    }
}
