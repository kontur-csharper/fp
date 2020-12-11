using System;
using System.Collections.Generic;
using System.Linq;
using FunctionalStuff.Results;
using FunctionalStuff.Results.Fails;

namespace FunctionalStuff.Common
{
    public static class EnumerableExtensions
    {
        public static Result<ICollection<T>> ToResult<T>(this IEnumerable<Result<T>> source)
        {
            var arr = source.ToArray();

            var failed = arr.Where(x => !x.IsSuccessful).Select(x => x.Error).ToArray();
            if (failed.Length > 0)
                return Result.Fail<ICollection<T>>(failed.JoinStrings());

            ICollection<T> successfulResults = arr.Select(x => x.Value).ToArray();
            return Result.Ok(successfulResults);
        }

        public static string JoinStrings<T>(this IEnumerable<T> source, string separator = ", ",
            Func<T, string> mapper = null)
        {
            var strings = mapper == null
                ? source.Select(x => x.ToString())
                : source.Select(mapper);

            return string.Join(separator, strings);
        }
    }
}