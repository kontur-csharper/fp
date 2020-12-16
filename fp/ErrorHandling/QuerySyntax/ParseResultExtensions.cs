using System;

namespace ResultOfTask
{
    public static class ParseResultExtensions
    {
        public static Result<int> ParseIntResult(this string s, string error = null)
        {
            int v;
            return int.TryParse(s, out v)
                ? v.AsResult()
                : Result.Fail<int>(error ?? "Не число " + s);
        }
        public static Result<Guid> ParseGuidResult(this string s, string error = null)
        {
            Guid v;
            return Guid.TryParse(s, out v)
                ? v.AsResult()
                : Result.Fail<Guid>(error ?? "Не GUID " + s);
        }
    }
}