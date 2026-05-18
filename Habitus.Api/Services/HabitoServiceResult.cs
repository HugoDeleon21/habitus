namespace Habitus.Api.Services
{
    public class HabitoServiceResult<T>
    {
        private HabitoServiceResult(bool success, T? data, string message, HabitoErrorType? errorType)
        {
            Success = success;
            Data = data;
            Message = message;
            ErrorType = errorType;
        }

        public bool Success { get; }

        public T? Data { get; }

        public string Message { get; }

        public HabitoErrorType? ErrorType { get; }

        public static HabitoServiceResult<T> Ok(T data, string message = "")
        {
            return new HabitoServiceResult<T>(true, data, message, null);
        }

        public static HabitoServiceResult<T> Fail(string message, HabitoErrorType errorType)
        {
            return new HabitoServiceResult<T>(false, default, message, errorType);
        }
    }
}
