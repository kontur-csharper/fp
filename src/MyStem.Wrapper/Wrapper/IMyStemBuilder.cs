using FunctionalStuff.Results;
using MyStem.Wrapper.Enums;

namespace MyStem.Wrapper.Wrapper
{
    public interface IMyStemBuilder
    {
        Result<IMyStem> Create(MyStemOutputFormat outputFormat, params MyStemOptions[] args);
    }
}