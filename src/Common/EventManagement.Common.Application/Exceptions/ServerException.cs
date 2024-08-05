using EventManagement.Common.Domain.Results;

namespace EventManagement.Common.Application.Exceptions
{
    public class ServerException : Exception
    {
        public string RequestName { get; }
        public Error? Error { get; }
        public ServerException(
            string requestName,
            Error? error = default,
            Exception? innerException = default) : base("Server Exception", innerException)
        {
            RequestName = requestName;
            Error = error;
        }
    }
}
