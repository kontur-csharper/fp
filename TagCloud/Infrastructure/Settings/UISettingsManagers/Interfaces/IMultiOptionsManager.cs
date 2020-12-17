using System.Collections.Generic;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers.Interfaces
{
    public interface IMultiOptionsManager : IInputManager
    {
        public Dictionary<string, IEnumerable<string>> GetOptions();
        public Dictionary<string, string> GetSelectedOptions();
    }
}