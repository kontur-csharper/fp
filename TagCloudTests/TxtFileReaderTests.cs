﻿using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.FileReaders;

namespace TagCloudTests
{
    public class TxtFileReaderTests
    {
        private TxtFileReader reader;

        [SetUp]
        public void SetUp()
        {
            reader = new TxtFileReader();
        }

        [Test]
        public void GetWordsFromFile_ShouldWorksCorrect_WhenReadWordsFromFile()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "TestsResourses\\file1.txt");

            var result = reader.GetWordsFromFile(filePath);

            result.GetValueOrThrow().Should().BeEquivalentTo("всем", "привет", "меня", "зовут", "данил");
        }

        [Test]
        public void GetWordsFromFile_ShouldThrow_WhenFileDoesNotExists()
        {
            reader.GetWordsFromFile("bla bla file").IsSuccess.Should().BeFalse();
        }
    }
}
