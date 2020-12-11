using FluentAssertions;
using FluentAssertions.Primitives;
using FunctionalStuff.Results;

namespace FunctionalStuff.TestingExtensions
{
    public static class ResultExtensions
    {
        public static AndWhichConstraint<ObjectAssertions, Result<T>> ShouldBeSuccessful<T>(this Result<T> input)
        {
            input.Error
                .Should()
                .BeNullOrWhiteSpace();

            input.IsSuccessful
                .Should()
                .BeTrue();

            return input.Should()
                .BeAssignableTo<Result<T>>();
        }

        public static AndWhichConstraint<ObjectAssertions, Result<T>> ShouldBeFailed<T>(this Result<T> input)
        {
            input.IsSuccessful
                .Should()
                .BeFalse();

            return input.Should()
                .BeAssignableTo<Result<T>>();
        }

        public static AndWhichConstraint<ObjectAssertions, Result<T>> WithError<T>(
            this AndWhichConstraint<ObjectAssertions, Result<T>> input, string expectedValue)
        {
            input.Subject
                .Error
                .Should()
                .Be(expectedValue);

            return input;
        }

        public static T Value<T>(this Result<T> input) =>
            input.ShouldBeSuccessful().Which.GetValueOrThrow();
    }
}