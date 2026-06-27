namespace ProjectManagement.API.Controllers.V1
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

        protected IActionResult HandleResult<T>(T data, string? message = null) =>
            Ok(ApiResponse<T>.Success(data, message));
    }
}
