using AutoMapper;
using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Habits.DTOs;
using HabitTracker.Application.Features.Habits.Interfaces.Repositories;
using MediatR;

namespace HabitTracker.Application.Features.Habits.Queries
{
    // Handles GetHabitsForUserQuery and maps domain entities to DTOs using AutoMapper
    public class GetHabitsForUserQueryHandler
        : IRequestHandler<GetHabitsForUserQuery, OperationResult<List<HabitDto>>>
    {
        private readonly IHabitRepository _habitRepository; // Repository abstraction
        private readonly IMapper _mapper;                   // AutoMapper service

        // Constructor injection guarantees non-null fields and satisfies DI
        public GetHabitsForUserQueryHandler(IHabitRepository habitRepository, IMapper mapper)
        {
            _habitRepository = habitRepository ?? throw new ArgumentNullException(nameof(habitRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResult<List<HabitDto>>> Handle(
            GetHabitsForUserQuery request,
            CancellationToken cancellationToken)
        {
            var habits = await _habitRepository.GetHabitsByUserIdAsync(request.UserId);

            // Use AutoMapper to convert entities to DTOs
            var habitDtos = _mapper.Map<List<HabitDto>>(habits);

            return OperationResult<List<HabitDto>>.Success(habitDtos);
        }
    }
}
