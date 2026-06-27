namespace ProjectManagement.Application.Features.Projects.Commands.DeleteProject
{
    public record DeleteProjectCommand(string Id) : IRequest<Unit>;
}
