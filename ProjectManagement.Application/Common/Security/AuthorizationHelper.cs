namespace ProjectManagement.Application.Common.Security
{
    public static class AuthorizationHelper
    {
        public static void EnsureCanAccessProject(Project project, Guid currentUserId, bool isAdmin)
        {
            if (!isAdmin && project.UserId != currentUserId)
                throw new ForbiddenException("You do not have access to this project.");
        }
    }
}
