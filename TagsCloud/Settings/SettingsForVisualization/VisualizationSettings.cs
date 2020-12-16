using System.Drawing;

namespace TagsCloud.Settings.SettingsForVisualization
{
    public class VisualizationSettings : IVisualizationSettings
    {
        public Size ImageSize { get; }
        public Color BackgroundColor { get; }
        public Color TextColor { get; }
        public Font Font { get; }

        public VisualizationSettings(Size imageSize, Color backgroundColor, Color textColor, Font font)
        {
            ImageSize = imageSize;
            BackgroundColor = backgroundColor;
            TextColor = textColor;
            Font = font;
        }
    }
}