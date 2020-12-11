using System;

namespace FunctionalStuff.Actions
{
    public readonly struct Conditional<T>
    {
        private readonly Predicate<T> check;
        private readonly Action<T> whenTrue;
        private readonly Action<T> whenFalse;

        internal Conditional(Predicate<T> check, Action<T> whenTrue, Action<T> whenFalse)
        {
            this.check = check;
            this.whenTrue = whenTrue;
            this.whenFalse = whenFalse;
        }

        public Action<T> ToAction() => Invoke;

        public void Invoke(T value)
        {
            if (check == null) return;
            if (check(value)) whenTrue?.Invoke(value);
            else whenFalse?.Invoke(value);
        }

        public Conditional<T> With(
            Predicate<T> newCheck = null,
            Action<T> @true = null,
            Action<T> @false = null)
        {
            return new Conditional<T>(newCheck ?? check, @true ?? whenTrue, @false ?? whenFalse);
        }

        public static implicit operator Action<T>(Conditional<T> conditional) => conditional.ToAction();
    }
}