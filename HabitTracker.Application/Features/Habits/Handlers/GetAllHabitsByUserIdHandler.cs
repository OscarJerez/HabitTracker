using AutoMapper;
using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Habits.DTOs;
using HabitTracker.Application.Features.Habits.Interfaces.Repositories;
using HabitTracker.Application.Features.Habits.Queries;
using MediatR;

namespace HabitTracker.Application.Features.Habits.Handlers
{
    // Handler for GetAllHabitsByUserIdQuery
    public class GetAllHabitsByUserIdHandler : IRequestHandler<GetAllHabitsByUserIdQuery, OperationResult<List<HabitDto>>>
    {
        private readonly IHabitRepository _habitRepository;
        private readonly IMapper _mapper;

        public GetAllHabitsByUserIdHandler(IHabitRepository habitRepository, IMapper mapper)
        {
            _habitRepository = habitRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<HabitDto>>> Handle(GetAllHabitsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var habits = await _habitRepository.GetHabitsByUserIdAsync(request.UserId);

            if (habits == null || !habits.Any())
            {
                return OperationResult<List<HabitDto>>.Fail("No habits found for this user.");
            }

            List<HabitDto> habitDtos = _mapper.Map<List<HabitDto>>(habits);

            return OperationResult<List<HabitDto>>.Success(habitDtos);
        }
    }
}
