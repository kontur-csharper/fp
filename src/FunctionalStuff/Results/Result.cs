namespace FunctionalStuff.Results
{
    public readonly struct Result<T>
    {
        public Result(string error, T value = default)
        {
            Error = error;
            Value = value;
        }

        public static implicit operator Result<T>(T v) => Result.Ok(v);

        public string Error { get; }
        internal T Value { get; }

        public bool IsSuccessful => Error == null;
    }
}