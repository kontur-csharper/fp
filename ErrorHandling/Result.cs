using System;
using System.Collections.Generic;
using System.Linq;

namespace ResultOfTask
{
    public class None
    {
        private None()
        {
        }
    }

    public struct Result<T>
    {
        public Result(string error, T value = default(T))
        {
            Error = error;
            Value = value;
        }

        public string Error { get; }
        internal T Value { get; }

        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }

        public bool IsSuccess => Error == null;
        
    }

    public static class Result
    {
        public static Result<T> AsResult<T>(this T value)
        {
            return Ok(value);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
        }

        public static Result<T> Fail<T>(string e)
        {
            return new Result<T>(e);
        }

        public static Result<T> Of<T>(Func<T> f, string error = null)
        {
            try
            {
                return Ok(f());
            }
            catch (Exception e)
            {
                return Fail<T>(error ?? e.Message);
            }
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            if (!input.IsSuccess)
                return Fail<TOutput>(input.Error);

            try
            {
                return Ok(continuation.Invoke(input.Value));
            }
            catch (Exception e)
            {
                return Fail<TOutput>(e.Message);
            }
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            if (input.IsSuccess)
                return continuation.Invoke(input.Value);
            return Fail<TOutput>(input.Error);
        }

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<string> handleError)
        {
            if (!input.IsSuccess)
                handleError.Invoke(input.Error);
            return input;
        }

        public static Result<TInput> ReplaceError<TInput>(
            this Result<TInput> input, 
            Func<string,string> handleError)
        {
            if(!input.IsSuccess)
                return Fail<TInput>(handleError.Invoke(input.Error));
            return input;
        }
        
        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input, 
            string handleError)
        {
            if(!input.IsSuccess)
                return Fail<TInput>($"{handleError}. {input.Error}");
            return input;
        }
        
        // стримы по программированию это тема
    }

//    public static class Sample
//    {
//        public static void DoStuff()
//        {
//            var input = Console.ReadLine();
//            ParseInt32("huj")
//                .Then(i => i + 1)
//                .Then(i => Console.WriteLine($"Incremented value is {i}"))
//                .OnFail(error => Console.WriteLine(error));
//        }
//
//        private static Result<int> ParseInt32(string input)
//        {
//            return int.TryParse(input, out var parsed)
//                ? Result.Ok(parsed)
//                : Result.Fail<int>($"Input {input} is not a valid Int32");
//        }
//    }
}