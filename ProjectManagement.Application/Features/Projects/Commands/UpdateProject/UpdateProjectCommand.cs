namespace ProjectManagement.Application.Features.Projects.Commands.UpdateProject
{
    public record UpdateProjectCommand(string Id, string Name, string? Description) : IRequest<ProjectDto>;
}
