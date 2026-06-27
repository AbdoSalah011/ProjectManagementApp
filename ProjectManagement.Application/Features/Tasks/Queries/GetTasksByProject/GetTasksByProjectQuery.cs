namespace ProjectManagement.Application.Features.Tasks.Queries.GetTasksByProject
{
    public class GetTasksByProjectQuery : PaginationRequest, IRequest<PaginatedList<TaskDto>>
    {
        public string ProjectId { get; init; } = null!;
    }
}
