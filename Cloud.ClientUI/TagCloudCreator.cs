using System;
using CloudContainer;
using Microsoft.Extensions.DependencyInjection;
using TagsCloudVisualization;

namespace Cloud.ClientUI
{
    public class TagCloudCreator
    {
        private readonly TagCloudContainerBuilder container;
        private readonly ISaver saver;

        public TagCloudCreator(TagCloudContainerBuilder container, ISaver saver)
        {
            this.container = container;
            this.saver = saver;
        }

        public void Run(TagCloudArguments arguments)
        {
            var visualizationContainer = container.CreateTagCloudContainer(arguments);
            visualizationContainer.BuildServiceProvider().GetService<TagCloudContainer>()
                .GetImage(arguments)
                .Then(x => saver.SaveImage(x, arguments.OutputFileName)
                    .OnFail(Console.WriteLine));
        }
    }
}