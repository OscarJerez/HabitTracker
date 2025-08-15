namespace HabitTracker.Application.Features.Habits.DTOs
{
    public class CreateHabitDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        //public Guid UserId { get; set; }
    }
}
