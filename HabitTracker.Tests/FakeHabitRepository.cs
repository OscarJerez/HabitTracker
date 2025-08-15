using HabitTracker.Application.Features.Habits.Interfaces.Repositories;
using HabitTracker.Domain.Entities;

public class FakeHabitRepository : IHabitRepository
{
    public List<Habit> Habits { get; set; } = new();

    public FakeHabitRepository(List<Habit> initialHabits)
    {
        Habits = initialHabits;
    }

    public Task CreateHabitAsync(Habit habit)
    {
        Habits.Add(habit);
        return Task.CompletedTask;
    }

    public Task DeleteHabitAsync(Guid id)
    {
        var habit = Habits.FirstOrDefault(h => h.Id == id);
        if (habit != null)
            Habits.Remove(habit);

        return Task.CompletedTask;
    }

    public Task<List<Habit>> GetHabitsByUserIdAsync(Guid userId)
    {
        return Task.FromResult(Habits.Where(h => h.UserId == userId).ToList());
    }

    public Task<Habit?> GetHabitByIdAsync(Guid id)
    {
        return Task.FromResult(Habits.FirstOrDefault(h => h.Id == id));
    }

    public Task<bool> UpdateHabitAsync(Habit habit)
    {
        var index = Habits.FindIndex(h => h.Id == habit.Id);
        if (index != -1)
        {
            Habits[index] = habit;
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

}
