namespace ProjectManagement.Application.Features.Tasks.Commands.CreateTask
{
    public record CreateTaskCommand(
        string Title,
        string? Description,
        DateTime DueDate,
        TaskPriority Priority,
        string ProjectId) : IRequest<TaskDto>;
}
