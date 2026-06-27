namespace ProjectManagement.Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, TaskDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateTaskStatusCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<TaskDto> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.ProjectTasks.GetByIdAsync(Guid.Parse(request.TaskId))
                ?? throw new NotFoundException(nameof(ProjectTask), request.TaskId);

            var project = await _unitOfWork.Projects.GetByIdAsync(task.ProjectId)
                ?? throw new NotFoundException(nameof(Project), task.ProjectId);

            AuthorizationHelper.EnsureCanAccessProject(project, _currentUser.UserId!, _currentUser.IsAdmin);

            task.Status = request.Status;
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TaskDto(task.Id.ToString(), task.Title, task.Description, task.Status, task.DueDate, task.Priority, task.ProjectId.ToString());
        }
    }
}
