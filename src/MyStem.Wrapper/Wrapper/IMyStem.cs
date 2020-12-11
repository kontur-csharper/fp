using FunctionalStuff.Results;

namespace MyStem.Wrapper.Wrapper
{
    public interface IMyStem
    {
        Result<string> GetResponse(string text);
    }
}