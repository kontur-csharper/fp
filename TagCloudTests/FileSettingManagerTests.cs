using System;
using System.IO;
using Autofac;
using NUnit.Framework;
using TagCloud.Infrastructure.Settings;
using TagCloud.Infrastructure.Settings.UISettingsManagers;
using TagCloud.Infrastructure.Settings.UISettingsManagers.Interfaces;
using TagCloud.Infrastructure.Text;

namespace TagCloudTests
{
    [TestFixture]
    public class FileSettingManagerTests
    {
        [SetUp]
        public void SetUp()
        {
            builder = new ContainerBuilder();
            builder.RegisterType<TxtReader>().As<IReader<string>>();

            builder.RegisterType<Settings>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<FileSettingManager>().AsImplementedInterfaces();
            container = builder.Build();
            fileSettingManager = container.Resolve<IInputManager>() as FileSettingManager;

            validPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TagCloudTests.dll");
            Assert.True(IsExistingFile(validPath), $"Tests are incorrect! file not found: {validPath}");
        }

        private ContainerBuilder builder;
        private FileSettingManager fileSettingManager;
        private IContainer container;
        private string validPath;

        private bool IsExistingFile(string path)
        {
            return File.Exists(path);
        }

        [Test]
        public void Get_AfterSet()
        {
            var expected = validPath;
            var result = fileSettingManager.TrySet(expected);

            var actual = fileSettingManager.Get();

            Assert.True(result.IsSuccess, $"Path {expected} was not set!");
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Get_WhenDefault()
        {
            var expected = container.Resolve<Func<Settings>>()().Path;

            var actual = fileSettingManager.Get();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("asdasd", false, TestName = "FileDoesNotExists")]
        [TestCase(".", false, TestName = "Is Current Directory")]
        [TestCase("..", false, TestName = "Is Parent Directory")]
        public void TrySet_CorrectFileSet(string path, bool expected)
        {
            var actual = fileSettingManager.TrySet(path);

            Assert.That(actual.IsSuccess, Is.EqualTo(expected));
        }

        [Test]
        public void TrySet_WhenExists()
        {
            var path = validPath;

            var actual = fileSettingManager.TrySet(path);

            Assert.IsTrue(actual.IsSuccess, $"{Path.GetFullPath(path)} not found");
        }
    }
}