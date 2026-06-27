namespace ProjectManagement.Application.Features.Projects.DTOs
{
    public record ProjectDto(
        string Id,
        string Name,
        string? Description,
        DateTime CreatedAt,
        string UserId);
}
