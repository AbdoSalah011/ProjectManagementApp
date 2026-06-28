namespace ProjectManagement.Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
    {
        public UpdateTaskStatusCommandValidator()
        {
            RuleFor(x => x.TaskId).NotEqual(Guid.Empty);
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
