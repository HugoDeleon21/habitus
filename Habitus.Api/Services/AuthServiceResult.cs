namespace Habitus.Api.Services
{
    public class AuthServiceResult<T>
    {
        private AuthServiceResult(bool success, T? data, string message, AuthErrorType? errorType)
        {
            Success = success;
            Data = data;
            Message = message;
            ErrorType = errorType;
        }

        public bool Success { get; }

        public T? Data { get; }

        public string Message { get; }

        public AuthErrorType? ErrorType { get; }

        public static AuthServiceResult<T> Ok(T data, string message)
        {
            return new AuthServiceResult<T>(true, data, message, null);
        }

        public static AuthServiceResult<T> Fail(string message, AuthErrorType errorType)
        {
            return new AuthServiceResult<T>(false, default, message, errorType);
        }
    }
}
