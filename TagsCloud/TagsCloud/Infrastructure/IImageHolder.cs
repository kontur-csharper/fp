using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloud.Layouters;

namespace TagsCloud.Infrastructure
{
    public interface IImageHolder
    {
        event EventHandler Error;
        void OnError();
        string SettingsErrorMessage { get; }
        ImageSettings Settings { get; }
        Result<None> ChangeLayouter(ICloudLayouter layouter);
        void RecreateCanvas(ImageSettings settings);
        Result<None> SaveImage(string fileName);
        Result<None> RenderWordsFromFile(string fileName);
        Result<None> RedrawCurrentImage();
    }
}