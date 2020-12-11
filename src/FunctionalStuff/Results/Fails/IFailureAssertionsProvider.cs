using System;

namespace FunctionalStuff.Results.Fails
{
    public interface IFailureAssertionsProvider<T>
    {
        string Name { get; }
        bool IsInverted { get; }
        T Value { get; }
        Result<T> ValidateWithMessage(Predicate<T> predicate, string message);
    }
}