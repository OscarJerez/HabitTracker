using FluentValidation;

namespace HabitTracker.Application.Features.Habits.Commands.Validators
{
    public class DeleteHabitCommandValidator : AbstractValidator<DeleteHabitCommand>
    {
        public DeleteHabitCommandValidator()
        {
            RuleFor(command => command.HabitId)
                .NotEmpty().WithMessage("HabitId must be a valid GUID.");

            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("UserId must be a valid GUID.");
        }
    }
}
