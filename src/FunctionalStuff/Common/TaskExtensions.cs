using System;
using System.Threading.Tasks;
using FunctionalStuff.Results;

namespace FunctionalStuff.Common
{
    public static class TaskExtensions
    {
        public static Task<TOutput> ContinueWithTask<TInput, TOutput>(
            this Task<TInput> task,
            Func<Task<TInput>, Task<TOutput>> continuation) =>
            task.ContinueWith(continuation).Unwrap();

        public static Result<TInput> WaitResult<TInput>(this Task<Result<TInput>> input) =>
            input.ContinueWithResult().Result;

        public static Result<TInput> WaitResult<TInput>(this Task<TInput> input) =>
            input.ContinueWithResult().Result;

        public static Task<Result<T>> ContinueWithResult<T>(this Task<T> input) =>
            input.ThenContinueWith(r => r.AsResult());

        public static Task<Result<T>> ContinueWithResult<T>(this Task<Result<T>> input) =>
            input.ThenContinueWith(r => r);

        public static Task<Result<TOut>> ThenContinueWith<T, TOut>(this Task<T> input, Func<T, Result<TOut>> result) =>
            input.ContinueWith(task =>
            {
                if (task.IsCanceled) return HandleTaskCanceled<TOut>();
                if (task.IsFaulted) return HandleTaskFaulted<TOut>(task);
                return result(task.Result);
            });

        private static Result<T> HandleTaskCanceled<T>() => Result.Fail<T>(FailMessages.CancellationRequested);

        private static Result<T> HandleTaskFaulted<T>(Task task) =>
            Result.Fail<T>(
                task.Exception?.InnerExceptions != null
                    ? string.Join(", ", task.Exception.InnerExceptions)
                    : FailMessages.TaskFailed
            );
    }
}