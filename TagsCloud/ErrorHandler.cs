using System;
using System.Diagnostics;

namespace TagsCloud
{
    public static class ErrorHandler
    {
        public static void ThrowError(string text)
        {
            Console.WriteLine(text);
            Process.GetCurrentProcess().Kill();
        }
    }
}
