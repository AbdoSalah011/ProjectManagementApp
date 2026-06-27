namespace ProjectManagement.Application.Common.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException() : base("One or more validation failures occurred.")
            => Errors = new List<string>();

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
            => Errors = failures.Select(f => f.ErrorMessage).ToList();

    }
}
