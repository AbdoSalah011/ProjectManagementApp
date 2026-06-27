namespace ProjectManagement.Application.Features.Projects.Queries.GetProjectById
{
    public record GetProjectByIdQuery(string Id) : IRequest<ProjectDto>;
}
