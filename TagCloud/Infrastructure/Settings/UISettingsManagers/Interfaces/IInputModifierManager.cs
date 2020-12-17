using System.Collections.Generic;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers.Interfaces
{
    public interface IInputModifierManager : IInputManager
    {
        public IEnumerable<string> GetModifiers();

        public Result<string> ApplyModifier(string modifier);
    }
}