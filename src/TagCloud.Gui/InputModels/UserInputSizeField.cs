using System.Drawing;

namespace TagCloud.Gui.InputModels
{
    public class UserInputSizeField
    {
        public UserInputSizeField(string description, string xInputLabel, string yInputLabel)
        {
            Description = description;
            XInputLabel = xInputLabel;
            YInputLabel = yInputLabel;
        }

        public string Description { get; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string XInputLabel { get; }
        public string YInputLabel { get; }

        public Point PointFromCurrent() => new Point(Width, Height);
        public Size SizeFromCurrent() => new Size(Width, Height);
    }
}