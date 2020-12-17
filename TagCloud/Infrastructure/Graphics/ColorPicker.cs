using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud.Infrastructure.Settings.SettingsProviders;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Graphics
{
    public class ColorPicker
    {
        private const int MaxColorValue = 256;
        private readonly Func<IColorPickerSettingProvider> colorMapProvider;
        private readonly Random random;
        private readonly Dictionary<WordType, Color> wordTypeColorMap;

        public ColorPicker(Random random, Func<IColorPickerSettingProvider> colorMapProvider)
        {
            this.random = random;
            this.colorMapProvider = colorMapProvider;
            wordTypeColorMap = new Dictionary<WordType, Color>();
        }

        private void UpdateColors()
        {
            if (colorMapProvider().ColorMap == null) return;
            foreach (var pair in colorMapProvider().ColorMap)
                wordTypeColorMap[pair.Key] = pair.Value;
            foreach (var pair in wordTypeColorMap)
                colorMapProvider().ColorMap[pair.Key] = pair.Value;
        }

        public Color GetColor(TokenInfo info)
        {
            UpdateColors();
            if (wordTypeColorMap.TryGetValue(info.WordType, out var color))
                return color;
            color = GetRandomColor();
            wordTypeColorMap.Add(info.WordType, color);
            UpdateColors();
            return color;
        }

        private Color GetRandomColor()
        {
            return Color.FromArgb(random.Next(MaxColorValue), random.Next(MaxColorValue), random.Next(MaxColorValue));
        }
    }
}