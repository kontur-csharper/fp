using System.Collections.Generic;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers.Interfaces
{
    public interface IMultiInputModifierManager : IInputManager
    {
        public Dictionary<string, IEnumerable<string>> GetModifiers();

        public Result<string> ApplyModifier(string type, string modifier);
    }
}