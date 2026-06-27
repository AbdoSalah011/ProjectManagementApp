namespace ProjectManagement.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CreateTaskCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken ct)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(Guid.Parse(request.ProjectId))
                ?? throw new NotFoundException(nameof(Project), request.ProjectId);

            AuthorizationHelper.EnsureCanAccessProject(project, _currentUser.UserId!, _currentUser.IsAdmin);

            var task = new ProjectTask
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Priority = request.Priority,
                ProjectId = Guid.Parse(request.ProjectId)
            };

            await _unitOfWork.ProjectTasks.AddAsync(task, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new TaskDto(task.Id.ToString(), task.Title, task.Description, task.Status, task.DueDate, task.Priority, task.ProjectId.ToString());
        }
    }
}
