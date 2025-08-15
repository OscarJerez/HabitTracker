using AutoMapper;
using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Habits.Commands;    // <-- viktigt
using HabitTracker.Application.Features.Habits.DTOs;
using HabitTracker.Application.Features.Habits.Interfaces.Repositories;
using HabitTracker.Domain.Entities;
using MediatR;

namespace HabitTracker.Application.Features.Habits.Handlers
{
    public class UpdateHabitCommandHandler
        : IRequestHandler<UpdateHabitCommand, OperationResult<HabitDto>>   // <-- viktigt
    {
        private readonly IHabitRepository habitRepository;
        private readonly IMapper mapper;

        public UpdateHabitCommandHandler(IHabitRepository habitRepository, IMapper mapper)
        {
            this.habitRepository = habitRepository;
            this.mapper = mapper;
        }

        public async Task<OperationResult<HabitDto>> Handle(UpdateHabitCommand request, CancellationToken cancellationToken)
        {
            Habit? existingHabit = await habitRepository.GetHabitByIdAsync(request.HabitId);

            if (existingHabit is null || existingHabit.UserId != request.UserId)
                return OperationResult<HabitDto>.Fail("Habit not found or access denied.");

            mapper.Map(request.UpdateHabitDto, existingHabit);

            bool updated = await habitRepository.UpdateHabitAsync(existingHabit);
            if (!updated)
                return OperationResult<HabitDto>.Fail("Failed to update habit.");

            HabitDto dto = mapper.Map<HabitDto>(existingHabit);
            return OperationResult<HabitDto>.Success(dto);
        }
    }
}
