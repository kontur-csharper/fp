using System;

namespace FunctionalStuff.Results
{
    public static class Result
    {
        public static Result<T> AsResult<T>(this T value) => Ok(value);
        public static Result<T> Ok<T>(T value) => new Result<T>(null, value);
        public static Result<None> Ok() => Ok<None>(null);
        public static Result<T> Fail<T>(string e) => new Result<T>(e);

        public static Result<T> Of<T>(Func<T> f, string error = null)
        {
            try
            {
                return Ok(f());
            }
            catch (Exception e)
            {
                return Fail<T>(error ?? e.Message);
            }
        }

        public static Result<None> OfAction(Action f, string error = null)
        {
            try
            {
                f();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail<None>(error ?? e.Message);
            }
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            return input.Then(inp => Of(() => continuation(inp)));
        }

        public static Result<None> Then<TInput>(
            this Result<TInput> input,
            Action<TInput> continuation)
        {
            return input.Then(inp => OfAction(() => continuation(inp)));
        }

        public static Result<None> Then(
            this Result<None> input,
            Action continuation)
        {
            return input.Then(_ => continuation());
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.IsSuccessful
                ? continuation(input.Value)
                : Fail<TOutput>(input.Error);
        }

        public static Result<TInput> ThenDo<TInput>(this Result<TInput> input, Action<TInput> action)
        {
            return input.Then(i =>
            {
                action(i);
                return i;
            });
        }

        public static Result<TInput> ThenDo<TInput>(this Result<TInput> input, Action action)
        {
            return input.ThenDo(i => action());
        }

        public static Result<TInput> ThenDo<TInput>(
            this Result<TInput> input,
            Func<TInput, Result<None>> continuation)
        {
            return input.Then(i =>
            {
                var continuationResult = continuation(i);
                return continuationResult.IsSuccessful
                    ? Ok(i)
                    : Fail<TInput>(continuationResult.Error);
            });
        }

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<string> handleError)
        {
            if (!input.IsSuccessful) handleError(input.Error);
            return input;
        }

        public static Result<TInput> ReplaceError<TInput>(
            this Result<TInput> input,
            Func<string, string> replaceError)
        {
            if (input.IsSuccessful) return input;
            return Fail<TInput>(replaceError(input.Error));
        }

        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input,
            string errorMessage)
        {
            return input.ReplaceError(err => errorMessage + ". " + err);
        }

        public static T GetValueOrThrow<T>(this Result<T> input)
        {
            if (input.IsSuccessful) return input.Value;
            throw new InvalidOperationException($"No value. Only Error: {input.Error}");
        }

        public static Result<TOutput> DisposeAfter<TInput, TOutput>(
            this Result<TInput> input,
            Func<Result<TInput>, Result<TOutput>> continuation)
            where TInput : IDisposable
        {
            if (input.IsSuccessful)
            {
                using var disposable = input.Value;
                return continuation(input);
            }

            return Fail<TOutput>(input.Error);
        }

        public static Result<TOutput> ThenJoin<TInput, TSecond, TOutput>(
            this Result<TInput> input,
            Result<TSecond> second,
            Func<TInput, TSecond, TOutput> combine)
        {
            return input.Then(i => second.Then(v => combine(i, v)));
        }

        public static TInput GetValueOr<TInput>(
            this Result<TInput> input,
            Action<string> handleError,
            TInput defaultValue)
        {
            input.OnFail(handleError);
            return input.IsSuccessful
                ? input.Value
                : defaultValue;
        }
    }
}