using HabitTracker.Domain.Entities;

namespace HabitTracker.Application.Features.Habits.Interfaces.Repositories;

public interface IHabitRepository
{
    Task<List<Habit>> GetHabitsByUserIdAsync(Guid userId);
    Task<Habit?> GetHabitByIdAsync(Guid id);
    Task CreateHabitAsync(Habit habit);
    Task<bool> UpdateHabitAsync(Habit habit);
    Task DeleteHabitAsync(Guid id);
}
