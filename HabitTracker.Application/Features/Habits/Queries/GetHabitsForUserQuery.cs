using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Habits.DTOs;
using MediatR;

namespace HabitTracker.Application.Features.Habits.Queries;
public sealed class GetHabitsForUserQuery : IRequest<OperationResult<List<HabitDto>>>
{
    public Guid UserId { get; set; }

    public GetHabitsForUserQuery(Guid userId)
    {
        UserId = userId;
    }
}
