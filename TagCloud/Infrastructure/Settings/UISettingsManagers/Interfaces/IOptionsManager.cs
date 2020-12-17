using System.Collections.Generic;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers.Interfaces
{
    public interface IOptionsManager : IInputManager
    {
        public IEnumerable<string> GetOptions();
        public string GetSelectedOption();
    }
}