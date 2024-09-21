using FluentValidation;
using PM.DATA.Models;

namespace PM.DATA.Validators
{
    public class TaskItemValidator : AbstractValidator<TaskItem>
    {
        public TaskItemValidator()
        {
            RuleFor(t => t.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(t => t.StartDate).LessThan(t => t.EndDate).WithMessage("Start date must be earlier than end date.");
            RuleFor(t => t.Status).IsInEnum().WithMessage("Invalid status value.");
            RuleFor(t => t.Priority).IsInEnum().WithMessage("Invalid priority value.");
        }
    }
}
