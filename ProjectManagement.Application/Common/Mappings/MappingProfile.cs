namespace ProjectManagement.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectTask, TaskDto>();
        }
    }
}
