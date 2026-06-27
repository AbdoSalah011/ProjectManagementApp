namespace ProjectManagement.Application.Common.Wrappers
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public IEnumerable<string>? Errors { get; set; }

        public static ApiResponse<T> Success(T data, string? message)
            => new()
            {
                Succeeded = true,
                Message = message ?? "",
                Data = data
            };

        public static ApiResponse<T> Failure(IEnumerable<string>? errors, string message = "")
            => new()
            {
                Succeeded = false,
                Message = message,
                Errors = errors
            };
    }
}
