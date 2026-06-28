namespace ProjectManagement.Application.Features.Projects.DTOs
{
    public record ProjectDto(
        Guid Id,
        string Name,
        string? Description,
        DateTime CreatedAt,
        Guid UserId);
}
