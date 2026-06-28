namespace ProjectManagement.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/tasks")]
    [Authorize]
    public class TasksController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Task created.");
        }

        [HttpGet("by-project/{projectId}")]
        public async Task<IActionResult> GetByProject(Guid projectId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetTasksByProjectQuery { ProjectId = projectId, PageNumber = pageNumber, PageSize = pageSize };
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPatch("{taskId}/status")]
        public async Task<IActionResult> UpdateStatus(Guid taskId, UpdateTaskStatusCommand command)
        {
            if (taskId != command.TaskId)
                return BadRequest("Route id and body id do not match.");

            var result = await Mediator.Send(command);
            return HandleResult(result, "Task status updated.");
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> Delete(Guid taskId)
        {
            await Mediator.Send(new DeleteTaskCommand(taskId));
            return HandleResult<object?>(null, "Task deleted.");
        }
    }
}
