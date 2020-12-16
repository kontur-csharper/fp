using MatthiWare.CommandLine;
using TagsCloud.UserCommands;

namespace TagsCloud.Clients
{
    public class ConsoleClient : IClient
    {
        public bool TryGetUserCommands(string[] args, out AllUserCommands commands)
        {
            var parsed = new CommandLineParser<AllUserCommands>().Parse(args);
            if (parsed.HasErrors || parsed.HelpRequested)
            {
                commands = default;
                return false;
            }

            commands = parsed.Result;
            return true;
        }
    }
}