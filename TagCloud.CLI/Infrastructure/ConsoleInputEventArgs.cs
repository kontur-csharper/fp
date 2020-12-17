using System;

namespace TagCloud.CLI.Infrastructure
{
    public class ConsoleInputEventArgs : EventArgs
    {
        public ConsoleInputEventArgs(string input, bool isTransfer)
        {
            IsTransfer = isTransfer;
            Input = input;
        }

        public bool IsTransfer { get; }

        public string Input { get; }
    }
}