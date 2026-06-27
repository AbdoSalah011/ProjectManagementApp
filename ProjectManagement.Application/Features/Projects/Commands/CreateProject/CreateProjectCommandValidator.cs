namespace ProjectManagement.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }
}
