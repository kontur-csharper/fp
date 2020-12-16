using System;
using System.IO;
using Autofac;

namespace TagsCloudContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.GetDirectoryName(
                Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            var creator = Configurator.GetContainer()
                .Then(cont => cont.BeginLifetimeScope())
                .Then(scope => scope.Resolve<TagsCloudCreator>())
                .OnFail(err => Console.WriteLine(err));
            if (!creator.IsSuccess)
                return;
            var mainResult = Result.Of(() => creator.Value.SetFontRandomColor())
                .Then(res => creator.Value.SetImageFormat("png"))
                .Then(res => creator.Value.SetFontFamily("Comic Sans MS"))
                .Then(res => creator.Value.SetImageSize(200))
                .Then(res => creator.Value.AddStopWord("aba"))
                .Then(res => creator.Value.Create(Path.Combine(path, "input.txt"), path, "Cloud2"))
                .OnFail(err => Console.WriteLine(err));
        }
    }
}
