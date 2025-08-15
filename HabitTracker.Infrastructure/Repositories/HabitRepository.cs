using HabitTracker.Application.Features.Habits.Interfaces.Repositories;
using HabitTracker.Domain.Entities;
using HabitTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTracker.Infrastructure.Repositories
{
    public class HabitRepository : IHabitRepository
    {
        private readonly AppDbContext _context;

        public HabitRepository(AppDbContext context)
        {
            _context = context;
        }

        // Retrieves all habits for a specific user
        public async Task<List<Habit>> GetHabitsByUserIdAsync(Guid userId)
        {
            return await _context.Habits
                .AsNoTracking()
                .Where(habit => habit.UserId == userId)
                .OrderByDescending(habit => habit.CreatedAt)
                .ToListAsync();
        }

        // Retrieves a specific habit by its ID
        public async Task<Habit?> GetHabitByIdAsync(Guid id)
        {
            return await _context.Habits
                .AsNoTracking()
                .FirstOrDefaultAsync(habit => habit.Id == id);
        }

        // Creates a new habit
        public async Task CreateHabitAsync(Habit habit)
        {
            await _context.Habits.AddAsync(habit);
            await _context.SaveChangesAsync();
        }

        // Updates an existing habit
        public async Task<bool> UpdateHabitAsync(Habit habit)
        {
            _context.Habits.Update(habit);
            int affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        // Soft-deletes a habit by ID by setting the IsDeleted flag to true
        // This method does not remove the record from the database, allowing recovery later
        public async Task DeleteHabitAsync(Guid habitId)
        {
            // Find the habit in the database by its unique ID
            Habit? habitToDelete = await _context.Habits
                .FirstOrDefaultAsync(habit => habit.Id == habitId);

            // If no matching habit is found, stop the method
            if (habitToDelete == null)
                return;

            // Mark the habit as deleted instead of removing it from the database
            habitToDelete.IsDeleted = true;

            // Save the change to the database
            await _context.SaveChangesAsync();
        }

    }
}
