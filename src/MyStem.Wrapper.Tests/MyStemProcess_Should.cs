using System;
using System.IO;
using FluentAssertions;
using FunctionalStuff.Results;
using FunctionalStuff.TestingExtensions;
using MyStem.Wrapper.Enums;
using MyStem.Wrapper.Wrapper;
using NUnit.Framework;

namespace MyStem.Wrapper.Tests
{
    // ReSharper disable once InconsistentNaming
    public class MyStemProcess_Should
    {
        private string path;
        private IMyStem process;

        [SetUp]
        public void Setup()
        {
            path = Path.Combine(
                TestContext.CurrentContext.TestDirectory,
                $"{Guid.NewGuid()}.exe"
            );

            File.Copy(
                Path.Combine(TestContext.CurrentContext.WorkDirectory, "../../../../dlls/", "mystem.exe"),
                path,
                overwrite: true
            );

            process = new MyStemBuilder(path).Create(MyStemOutputFormat.Text).GetValueOrThrow();
        }

        [Test]
        public void FileMissing_ShouldThrow()
        {
            File.Delete(path);

            process.GetResponse("бибочка")
                .ShouldBeFailed()
                .Which
                .Error
                .Should()
                .ContainAll(path);
        }

        [Test]
        public void FileExists_ShouldNotThrow()
        {
            process.GetResponse("бибочка")
                .ShouldBeSuccessful();
        }

        [Test]
        public void ReturnCorrectResponse()
        {
            process.GetResponse("упячки")
                .Value()
                .Should()
                .Be("упячки{упячка?}");
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(path);
        }
    }
}