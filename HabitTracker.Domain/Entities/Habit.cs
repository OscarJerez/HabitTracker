namespace HabitTracker.Domain.Entities;


public class Habit
{
    public Guid Id { get; set; }                // Unique identifier
    public string Title { get; set; } = null!;  // Title of the habit
    public string Description { get; set; } = null!; // Description of the habit
    public DateTime CreatedAt { get; set; }     // When the habit was created
                                                
    // Foreign key to HabitUser
    public Guid UserId { get; set; }                  // Owner of the habit
    public HabitUser User { get; set; } = null!;      // Navigation property
    public bool IsDeleted { get; set; } = false;     // Soft delete flag
}
