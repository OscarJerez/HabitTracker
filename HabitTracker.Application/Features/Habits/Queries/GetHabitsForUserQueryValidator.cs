using FluentValidation;

namespace HabitTracker.Application.Features.Habits.Queries
{
    // Validates the "get habits for user" query before it reaches the handler
    public sealed class GetHabitsForUserQueryValidator : AbstractValidator<GetHabitsForUserQuery>
    {
        public GetHabitsForUserQueryValidator()
        {
            // UserId must be a non-empty GUID
            RuleFor(q => q.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }
    }
}
