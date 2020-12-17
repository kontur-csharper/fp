using System.Collections.Generic;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Settings.SettingsProviders
{
    public interface IExcludeTypesSettingsProvider
    {
        public IEnumerable<WordType> ExcludedTypes { get; }
    }
}