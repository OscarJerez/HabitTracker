using FluentValidation;
using HabitTracker.Application.Features.Habits.Commands;

namespace HabitTracker.Application.Features.Habits.Commands.Validators
{
    public class UpdateHabitCommandValidator : AbstractValidator<UpdateHabitCommand>
    {
        public UpdateHabitCommandValidator()
        {
            RuleFor(x => x.HabitId)
                .NotEmpty().WithMessage("HabitId must be a valid GUID.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId must be a valid GUID.");

            RuleFor(x => x.UpdateHabitDto.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must be 100 characters or fewer.");

            RuleFor(x => x.UpdateHabitDto.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must be 500 characters or fewer.");
        }
    }
}
