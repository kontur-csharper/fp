using System.Drawing;

namespace HomeExercise.Helpers
{
    public static class FontFamilyHelper
    {
        public static FontFamily GetFontFamily(string stringRepresentations)
        {
            return new FontFamily(stringRepresentations);
        }
    }
}