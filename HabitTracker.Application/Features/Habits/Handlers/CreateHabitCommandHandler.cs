using AutoMapper;
using HabitTracker.Application.Common.Responses;
using HabitTracker.Application.Features.Habits.DTOs;
using HabitTracker.Application.Features.Habits.Interfaces.Repositories;
using HabitTracker.Domain.Entities;
using MediatR;

namespace HabitTracker.Application.Commands.Handlers;

// Handler for creating a new habit
public class CreateHabitCommandHandler : IRequestHandler<CreateHabitCommand, OperationResult<HabitDto>>
{
    private readonly IHabitRepository _habitRepository;
    private readonly IMapper _mapper;

    // Dependency Injection: repository and mapper
    public CreateHabitCommandHandler(IHabitRepository habitRepository, IMapper mapper)
    {
        _habitRepository = habitRepository ?? throw new ArgumentNullException(nameof(habitRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    // Handle the command: map DTO to domain, save, and map back to DTO
    public async Task<OperationResult<HabitDto>> Handle(CreateHabitCommand request, CancellationToken cancellationToken)
    {
        // Map from DTO to Domain model
        var newHabit = _mapper.Map<Habit>(request.CreateHabitDto);
        newHabit.Id = Guid.NewGuid();
        newHabit.UserId = request.UserId;
        newHabit.CreatedAt = DateTime.UtcNow;

        await _habitRepository.CreateHabitAsync(newHabit);

        // Map back from Domain to DTO
        var habitDto = _mapper.Map<HabitDto>(newHabit);

        return OperationResult<HabitDto>.Success(habitDto);
    }
}
