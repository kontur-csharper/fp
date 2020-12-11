using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FunctionalStuff;
using FunctionalStuff.Actions;
using FunctionalStuff.Common;
using FunctionalStuff.Results;
using FunctionalStuff.Results.Fails;
using MyStem.Wrapper.Workers.Grammar.Parsing.Models;
using TagCloud.Core;
using TagCloud.Core.Layouting;
using TagCloud.Core.Output;
using TagCloud.Core.Text;
using TagCloud.Core.Text.Formatting;
using TagCloud.Core.Text.Preprocessing;
using TagCloud.Gui.ImageResizing;
using TagCloud.Gui.InputModels;
using TagCloud.Gui.Localization;

namespace TagCloud.Gui
{
    public class App : IApp, IDisposable
    {
        private readonly IUi ui;
        private readonly ITagCloudGenerator cloudGenerator;
        private readonly IUserNotifier notifier;
        private readonly UserInputOneOptionChoice<IFileWordsReader> readerPicker;
        private readonly UserInputMultipleOptionsChoice<IWordFilter> filterPicker;
        private readonly UserInputOneOptionChoice<IWordConverter> normalizerPicker;
        private readonly UserInputOneOptionChoice<IFileResultWriter> writerPicker;
        private readonly UserInputOneOptionChoice<LayouterType> layouterPicker;
        private readonly UserInputOneOptionChoice<FontSizeSourceType> fontSizeSourcePicker;
        private readonly UserInputOneOptionChoice<FontFamily> fontPicker;
        private readonly UserInputOneOptionChoice<IImageResizer> imageResizerPicker;
        private readonly UserInputField filePathInput;
        private readonly UserInputSizeField centerOffsetPicker;
        private readonly UserInputSizeField betweenWordsDistancePicker;
        private readonly UserInputSizeField imageSizePicker;
        private readonly UserInputOneOptionChoice<ImageFormat> imageFormatPicker;
        private readonly UserInputColor backgroundColorPicker;
        private readonly UserInputColorPalette colorPalettePicker;
        private readonly UserInputMultipleOptionsChoice<MyStemSpeechPart> speechPartPicker;

        public App(IUi ui, IUserInputBuilder inputBuilder,
            ITagCloudGenerator cloudGenerator,
            IEnumerable<IFileWordsReader> readers,
            IEnumerable<IWordFilter> filters,
            IEnumerable<IWordConverter> normalizers,
            IEnumerable<IFileResultWriter> writers,
            IEnumerable<IImageResizer> resizers,
            IUserNotifier notifier,
            ISet<MyStemSpeechPart> allowedSpeechPartsHolder)
        {
            this.ui = ui;
            this.cloudGenerator = cloudGenerator;
            this.notifier = notifier;

            readerPicker = inputBuilder.ServiceChoice(readers, UiLabel.FileReader);
            writerPicker = inputBuilder.ServiceChoice(writers, UiLabel.WritingMethod);
            normalizerPicker = inputBuilder.ServiceChoice(normalizers, UiLabel.NormalizationMethod);
            imageResizerPicker = inputBuilder.ServiceChoice(resizers, UiLabel.ResizingMethod);
            filterPicker = inputBuilder.SeveralServicesChoice(filters, UiLabel.FilteringMethod);
            layouterPicker = inputBuilder.EnumChoice<LayouterType>(UiLabel.LayoutingAlgorithm);
            fontSizeSourcePicker = inputBuilder.EnumChoice<FontSizeSourceType>(UiLabel.SizeSource);
            speechPartPicker = inputBuilder.SeveralEnumValuesChoice<MyStemSpeechPart>(UiLabel.SpeechPart);
            speechPartPicker.BindSelectionTo(allowedSpeechPartsHolder);

            backgroundColorPicker = inputBuilder.Color(Color.Khaki, UiLabel.BackgroundColor);
            colorPalettePicker = inputBuilder.ColorPalette(UiLabel.ColorPalette, Color.DarkRed);

            filePathInput = inputBuilder.Field(UiLabel.SourceFile);
            centerOffsetPicker = inputBuilder.Size(UiLabel.LayoutingCenterOffset, true);
            betweenWordsDistancePicker = inputBuilder.Size(UiLabel.LayoutingRectDistance);
            imageSizePicker = inputBuilder.Size(UiLabel.ImageSize);

            fontPicker = inputBuilder.SingleChoice(FontFamily.Families.ToDictionary(x => x.Name), UiLabel.FontFamily);
            imageFormatPicker = inputBuilder.SingleChoice(
                new[] {ImageFormat.Gif, ImageFormat.Png, ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Tiff}
                    .ToDictionary(x => x.ToString()),
                UiLabel.ImageFormat
            );
        }

