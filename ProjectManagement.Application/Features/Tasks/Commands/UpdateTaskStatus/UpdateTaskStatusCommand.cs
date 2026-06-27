namespace ProjectManagement.Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public record UpdateTaskStatusCommand(string TaskId, ProjectTaskStatus Status) : IRequest<TaskDto>;
}
