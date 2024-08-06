using EventManagement.Common.Domain.Results;

namespace EventManagement.Common.Application.Exceptions
{
    public class InternalServerException : Exception
    {
        public string RequestName { get; }
        public Error? Error { get; }
        public InternalServerException(
            string requestName,
            Error? error = default,
            Exception? innerException = default) : base("Server Exception", innerException)
        {
            RequestName = requestName;
            Error = error;
        }
    }
}
