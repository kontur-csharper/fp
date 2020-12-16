using System;

namespace ResultOfTask
{
    public static class ResultQueryExpressionExtensions
    {
        public static Result<TOutput> SelectMany<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            throw new NotImplementedException();
        }

        public static Result<TSelected> SelectMany<TInput, TOutput, TSelected>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation,
            Func<TInput, TOutput, TSelected> resultSelector)
        {
            throw new NotImplementedException();
        }
    }
}