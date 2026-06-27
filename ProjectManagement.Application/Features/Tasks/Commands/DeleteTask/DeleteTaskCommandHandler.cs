namespace ProjectManagement.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteTaskCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.ProjectTasks.GetByIdAsync(Guid.Parse(request.TaskId))
                ?? throw new NotFoundException(nameof(ProjectTask), request.TaskId);

            var project = await _unitOfWork.Projects.GetByIdAsync(task.ProjectId, cancellationToken)
                ?? throw new NotFoundException(nameof(Project), task.ProjectId);

            AuthorizationHelper.EnsureCanAccessProject(project, _currentUser.UserId!, _currentUser.IsAdmin);

            _unitOfWork.ProjectTasks.Delete(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
