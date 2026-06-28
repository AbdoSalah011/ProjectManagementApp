namespace ProjectManagement.Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken ct = default)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.Id, ct)
                ?? throw new Application.Common.Exceptions.NotFoundException(nameof(Project), request.Id);

            AuthorizationHelper.EnsureCanAccessProject(project, _currentUser.UserId!, _currentUser.IsAdmin);

            project.Name = request.Name;
            project.Description = request.Description;

            _unitOfWork.Projects.Update(project);
            await _unitOfWork.SaveChangesAsync(ct);

            return new ProjectDto(project.Id, project.Name, project.Description, project.CreatedAt, project.UserId);
        }
    }
}
