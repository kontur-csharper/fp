using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagsCloudCreating.Contracts;
using TagsCloudCreating.Core.WordProcessors;
using TagsCloudCreating.Infrastructure;
using TagsCloudVisualization.App;
using TagsCloudVisualization.Contracts;
using TagsCloudVisualization.Infrastructure.Common;

namespace TagsCloudVisualization.MenuItems
{
    public class TagsCloudVisualizer : IMenuItem
    {
        public string MenuAffiliation => "Draw";
        public string ItemName => "Cloud from file...";
        private ITagsCloudCreator CloudCreator { get; }
        private IWordsReader Reader { get; }
        private TagsCloudPictureHolder PictureHolder { get; }

        public TagsCloudVisualizer(
            ITagsCloudCreator cloudCreator,
            IWordsReader reader,
            TagsCloudPictureHolder pictureHolder)
        {
            CloudCreator = cloudCreator;
            Reader = reader;
            PictureHolder = pictureHolder;
        }

        public DialogResult Execute()
        {
            CreateImage();
            return DialogResult.OK;
        }

        private void CreateImage() => GetUserFile().AsResult()
            .Then(Reader.GetAllData)
            .Then(CloudCreator.CreateTagsCloud)
            .Then(DrawTags)
            .OnFail(InformationMessageHelper.ShowExceptionMessage);

        private Result<None> DrawTags(IEnumerable<Tag> tags)
        {
            var listTags = tags.ToList();

            var realImageSize = GetRealImageSize(listTags);
            if (PictureHolder.Image.Size.Width < realImageSize.Width ||
                PictureHolder.Image.Size.Height < realImageSize.Height)
                return Result.Fail<None>("Tag's Cloud is very big! Please, resize your image or tag's font.");

            var graphics = Graphics.FromImage(PictureHolder.Image);
            var imageBackground = new Rectangle(0, 0, PictureHolder.Image.Width, PictureHolder.Image.Height);

            graphics.FillRectangle(new SolidBrush(PictureHolder.BackColor), imageBackground);
            foreach (var tag in listTags)
            {
                graphics.DrawString(tag.Word, tag.Font, new SolidBrush(tag.Color), tag.Frame);
                PictureHolder.UpdateUi();
            }

            return Result.Ok();
        }

        private static Size GetRealImageSize(List<Tag> listTags)
        {
            var leftBorder = listTags.Min(t => t.Frame.Left);
            var rightBorder = listTags.Max(t => t.Frame.Right);
            var topBorder = listTags.Min(t => t.Frame.Top);
            var bottomBorder = listTags.Max(t => t.Frame.Bottom);

            return new Size(rightBorder - leftBorder, bottomBorder - topBorder);
        }

        private static string GetUserFile()
        {
            var fileInput = new OpenFileDialog
            {
                Title = "Source for tags cloud",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                Filter = "Text files(*.txt)|*.txt"
            };
            fileInput.ShowDialog();
            return fileInput.FileName;
        }
    }
}