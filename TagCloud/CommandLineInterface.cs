using System.Drawing;
using McMaster.Extensions.CommandLineUtils;

namespace TagCloud
{
    public class CommandLineInterface
    {
        public Color StringColor { get; private set; }
        public FontFamily StringFont { get; private set; }
        public string FileName { get; private set; }
        public Size CanvasSize { get; private set; }
        public Background BackgroundType { get; private set; }

        public CommandLineInterface()
        {
            CanvasSize = new Size(1000, 800);
            BackgroundType = Background.Empty;
            FileName = "input.txt";
            StringFont = new FontFamily("Arial");
            StringColor = Color.Black;

        }

        public void ConfigureCLI(CommandLineApplication app)
        {
            app.HelpOption();
            var optionInput = app.Option("-i|--input <INPUT>", "input filename", CommandOptionType.SingleValue);
            var optionFont = app.Option("-f|--font <FONT>", "font family", CommandOptionType.SingleValue);
            var optionSize = app.Option("-s|--size <SIZE>", "size of image width,height", CommandOptionType.SingleValue);
            var optionBackground = app.Option("-b|--backgound <BACKGROUND_STYLE>", "background style rectangles|empty|circle", CommandOptionType.SingleValue);
            var optionStringColor = app.Option("-c|--color <COLOR>", "string color r,g,b", CommandOptionType.SingleValue);
            
            app.OnExecute(() =>
            {
                if (optionSize.HasValue())
                {
                    var sizeResult = ArgumentParser.GetSize(optionSize.Value());
                    if (sizeResult.IsSuccess)
                        CanvasSize = sizeResult.Value;
                }

                if (optionBackground.HasValue())
                {
                    var backgroundResult = ArgumentParser.GetBackground(optionBackground.Value());
                    if (backgroundResult.IsSuccess)
                        BackgroundType = backgroundResult.Value;
                }

                if (optionInput.HasValue() )
                {
                    var filenameResult = ArgumentParser.CheckFileName(optionInput.Value());
                    if (filenameResult.IsSuccess)
                        FileName = filenameResult.Value;
                }

                if (optionFont.HasValue())
                {
                    var fontResult = ArgumentParser.GetFont(optionFont.Value());
                    if (fontResult.IsSuccess)
                        StringFont = fontResult.Value;
                }

                if (optionStringColor.HasValue())
                {
                    var colorResult = ArgumentParser.ParseColor(optionStringColor.Value());
                    if (colorResult.IsSuccess)
                        StringColor = colorResult.Value;
                }

                return 1;
            });
        }
    }
}