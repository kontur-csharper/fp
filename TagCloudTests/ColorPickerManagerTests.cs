using System;
using System.Collections.Generic;
using System.Drawing;
using Autofac;
using NUnit.Framework;
using TagCloud.Infrastructure.Graphics;
using TagCloud.Infrastructure.Layout;
using TagCloud.Infrastructure.Layout.Environment;
using TagCloud.Infrastructure.Layout.Strategies;
using TagCloud.Infrastructure.Settings;
using TagCloud.Infrastructure.Settings.UISettingsManagers;
using TagCloud.Infrastructure.Settings.UISettingsManagers.Interfaces;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloudTests
{
    [TestFixture]
    public class ColorPickerManagerTests
    {
        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Settings>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<PlainEnvironment>().AsImplementedInterfaces();
            builder.RegisterType<SpiralStrategy>().As<ILayoutStrategy>();
            builder.RegisterType<TagCloudLayouter>().As<ILayouter<Size, Rectangle>>();

            builder.RegisterType<Random>().SingleInstance();
            builder.RegisterType<ColorPicker>();
            builder.RegisterType<ColorConverter>();
            builder.RegisterType<ColorPickerSettingsManager>().AsImplementedInterfaces();

            container = builder.Build();


            var settings = container.Resolve<Settings>();
            settings.Import(new Settings {ColorMap = new Dictionary<WordType, Color>()});

            colorPicker = container.Resolve<IMultiOptionsManager>();
        }

        private IMultiOptionsManager colorPicker;
        private IContainer container;

        [TestCase("A=White", true, TestName = "Correct pair")]
        [TestCase("A=asdasdasd", false, TestName = "Correct wordType, incorrect color")]
        [TestCase("asdasdasd=White", false, TestName = "Incorrect wordType, correct color")]
        [TestCase("A=ffffff", false, TestName = "hex color")]
        [TestCase("A=1", true, TestName = "numeric color")]
        public void TrySet_CheckIsSuccess_OnUserInput(string input, bool expected)
        {
            var result = colorPicker.TrySet(input);

            Assert.That(result.IsSuccess, Is.EqualTo(expected));
        }

        [TestCase("A", "White")]
        [TestCase("V", "Black")]
        public void GetSelectedOptions_AfterSet(string type, string expectedColor)
        {
            colorPicker.TrySet($"{type}={expectedColor}");

            var actual = colorPicker.GetSelectedOptions();

            CollectionAssert.IsNotEmpty(actual);
            Assert.That(actual.TryGetValue(type, out var actualColor), Is.True);
            Assert.That(actualColor, Is.EqualTo(expectedColor));
        }

        [Test]
        public void GetOptions_HasOptions()
        {
            var result = colorPicker.GetOptions();

            CollectionAssert.IsNotEmpty(result);
        }

        [Test]
        public void TrySet_KeepsValidOnly()
        {
            const string input = "A=White; ADV=asdasdasda; V=Black";
            colorPicker.TrySet(input);

            var actual = colorPicker.GetSelectedOptions();

            CollectionAssert.IsNotEmpty(actual);
            Assert.That(actual.TryGetValue("A", out var actualColor), Is.True);
            Assert.That(actualColor, Is.EqualTo("White"));
            Assert.That(actual.TryGetValue("V", out actualColor), Is.True);
            Assert.That(actualColor, Is.EqualTo("Black"));
            Assert.That(actual.TryGetValue("ADV", out actualColor), Is.False);
        }

        [Test]
        public void TrySet_SameKeyIgnoreCase_KeepsLast()
        {
            const string input = "A=White; A=asdasdasda; a=Black";
            colorPicker.TrySet(input);

            var actual = colorPicker.GetSelectedOptions();

            CollectionAssert.IsNotEmpty(actual);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.TryGetValue("A", out var actualColor), Is.True);
            Assert.That(actualColor, Is.EqualTo("Black"));
        }
    }
}