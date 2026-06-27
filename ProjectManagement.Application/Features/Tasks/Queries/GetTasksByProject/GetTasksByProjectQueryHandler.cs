namespace ProjectManagement.Application.Features.Tasks.Queries.GetTasksByProject
{
    public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, PaginatedList<TaskDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetTasksByProjectQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<TaskDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(Guid.Parse(request.ProjectId), cancellationToken)
                ?? throw new NotFoundException(nameof(Project), request.ProjectId);

            AuthorizationHelper.EnsureCanAccessProject(project, _currentUser.UserId!, _currentUser.IsAdmin);

            var paged = await _unitOfWork.ProjectTasks.GetPagedByProjectAsync(Guid.Parse(request.ProjectId), request.PageNumber, request.PageSize, cancellationToken);

            var dtos = paged.Items
                .Select(t => new TaskDto(t.Id.ToString(), t.Title, t.Description, t.Status, t.DueDate, t.Priority, t.ProjectId.ToString()))
                .AsQueryable();

            return await PaginatedList<TaskDto>.CreatAsync(dtos, paged.PageNumber, paged.PageSize, cancellationToken);
        }
    }
}
