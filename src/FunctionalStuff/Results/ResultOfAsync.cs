using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionalStuff.Results
{
    public static class ResultOfAsync
    {
        
        public static Task<Result<TOutput>> Task<TOutput>(
            Func<TOutput> continuation,
            CancellationToken token)
        {
            return Task(() => Result.Of(continuation), token);
        }

        public static Task<Result<TOutput>> Task<TOutput>(
            Func<Result<TOutput>> continuation,
            CancellationToken token)
        {
            return System.Threading.Tasks.Task.Run(continuation, token);
        }

        public static Task<Result<TOutput>> ThenRunTask<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Task<Result<TOutput>>> continuation)
        {
            return input.Then(continuation).Unwrap();
        }

        public static async Task<Result<T>> Unwrap<T>(this Result<Task<Result<T>>> input)
        {
            return input.IsSuccessful
                ? await input.Value
                : Result.Fail<T>(input.Error);
        }
    }
}