namespace ProjectManagement.Application.Features.Tasks.Commands.DeleteTask
{
    public record DeleteTaskCommand(Guid TaskId) : IRequest<Unit>;
}
