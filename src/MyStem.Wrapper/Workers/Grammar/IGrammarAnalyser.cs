using FunctionalStuff.Results;
using MyStem.Wrapper.Workers.Grammar.Raw;

namespace MyStem.Wrapper.Workers.Grammar
{
    public interface IGrammarAnalyser
    {
        Result<AnalysisResultRaw[]> GetRawResult(string text);
    }
}