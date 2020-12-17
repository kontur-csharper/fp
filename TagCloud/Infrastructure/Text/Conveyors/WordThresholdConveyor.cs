using System;
using System.Collections.Generic;
using System.Linq;
using TagCloud.Infrastructure.Settings.SettingsProviders;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Text.Conveyors
{
    public class WordThresholdConveyor : IConveyor
    {
        private readonly Func<IWordCountThresholdSettingProvider> wordCountThresholdProvider;

        public WordThresholdConveyor(Func<IWordCountThresholdSettingProvider> wordCountThresholdProvider)
        {
            this.wordCountThresholdProvider = wordCountThresholdProvider;
        }

        public IEnumerable<TokenInfo> Handle(IEnumerable<TokenInfo> tokens)
        {
            var threshold = wordCountThresholdProvider().WordCountThreshold;
            return tokens.Where(pair => pair.Frequency > threshold);
        }
    }
}