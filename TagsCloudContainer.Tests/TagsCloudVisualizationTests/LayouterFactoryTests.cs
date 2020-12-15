using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.TagsCloudVisualization;
using TagsCloudContainer.TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.Tests.TagsCloudVisualizationTests
{
    public class LayouterFactoryTests
    {
        private LayouterFactory LayouterFactory { get; set; }

        [SetUp]
        public void SetUp()
        {
            var spiral = new UlamSpiral(new Point(0, 0));
            LayouterFactory = new LayouterFactory(new List<ISpiral> {spiral});
        }

        [Test]
        public void GetLayouter_ThrowException_WhenCalled()
        {
            Action layouter = () => LayouterFactory.GetLayouter(SpiralType.Archimedean);

            layouter.Should().Throw<ArgumentException>();
        }

        [Test]
        public void GetLayouter_NotThrowException_WhenCalled()
        {
            Action layouter = () => LayouterFactory.GetLayouter(SpiralType.UlamSpiral);

            layouter.Should().NotThrow();
        }

        [Test]
        public void GetLayouter_ReturnNotNull_WhenCalled()
        {
            LayouterFactory.GetLayouter(SpiralType.UlamSpiral).Should().NotBeNull();
        }
    }
}