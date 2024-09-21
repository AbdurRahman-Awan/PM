using FluentValidation;
using PM.DATA.Models;

namespace PM.DATA.Validators
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator()
        {
            RuleFor(p => p.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(p => p.StartDate).LessThan(p => p.EndDate).WithMessage("Start date must be earlier than end date.");
            RuleFor(p => p.Status).IsInEnum().WithMessage("Invalid status value.");
        }
    }
}
