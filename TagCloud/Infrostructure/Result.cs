using System;

namespace WordCloudGenerator
{
    public struct Result<T>
    {
        public Result(string error, T value = default)
        {
            Error = error;
            Value = value;
        }

        public static implicit operator Result<T>(T value)
        {
            return Result.Ok(value);
        }

        public string Error { get; }
        internal T Value { get; }
        public bool IsSuccess => Error == null;
    }

    public static class Result
    {
        public static Result<T> RepeatUntilOk<T>(Func<T> func, Action<string> errorHandler)
        {
            Result<T> result;
            do
            {
                result = Of(func);
                result.OnFail(errorHandler);
            } while (!result.IsSuccess);

            return result;
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
        }

        public static Result<T> Of<T>(Func<T> func, string error = null)
        {
            try
            {
                return Ok(func());
            }
            catch (Exception e)
            {
                return Fail<T>(error ?? e.Message);
            }
        }

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<string> handleError)
        {
            if (!input.IsSuccess) handleError(input.Error);
            return input;
        }

        private static Result<T> Fail<T>(string err)
        {
            return new Result<T>(err);
        }
    }
}