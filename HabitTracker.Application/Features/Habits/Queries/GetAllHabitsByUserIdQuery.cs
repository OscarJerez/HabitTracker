using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Habits.DTOs;
using MediatR;
using System;

namespace HabitTracker.Application.Features.Habits.Queries
{
    // Query model for retrieving all habits by user ID
    public class GetAllHabitsByUserIdQuery : IRequest<OperationResult<List<HabitDto>>>
    {
        public Guid UserId { get; set; }

        public GetAllHabitsByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
