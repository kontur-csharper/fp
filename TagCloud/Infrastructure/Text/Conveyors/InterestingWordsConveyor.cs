using System;
using System.Collections.Generic;
using System.Linq;
using TagCloud.Infrastructure.Settings.SettingsProviders;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Text.Conveyors
{
    public class InterestingWordsConveyor : IConveyor
    {
        private readonly Func<IExcludeTypesSettingsProvider> excludeTypesSettingsProvider;

        public InterestingWordsConveyor(Func<IExcludeTypesSettingsProvider> excludeTypesSettingsProvider)
        {
            this.excludeTypesSettingsProvider = excludeTypesSettingsProvider;
        }

        public IEnumerable<TokenInfo> Handle(IEnumerable<TokenInfo> tokens)
        {
            return tokens.Where(pair => IsInteresting(pair.WordType));
        }

        private bool IsInteresting(WordType type)
        {
            return !excludeTypesSettingsProvider().ExcludedTypes.Contains(type);
        }
    }
}