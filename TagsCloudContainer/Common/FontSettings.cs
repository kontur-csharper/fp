using System.Drawing;

namespace TagsCloudContainer.Common
{
    public class FontSettings : ISettings
    {
        private static readonly int defaultMaxFontSize = 43;
        private static readonly int defaultMinFontSize = 6;
        private static readonly string defaultFontName = "Times New Roman";
        public int MaxFontSize { get; set; } = defaultMaxFontSize;

        public int MinFontSize { get; set; } = defaultMinFontSize;
        public string FontName { get; set; } = defaultFontName;

        public Result<ISettings> CheckSettings()
        {
            if (MaxFontSize <= 0)
            {
                MaxFontSize = defaultMaxFontSize;
                return new Result<ISettings>("Размер шрифта должен быть больше 0");
            }

            if (MaxFontSize < MinFontSize)
            {
                MaxFontSize = defaultMaxFontSize;
                return new Result<ISettings>("Максимальный размер шрифта должен быть не меньше минимального");
            }

            if (MinFontSize <= 0)
            {
                MinFontSize = defaultMinFontSize;
                return new Result<ISettings>("Размер шрифта должен быть больше 0");
            }

            var font = new Font(FontName, 1);
            if (font.Name != FontName)
            {
                FontName = defaultFontName;
                return new Result<ISettings>("Шрифт с таким именем не найден в системе");
            }

            return new Result<ISettings>(null, this);
        }
    }
}