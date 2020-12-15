using System;

namespace TagsCloud
{
    public readonly struct Result<T>
    {
        public string Error { get; }
        internal T Value { get; }
        public bool IsSuccess => Error == null;
        
        public Result(string error, T value = default)
        {
            Error = error;
            Value = value;
        }
        
        public static implicit operator Result<T>(T v)
        {
            return Result.Ok(v);
        }
        
        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }
    }
}