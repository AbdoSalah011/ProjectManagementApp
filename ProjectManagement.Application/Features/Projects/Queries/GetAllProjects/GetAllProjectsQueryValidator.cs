namespace ProjectManagement.Application.Features.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQueryValidator : AbstractValidator<GetAllProjectsQuery>
    {
        public GetAllProjectsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }
}

