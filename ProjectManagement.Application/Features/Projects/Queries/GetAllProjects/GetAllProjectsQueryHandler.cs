namespace ProjectManagement.Application.Features.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, PaginatedList<ProjectDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public GetAllProjectsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken ct)
        {
            var paged = _unitOfWork.Projects.GetPagedByUser(_currentUser.UserId!, _currentUser.IsAdmin);

            var dtos = _mapper.ProjectTo<ProjectDto>(paged);

            return await PaginatedList<ProjectDto>.CreatAsync(dtos, request.PageNumber, request.PageSize, ct);
        }
    }
}
