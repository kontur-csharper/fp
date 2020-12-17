namespace TagCloud.Infrastructure.Settings.UISettingsManagers.Interfaces
{
    public interface IInputManager
    {
        public string Title { get; }
        public string Help { get; }
        public Result<string> TrySet(string input);
        public string Get();
    }
}