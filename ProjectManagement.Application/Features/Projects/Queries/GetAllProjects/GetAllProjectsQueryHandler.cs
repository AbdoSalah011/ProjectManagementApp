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
            var paged = await _unitOfWork.Projects.GetPagedByUserAsync(_currentUser.UserId!, _currentUser.IsAdmin, request.PageNumber, request.PageSize);

            var dtos = _mapper.Map<List<ProjectDto>>(paged.Items).AsQueryable();

            return await PaginatedList<ProjectDto>.CreatAsync(dtos, paged.PageNumber, paged.PageSize, ct);
        }
    }
}
