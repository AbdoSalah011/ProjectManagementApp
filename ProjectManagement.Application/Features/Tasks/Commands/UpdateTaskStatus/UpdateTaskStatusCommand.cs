namespace ProjectManagement.Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public record UpdateTaskStatusCommand(Guid TaskId, ProjectTaskStatus Status) : IRequest<TaskDto>;
}
