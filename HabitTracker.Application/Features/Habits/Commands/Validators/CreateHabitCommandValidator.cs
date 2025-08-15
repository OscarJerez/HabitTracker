using FluentValidation;

public class CreateHabitCommandValidator : AbstractValidator<CreateHabitCommand>
{
    public CreateHabitCommandValidator()
    {
        RuleFor(x => x.CreateHabitDto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.CreateHabitDto.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
