namespace ProjectManagement.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(2000);
            RuleFor(x => x.ProjectId).NotEmpty();
            RuleFor(x => x.Priority).IsInEnum();
            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("Due date cannot be in the past.");
        }
    }
}
