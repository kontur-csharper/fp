using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagCloud.Infrastructure.Settings.SettingsProviders;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Text.Conveyors
{
    public class WordSizeConveyor : IConveyor
    {
        private readonly Func<IFontSettingProvider> fontSettingProvider;

        public WordSizeConveyor(Func<IFontSettingProvider> fontSettingProvider)
        {
            this.fontSettingProvider = fontSettingProvider;
        }

        public IEnumerable<TokenInfo> Handle(IEnumerable<TokenInfo> tokens)

        {
            return tokens.Select(info =>
            {
                var fontSize = info.FontSize;
                using var font = new Font(fontSettingProvider().FontFamily, fontSize);
                info.Size = TextRenderer.MeasureText(info.Token, font);
                return info;
            })
                .GroupBy(info => info.Token)
                .Select(x => x.FirstOrDefault());
        }
    }
}