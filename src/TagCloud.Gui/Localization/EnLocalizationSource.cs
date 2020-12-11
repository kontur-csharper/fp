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
    public class EnLocalizationSource : ILocalizationSource
    {
        private static readonly Dictionary<Type, string> overridingNames = new Dictionary<Type, string>
        {
            {typeof(RandomFontSizeSource), "Random font size"},
            {typeof(BiggerAtCenterFontSizeSource), "Most frequent bigger and closer to center"},
            {typeof(BlacklistWordFilter), "Without \"boring\" words"},
            {typeof(MyStemWordsConverter), "Yadnex MyStem"},
            {typeof(LengthWordFilter), "Only with length more or equal to 3"},
            {typeof(LowerCaseConverter), "Lower cased"},
            {typeof(FileWordsReader), "Text file"},
            {typeof(FileResultWriter), "Save to file"},
            {typeof(DontModifyImageResizer), "Save as it is"},
            {typeof(FitToSizeImageResizer), "Fit to size"},
            {typeof(StretchImageResizer), "Stretch to size"},
            {typeof(PlaceAtCenterImageResizer), "Place at center or fit to size"}
        };

        public CultureInfo ForCulture => CultureInfo.GetCultureInfo("en");
        public bool TryGet(Type type, out string value) => overridingNames.TryGetValue(type, out value);

        public bool TryGetLabel(UiLabel key, out string value)
        {
            value = key switch
            {
                UiLabel.FileReader => "Words file reader",
                UiLabel.FilteringMethod => "Words filtering method",
                UiLabel.WritingMethod => "Result writing method",
                UiLabel.NormalizationMethod => "Words normalization method",
                UiLabel.ResizingMethod => "Resizing method",
                UiLabel.TypeFilter => "Speech type filter",
                UiLabel.SizeSource => "Font size source",
                UiLabel.SourceFile => "Enter source file path",
                UiLabel.FontFamily => "Font family",
                UiLabel.LayoutingAlgorithm => "Layouting algorithm",
                UiLabel.LayoutingCenterOffset => "Cloud center offset",
                UiLabel.LayoutingRectDistance => "Minimal distance between rectangles",
                UiLabel.ImageSize => "Result image size",
                UiLabel.BackgroundColor => "Image background color",
                UiLabel.ColorPalette => "Words color palette",
                UiLabel.SpeechPart => "Speech parts rules",
                UiLabel.ImageFormat => "Result image format",
                UiLabel.ButtonAdd => "Add",
                UiLabel.ButtonRemove => "Remove",
                UiLabel.XPoint => "X coord",
                UiLabel.YPoint => "Y coord",
                UiLabel.Width => "Width",
                UiLabel.Height => "Height",
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
            MyStemSpeechPart.Unrecognized => "Unrecognized",
            MyStemSpeechPart.Adjective => "Adjective",
            MyStemSpeechPart.Adverb => "Adverb",
            MyStemSpeechPart.PronominalAdverb => "Pronominal Adverb",
            MyStemSpeechPart.PronounNumeral => "Pronoun Numeral",
            MyStemSpeechPart.PronounAdjective => "Pronoun Adjective",
            MyStemSpeechPart.CompositeWordPart => "Part of Composite word",
            MyStemSpeechPart.Union => "Union",
            MyStemSpeechPart.Interjection => "Interjection",
            MyStemSpeechPart.Numeral => "Numeral",
            MyStemSpeechPart.Particle => "Particle",
            MyStemSpeechPart.Pretext => "Pretext",
            MyStemSpeechPart.Noun => "Noun",
            MyStemSpeechPart.Pronoun => "Pronoun",
            MyStemSpeechPart.Verb => "Verb",
            _ => string.Empty
        };

        private static string Get(FontSizeSourceType sizeSourceType) => sizeSourceType switch
        {
            FontSizeSourceType.Random => "Randomized",
            FontSizeSourceType.FrequentIsBigger => "More frequent is bigger",
            _ => string.Empty
        };
    }
}