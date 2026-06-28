namespace ProjectManagement.Application.Features.Tasks.Queries.GetTasksByProject
{
    public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, PaginatedList<TaskDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public GetTasksByProjectQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TaskDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId, cancellationToken)
                ?? throw new NotFoundException(nameof(Project), request.ProjectId);

            AuthorizationHelper.EnsureCanAccessProject(project, _currentUser.UserId!, _currentUser.IsAdmin);

            var paged = _unitOfWork.ProjectTasks.GetPagedByProject(request.ProjectId);


            //var dtos = paged.ProjectTo<TaskDto>(_mapper.ConfigurationProvider);
            var dtoQuery = paged.Select(t => new TaskDto(
                t.Id,
                t.Title,
                t.Description,
                t.Status,
                t.DueDate,
                t.Priority,
                t.ProjectId));

            return await PaginatedList<TaskDto>.CreatAsync(dtoQuery, request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}
