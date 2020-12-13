using System;
using ResultOf;

namespace TagsCloudContainer
{
    public static class ResultExtensions
    {
        public static Result<T> Validate<T>(
            this Result<T> result, Func<T, bool> isValid, string errorMessage)
        {
            if (result.IsSuccess && !isValid(result.Value))
                return Result.Fail<T>(errorMessage);
            
            return result;
        }
    }
}