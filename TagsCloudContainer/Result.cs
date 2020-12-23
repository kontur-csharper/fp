using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public class None
    {
        private None()
        {
        }
    }

    public readonly struct Result<T>
    {
        public Result(string error, T value = default)
        {
            ErrorMessage = error;
            Value = value;
        }

        public string ErrorMessage { get; }
        internal T Value { get; }
        public bool IsSuccess => ErrorMessage == null;
        
        public Result<TOutput> Then<TOutput>(Func<T, TOutput> continuation)
            => Then(inp => Result.Of(() => continuation(inp)));

        public Result<None> Then(Action<T> continuation)
            => Then(inp => Result.OfAction(() => continuation(inp)));

        public Result<TOutput> Then<TOutput>(Func<T, Result<TOutput>> continuation)
            => IsSuccess ? continuation(Value) : Result.Fail<TOutput>(ErrorMessage);

        public Result<T> ReplaceError(Func<string, string> replaceError)
            => IsSuccess ? this : Result.Fail<T>(replaceError(ErrorMessage));

        public Result<T> RefineError(string errorMessage)
            => ReplaceError(err => errorMessage + ". " + err);
    }

    public static class Result
    {
        public static Result<T> AsResult<T>(this T value)
            => Ok(value);

        public static Result<T> Ok<T>(T value)
            => new Result<T>(null, value);

        public static Result<None> Ok()
            => Ok<None>(null);

        public static Result<T> Fail<T>(string errorMessage)
            => new Result<T>(errorMessage);

        public static Result<T> Of<T>(Func<T> function, string errorMessage = null)
        {
            try
            {
                return Ok(function());
            }
            catch (Exception e)
            {
                return Fail<T>(errorMessage ?? e.Message);
            }
        }

        public static Result<None> OfAction(Action action, string errorMessage = null)
        {
            try
            {
                action();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail<None>(errorMessage ?? e.Message);
            }
        }

        public static Result<IEnumerable<TInput>> EnumerateOrFail<TInput>(this IEnumerable<Result<TInput>> input)
        {
            string error = null;
            input = input.TakeWhile(t => (error = t.ErrorMessage) == null).ToArray();
            if (error != null) return Fail<IEnumerable<TInput>>(error);
            return input.Select(r => r.Value).AsResult();
        }
    }
}