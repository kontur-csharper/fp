using System.Drawing;

namespace TagCloud.Infrastructure.Text.Information
{
    public class TokenInfo
    {
        public string Token;

        public TokenInfo(string token, WordType wordType = default, int frequency = default, int fontSize = default,
            Size size = default)
        {
            Token = token;
            WordType = wordType;
            Frequency = frequency;
            FontSize = fontSize;
            Size = size;
        }

        public WordType WordType { get; }
        public int Frequency { get; set; }
        public int FontSize { get; set; }
        public Size Size { get; set; }
    }
}