using FunctionalStuff.Results;

namespace MyStem.Wrapper.Workers.Lemmas
{
    public interface ILemmatizer
    {
        Result<string[]> GetWords(string text);
    }
}