namespace ProjectManagement.Application.Features.Tasks.Commands.DeleteTask
{
    public record DeleteTaskCommand(string TaskId) : IRequest<Unit>;
}
