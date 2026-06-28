namespace ProjectManagement.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectTask, TaskDto>();
            //.ForCtorParam(nameof(TaskDto.Id), opt => opt.MapFrom(src => src.Id.ToString())
            //.ForCtorParam(nameof(TaskDto.ProjectId), opt => opt.MapFrom(src => src.ProjectId.ToString())
        }
    }
}
