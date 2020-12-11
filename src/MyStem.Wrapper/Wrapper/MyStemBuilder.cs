using System.Linq;
using FunctionalStuff.Common;
using FunctionalStuff.Results;
using MyStem.Wrapper.Enums;

namespace MyStem.Wrapper.Wrapper
{
    public sealed class MyStemBuilder : IMyStemBuilder
    {
        private readonly string path;

        public MyStemBuilder(string path)
        {
            this.path = path;
        }

        public Result<IMyStem> Create(MyStemOutputFormat outputFormat, params MyStemOptions[] args) =>
            args.Select(OptionToExecutionArg)
                .Prepend(OutputFormatToExecutionArg(outputFormat))
                .ToResult()
                .Then(arguments => string.Join(" ", arguments))
                .Then(launchArgs => (IMyStem) new MyStem(path, launchArgs))
                .RefineError("Error during MyStem creation");

        private static Result<string> OptionToExecutionArg(MyStemOptions option) => option switch
        {
            MyStemOptions.LinearMode => "-n",
            MyStemOptions.CopyEverything => "-c",
            MyStemOptions.OnlyLexical => "-w",
            MyStemOptions.WithoutOriginalForm => "-l",
            MyStemOptions.WithGrammarInfo => "-i",
            MyStemOptions.JoinSingleLemmaWordForms => "-g",
            MyStemOptions.PrintEndOfSentenceMarker => "-s",
            MyStemOptions.WithContextualDeHomonymy => "-d",
            _ => Result.Fail<string>($"Unsupported {nameof(MyStemOptions)} {option}")
        };

        private static Result<string> OutputFormatToExecutionArg(MyStemOutputFormat format) => format switch
        {
            MyStemOutputFormat.Json => Result.Ok("--format json"),
            MyStemOutputFormat.Xml => Result.Ok("--format xml"),
            MyStemOutputFormat.Text => Result.Ok("--format text"),
            _ => Result.Fail<string>($"Unsupported {nameof(MyStemOutputFormat)} {format}")
        };
    }
}