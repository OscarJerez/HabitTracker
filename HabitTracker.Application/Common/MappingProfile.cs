using AutoMapper;
using HabitTracker.Application.Features.Habits.DTOs;
using HabitTracker.Domain.Entities;

namespace HabitTracker.Application.Common;

// AutoMapper profile to define mapping rules between DTOs and domain entities
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map from domain entity to HabitDto (used for responses)
        CreateMap<Habit, HabitDto>();

        // Map from CreateHabitDto to domain entity Habit (used for creation)
        CreateMap<CreateHabitDto, Habit>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.UserId, options => options.Ignore())
            .ForMember(destination => destination.CreatedAt, options => options.Ignore());

        // Update DTO -> Entity (do not overwrite identifiers or timestamps)
        CreateMap<UpdateHabitDto, Habit>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.UserId, options => options.Ignore())
            .ForMember(destination => destination.CreatedAt, options => options.Ignore());
    }
}
