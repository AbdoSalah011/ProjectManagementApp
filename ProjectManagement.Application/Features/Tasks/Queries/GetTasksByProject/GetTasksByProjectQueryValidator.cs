namespace ProjectManagement.Application.Features.Tasks.Queries.GetTasksByProject
{
    public class GetTasksByProjectQueryValidator : AbstractValidator<GetTasksByProjectQuery>
    {
        public GetTasksByProjectQueryValidator()
        {
            RuleFor(x => x.ProjectId).NotEqual(Guid.Empty);
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }
}
