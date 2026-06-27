namespace ProjectManagement.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, response) = exception switch
            {
                Application.Common.Exceptions.ValidationException vex => (
                    HttpStatusCode.BadRequest,
                    ApiResponse<object>.Failure(vex.Errors, "Validation failed.")),

                NotFoundException nfEx => (
                    HttpStatusCode.NotFound,
                    ApiResponse<object>.Failure(errors: null, message: nfEx.Message)),

                ForbiddenException fEx => (
                    HttpStatusCode.Forbidden,
                    ApiResponse<object>.Failure(errors: null, message: fEx.Message)),

                ConflictException cEx => (
                    HttpStatusCode.Conflict,
                    ApiResponse<object>.Failure(errors: null, message: cEx.Message)),

                UnauthorizedAccessException => (
                    HttpStatusCode.Unauthorized,
                    ApiResponse<object>.Failure(errors: null, message: "Unauthorized.")),

                _ => (
                    HttpStatusCode.InternalServerError,
                    ApiResponse<object>.Failure(errors: null, message: "An unexpected error occurred."))
            };

            // Full details logged server-side only — never leaked in the response body.
            if (statusCode == HttpStatusCode.InternalServerError)
                _logger.LogError(exception, "Unhandled exception occurred.");
            else
                _logger.LogWarning("Handled exception: {Message}", exception.Message);

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
