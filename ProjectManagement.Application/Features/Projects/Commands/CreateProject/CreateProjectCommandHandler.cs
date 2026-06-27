namespace ProjectManagement.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public CreateProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                UserId = _currentUser.UserId!   // never trust a UserId from the request body — always derived from JWT
            };

            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProjectDto>(project);
        }
    }
}
