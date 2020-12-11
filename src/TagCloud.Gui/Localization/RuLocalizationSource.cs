using System;
using System.Collections.Generic;
using System.Globalization;
using MyStem.Wrapper.Workers.Grammar.Parsing.Models;
using TagCloud.Core.Output;
using TagCloud.Core.Text;
using TagCloud.Core.Text.Formatting;
using TagCloud.Core.Text.Preprocessing;
using TagCloud.Gui.ImageResizing;

namespace TagCloud.Gui.Localization
{
    public class RuLocalizationSource : ILocalizationSource
    {
        private static readonly Dictionary<Type, string> overridingNames = new Dictionary<Type, string>
        {
            {typeof(RandomFontSizeSource), "Случайный размер шрифта"},
            {typeof(BiggerAtCenterFontSizeSource), "Наиболее частые слова - больше"},
            {typeof(BlacklistWordFilter), "Без \"скучных\" слов"},
            {typeof(MyStemWordsConverter), "Yadnex MyStem"},
            {typeof(LengthWordFilter), "Слова длиннее 3 символов"},
            {typeof(LowerCaseConverter), "В нижнем регистре"},
            {typeof(FileWordsReader), "Из текстового файла"},
            {typeof(FileResultWriter), "Сохранять в файл"},
            {typeof(DontModifyImageResizer), "Игнорировать размер"},
            {typeof(FitToSizeImageResizer), "Уместить в размер"},
            {typeof(StretchImageResizer), "Растянуть до размера"},
            {typeof(PlaceAtCenterImageResizer), "Разместить в центре или сжать до размера"}
        };

        public CultureInfo ForCulture => CultureInfo.GetCultureInfo("ru");
        public bool TryGet(Type type, out string value) => overridingNames.TryGetValue(type, out value);

        public bool TryGetLabel(UiLabel key, out string value)
        {
            value = key switch
            {
                UiLabel.FileReader => "Способ чтения слов",
                UiLabel.FilteringMethod => "Фильтрация слов",
                UiLabel.WritingMethod => "Сохранение результат",
                UiLabel.NormalizationMethod => "Нормализовать слова",
                UiLabel.ResizingMethod => "Преобразователь размера",
                UiLabel.TypeFilter => "Фильтр частей речи",
                UiLabel.SizeSource => "Способ определения размера шрифта",
                UiLabel.SourceFile => "Путь к файлу",
                UiLabel.FontFamily => "Шрифт",
                UiLabel.LayoutingAlgorithm => "Алгоритм раскладки",
                UiLabel.LayoutingCenterOffset => "Смещение центра",
                UiLabel.LayoutingRectDistance => "Расстояние между словами",
                UiLabel.ImageSize => "Размер изображения",
                UiLabel.BackgroundColor => "Цвет фона",
                UiLabel.ColorPalette => "Цвета слов",
                UiLabel.SpeechPart => "Разрешенные части речи",
                UiLabel.ImageFormat => "Формат изображения",
                UiLabel.ButtonAdd => "Добавить",
                UiLabel.ButtonRemove => "Удалить",
                UiLabel.XPoint => "X",
                UiLabel.YPoint => "Y",
                UiLabel.Width => "Ширина",
                UiLabel.Height => "Высота",
                _ => string.Empty
            };

            return !string.IsNullOrWhiteSpace(value);
        }

        public bool TryGet<T>(T enumItem, out string value) where T : struct, Enum
        {
            value = enumItem switch
            {
                MyStemSpeechPart speechPart => Get(speechPart),
                FontSizeSourceType fontSizeSourceType => Get(fontSizeSourceType),
                _ => string.Empty
            };
            return !string.IsNullOrWhiteSpace(value);
        }

        private static string Get(MyStemSpeechPart speechPart) => speechPart switch
        {
            MyStemSpeechPart.Unrecognized => "Не распознали",
            MyStemSpeechPart.Adjective => "Прилагательное",
            MyStemSpeechPart.Adverb => "Наречие",
            MyStemSpeechPart.PronominalAdverb => "Местоименное наречие",
            MyStemSpeechPart.PronounNumeral => "Числительное-прилагательное",
            MyStemSpeechPart.PronounAdjective => "Местоимение-прилагательное",
            MyStemSpeechPart.CompositeWordPart => "Часть сложного слова",
            MyStemSpeechPart.Union => "Союз",
            MyStemSpeechPart.Interjection => "Междометие",
            MyStemSpeechPart.Numeral => "Числительное",
            MyStemSpeechPart.Particle => "Частица",
            MyStemSpeechPart.Pretext => "Предлог",
            MyStemSpeechPart.Noun => "Существительно",
            MyStemSpeechPart.Pronoun => "Местоимение-существительное",
            MyStemSpeechPart.Verb => "Глагол",
            _ => string.Empty
        };

        private static string Get(FontSizeSourceType sizeSourceType) => sizeSourceType switch
        {
            FontSizeSourceType.Random => "Случайно",
            FontSizeSourceType.FrequentIsBigger => "Более частые в центре",
            _ => string.Empty
        };
    }
}