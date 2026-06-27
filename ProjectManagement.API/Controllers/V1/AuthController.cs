namespace ProjectManagement.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : BaseApiController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Login successful.");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh(RefreshTokenCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Token refreshed.");
        }
    }
}
