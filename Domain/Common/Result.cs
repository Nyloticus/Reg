namespace Common
{

    public enum StatusCode
    {
        Ok = 200,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500
    }

    public class Result
    {
        public bool IsSuccess { get; init; }
        public string[]? Errors { get; set; }
        public StatusCode StatusCode { get; init; } = StatusCode.Ok;

        public bool IsFailure() => !IsSuccess;

        public static Result Failure(StatusCode statusCode, params string[]? errors) => new Result
        {
            Errors = errors,
            IsSuccess = false,
            StatusCode = statusCode
        };

        public static Result Success() => new Result { IsSuccess = true };
    }

    public class Result<T> : Result
    {
        public T Payload { get; init; }

        public Result(T payload)
        {
            Payload = payload;
            IsSuccess = true;
        }

        public static Result<T> Success(T payload) => new Result<T>(payload);

        public static Result<T> Failure(StatusCode statusCode, params string[]? errors) => new Result<T>(default)
        {
            Errors = errors,
            IsSuccess = false,
            StatusCode = statusCode
        };
    }

    public static class ResultExtensions
    {
        public static Result<T> AsSuccessResult<T>(this T obj) => Result<T>.Success(obj);
    }
}
