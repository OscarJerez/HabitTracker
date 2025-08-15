using AutoMapper;
using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Habits.DTOs;
using HabitTracker.Application.Features.Habits.Interfaces.Repositories;
using HabitTracker.Domain.Entities;
using MediatR;

namespace HabitTracker.Application.Features.Habits.Queries
{
    public class GetHabitByIdQueryHandler : IRequestHandler<GetHabitByIdQuery, OperationResult<HabitDto>>
    {
        private readonly IHabitRepository _habitRepository;
        private readonly IMapper _mapper;

        public GetHabitByIdQueryHandler(IHabitRepository habitRepository, IMapper mapper)
        {
            _habitRepository = habitRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<HabitDto>> Handle(GetHabitByIdQuery request, CancellationToken cancellationToken)
        {
            Habit? habit = await _habitRepository.GetHabitByIdAsync(request.HabitId);

            if (habit == null)
            {
                return OperationResult<HabitDto>.Fail("Habit not found.");
            }

            HabitDto habitDto = _mapper.Map<HabitDto>(habit);

            return OperationResult<HabitDto>.Success(habitDto);
        }
    }
}
