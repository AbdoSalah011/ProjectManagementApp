namespace ProjectManagement.Application.Features.Tasks.DTOs
{
    public record TaskDto(
        string Id,
        string Title,
        string? Description,
        ProjectTaskStatus Status,
        DateTime? DueDate,
        TaskPriority Priority,
        string ProjectId);
}
