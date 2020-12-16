using Autofac;

namespace HomeExercise
{
    public interface IConsoleCloudClient
    {
        void HandleSettingsFromConsole(string[] args, ContainerBuilder builder);
    }
}