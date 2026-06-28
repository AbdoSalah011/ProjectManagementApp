namespace ProjectManagement.Application.Features.Tasks.DTOs
{
    public record TaskDto(
        Guid Id,
        string Title,
        string? Description,
        ProjectTaskStatus Status,
        DateTime? DueDate,
        TaskPriority Priority,
        Guid ProjectId);
}
