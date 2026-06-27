namespace ProjectManagement.Application.Features.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQuery : PaginationRequest, IRequest<PaginatedList<ProjectDto>> { }
}
