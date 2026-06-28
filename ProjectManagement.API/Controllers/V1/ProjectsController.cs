namespace ProjectManagement.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/projects")]
    [Authorize]
    public class ProjectsController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Project created.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProjectsQuery query)
        {
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetProjectByIdQuery(id));
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateProjectCommand command)
        {
            if (id != command.Id)
                return BadRequest("Route id and body id do not match.");

            var result = await Mediator.Send(command);
            return HandleResult(result, "Project updated.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteProjectCommand(id));
            return HandleResult<object?>(null, "Project deleted.");
        }
    }
}
