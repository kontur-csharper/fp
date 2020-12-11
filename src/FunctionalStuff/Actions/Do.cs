using System;

namespace FunctionalStuff.Actions
{
    public static class Do
    {
        public static void Nothing()
        {
        }

        public static void Nothing<T>(T arg1) => Nothing();

        public static Conditional<T> When<T>(Predicate<T> when, Action<T> @do) => 
            new Conditional<T>(when, @do, Nothing);

        public static Conditional<T> When<T>(T inputIs, Action<T> @do) where T : IEquatable<T> =>
            When(i => i.Equals(inputIs), @do);

        public static Conditional<T> NothingWhen<T>(Predicate<T> when) =>
            When(when, Nothing);

        public static Conditional<T> NothingWhen<T>(T inputIs) where T : IEquatable<T> =>
            When(inputIs, Nothing);

        public static Action<T> Else<T>(this Conditional<T> conditional, Action<T> @do) =>
            conditional.With(@false: @do);
    }
}