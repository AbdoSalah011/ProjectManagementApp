namespace ProjectManagement.Application.Features.Projects.Commands.UpdateProject
{
    public record UpdateProjectCommand(Guid Id, string Name, string? Description) : IRequest<ProjectDto>;
}