        public void Subscribe()
        {
            ui.ExecutionRequested += ExecutionRequested;

            ui.AddUserInput(filePathInput);

            ui.AddUserInput(imageSizePicker);
            AddUserInputOrUseDefault(imageResizerPicker);

            AddUserInputOrUseDefault(layouterPicker);

            ui.AddUserInput(backgroundColorPicker);
            ui.AddUserInput(colorPalettePicker);

            AddUserInputOrUseDefault(fontPicker);
            AddUserInputOrUseDefault(fontSizeSourcePicker);

            ui.AddUserInput(speechPartPicker);

            ui.AddUserInput(filterPicker);
            AddUserInputOrUseDefault(normalizerPicker);

            ui.AddUserInput(centerOffsetPicker);
            ui.AddUserInput(betweenWordsDistancePicker);

            AddUserInputOrUseDefault(readerPicker);
            AddUserInputOrUseDefault(writerPicker);
            AddUserInputOrUseDefault(imageFormatPicker);
        }

        private async void ExecutionRequested()
        {
            using var uiLock = ui.StartLockingOperation();
            var cancellationToken = uiLock.CancellationToken;

            var result = await ReadWordsAsync(filePathInput.Value, cancellationToken);

            var imageCreationResult = await result
                .FailIf().TokenCanceled(cancellationToken)
                .Then(x => CreateImageAsync(x, cancellationToken)).Unwrap();

            imageCreationResult
                .ThenDo(i => ui.OnAfterWordDrawn(i, backgroundColorPicker.Picked))
                .ThenDo(UpdateImage)
                .OnFail(Do.NothingWhen(FailMessages.CancellationRequested).Else(notifier.Notify));
        }

        private void UpdateImage(Image i)
        {
            if (imageSizePicker.Height > 0 && imageSizePicker.Width > 0)
            {
                using var resized = imageResizerPicker.Selected.Value.Resize(i, imageSizePicker.SizeFromCurrent());
                FillBackgroundAndSave(resized, backgroundColorPicker.Picked);
            }
            else FillBackgroundAndSave(i, backgroundColorPicker.Picked);
        }

        private async Task<Result<Image>> CreateImageAsync(Dictionary<string, int> words, CancellationToken token) =>
            await cloudGenerator.DrawWordsAsync(
                fontSizeSourcePicker.Selected.Value,
                layouterPicker.Selected.Value,
                colorPalettePicker.PickedColors.ToArray(),
                words,
                fontPicker.Selected.Value,
                centerOffsetPicker.PointFromCurrent(),
                betweenWordsDistancePicker.SizeFromCurrent(),
                token
            );

        private async Task<Result<Dictionary<string, int>>> ReadWordsAsync(string path, CancellationToken token) =>
            await ResultOfAsync.Task(() => readerPicker.Selected.Value.GetWordsFrom(path), token)
                .ContinueWith(task => task
                    .WaitResult()
                    .Then(w => Fail.If(w, "Words reading result").NullOrEmpty())
                    .Then(ApplyFilters)
                    .FailIf().TokenCanceled(token)
                    .Then(normalizerPicker.Selected.Value.Normalize)
                    .Then(x => x.ToLookup(y => y))
                    .Then(x => x.ToDictionary(y => y.Key, y => y.Count())));

        private string[] ApplyFilters(ICollection<string> rw) =>
            filterPicker.Selected.Aggregate(rw, (current, filter) =>
                    filter.GetValidWordsOnly(current)
                        .Then(vw => current = vw)
                        .RefineError($"Failed to apply filter {filter.GetType().Name}")
                        .GetValueOr(notifier.Notify, current))
                .ToArray();

        private void FillBackgroundAndSave(Image image, Color backgroundColor)
        {
            using var newImage = new Bitmap(image.Size.Width, image.Size.Height);
            using (var g = Graphics.FromImage(newImage))
            using (var brush = new SolidBrush(backgroundColor))
            {
                g.FillRectangle(brush, new Rectangle(Point.Empty, image.Size));
                g.DrawImage(image, Point.Empty);
            }

            var selectedFormat = imageFormatPicker.Selected.Value;
            writerPicker.Selected.Value.Save(newImage,
                selectedFormat,
                filePathInput.Value + "." + selectedFormat.ToString().ToLower());
        }

        private void AddUserInputOrUseDefault<T>(UserInputOneOptionChoice<T> input)
        {
            if (input.Available.Length > 1) ui.AddUserInput(input);
            else input.SetSelected(input.Available.Single().Name);
        }

        public void Dispose()
        {
            cloudGenerator.Dispose();
        }
    }
}