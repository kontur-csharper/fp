using System.Drawing;

namespace TagsCloud.Settings.SettingsForVisualization
{
    public interface IVisualizationSettings
    {
        Size ImageSize { get; }
        Color BackgroundColor { get; }
        Color TextColor { get; }
        Font Font { get; }
    }
}