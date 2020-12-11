using System;

namespace FunctionalStuff.Results.Fails
{
    public static class Fail
    {
        public static FailureAssertionsProvider<T> If<T>(T value, string visibleName = null) =>
            new FailureAssertionsProvider<T>(true, value, visibleName);

        public static Result<T> If<T>(T value, Predicate<T> predicate, string failMessage) =>
            If(value).Matches(predicate, failMessage);

        public static FailureAssertionsProvider<T> FailIf<T>(this T input, string visibleName = null)
        {
            return If(input, visibleName);
        }

        public static Result<TOut> ThenFailIf<T, TOut>(this Result<T> input,
            Func<FailureAssertionsProvider<T>, Result<TOut>> selector,
            string visibleName = null)
        {
            return input.Then(i =>
            {
                var provider = i.FailIf(visibleName);
                return selector(provider);
            });
        }
    }
}