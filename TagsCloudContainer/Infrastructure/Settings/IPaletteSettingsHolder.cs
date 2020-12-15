using System.Drawing;

namespace TagsCloudContainer.Infrastructure.Settings
{
    public interface IPaletteSettingsHolder
    {
        public Color TextColor { get; }
        public Color BackgroundColor { get; }
    }
}
