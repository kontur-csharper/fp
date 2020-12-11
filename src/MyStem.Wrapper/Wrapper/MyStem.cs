using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using FunctionalStuff.Results;

namespace MyStem.Wrapper.Wrapper
{
    //ТУДУ переписать чтобы каждый раз не стартовать процесс по новой
    internal sealed class MyStem : IMyStem
    {
        private readonly Func<Result<Process>> processFactory;

        public MyStem(string exePath, string launchArgs)
        {
            processFactory = () => CreateProcess(exePath, launchArgs);
        }

        public Result<string> GetResponse(string text) =>
            processFactory.Invoke()
                .DisposeAfter(result => result
                    .ThenDo(p => ExecuteRequest(p, text))
                    .Then(ReadResponse));

        private static Result<string> ReadResponse(Process executingProcess) =>
            Result.Of(() => executingProcess.StandardOutput.ReadToEnd())
                .ThenDo(executingProcess.WaitForExit);

        private static Result<None> ExecuteRequest(Process process, string text) =>
            Result.Of(() => Encoding.UTF8.GetBytes(text))
                .RefineError($"Cannot get bytes from string {text}")
                .Then(bytes => process.StandardInput.BaseStream.Write(bytes, 0, bytes.Length))
                .Then(process.StandardInput.Flush)
                .Then(process.StandardInput.Close);

        private static Result<Process> CreateProcess(string exePath, string launchArgs)
        {
            if (!File.Exists(exePath))
                return Result.Fail<Process>($"MyStem file cannot be found on path {exePath}");

            return Result.Of(() => Process.Start(new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = launchArgs,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardInputEncoding = Encoding.UTF8,
                }))
                .RefineError("Cannot start MyStem process");
        }
    }
}