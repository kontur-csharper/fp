﻿using TagsCloudContainer.WordCounters;
using TagsCloudContainer.Visualizers;
using TagsCloudContainer.Readers;
using System.Drawing;
using TagsCloudContainer.WordPreprocessors;
using System;
using System.Linq;

namespace TagsCloudContainer.TagsCloudGenerators
{
    public class TagsCloudGenerator
    {
        private IWordCounter wordCounter;
        private IVisualizer visualizer;
        private IReader reader;
        private IWordPreprocessor wordPreprocessor;

        public TagsCloudGenerator(
            IWordCounter wordCounter,
            IVisualizer visualizer,
            IReader reader,
            IWordPreprocessor wordPreprocessor)
        {
            this.wordCounter = wordCounter;
            this.visualizer = visualizer;
            this.reader = reader;
            this.wordPreprocessor = wordPreprocessor;
        }

        public Result<Bitmap> CreateTagCloud()
        {
            return reader.ReadAllLines()
                .Then(wordPreprocessor.WordPreprocessing)
                .Then(wordCounter.CountWords)
                .Then(visualizer.VisualizeCloud);
        }
    }
}
