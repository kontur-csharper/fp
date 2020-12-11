using System;
using FunctionalStuff.Results;
using FunctionalStuff.Results.Fails;
using MyStem.Wrapper.Enums;
using MyStem.Wrapper.Wrapper;

namespace MyStem.Wrapper.Workers.Lemmas
{
    public class Lemmatizer : ILemmatizer
    {
        private readonly Result<IMyStem> myStem;

        public Lemmatizer(IMyStemBuilder myStemBuilder)
        {
            myStem = myStemBuilder.Create(MyStemOutputFormat.Text,
                MyStemOptions.WithoutOriginalForm, MyStemOptions.WithContextualDeHomonymy);
        }

        public Result<string[]> GetWords(string text) =>
            Fail.If(text, "Lemmatizer input").NullOrEmpty()
                .ThenJoin(myStem, (t, ms) => new {Text = t, MyStem = ms})
                .Then(x => x.MyStem.GetResponse(x.Text))
                .Then(r => Fail.If(r, "MyStem output").NullOrEmpty())
                .Then(r => r.Substring(1, r.Length - 2))
                .Then(r => r.Split(new[] {"}{"}, StringSplitOptions.RemoveEmptyEntries));
    }
}