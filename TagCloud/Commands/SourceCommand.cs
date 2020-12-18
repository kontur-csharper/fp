using System.IO;
using TagCloud.Extensions;
using TagCloud.Settings;
using TagCloud.Sources;

namespace TagCloud.Commands
{
    public class SourceCommand : ICommand
    {
        private readonly ISource[] sources;
        private readonly SourceSettings sourceSettings;

        public SourceCommand(ISource[] sources, SourceSettings sourceSettings)
        {
            this.sources = sources;
            this.sourceSettings = sourceSettings;
        }

        public string CommandId { get; } = "source";
        public string Description { get; } = "Allows to specify the path for the word file";
        public string Usage { get; } = "source <Full file name>";

        public ICommandResult Handle(string[] args)
        {
            if (args.Length != 1)
                return new CommandResult(false, "You must specify the path to the file");
            var destination = args[0];
            if (sources.SelectAppropriateSourceForExtension(destination) == null)
                return new CommandResult(false, "Document's format doesn't support");
            if (!File.Exists(destination))
                return new CommandResult(false, "Path to the file is incorrect or doesn't exists");
            sourceSettings.Destination = destination;
            return new CommandResult(true, "Success");
        }
    }
}
