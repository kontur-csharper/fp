﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudCreating.Configuration;
using TagsCloudCreating.Core.WordProcessors;

namespace TagsCloudTests.WordProcessorsTests
{
    [TestFixture]
    public class WordHandler_Should
    {
        private readonly Dictionary<PartsOfSpeech, bool> defaultSpeechPartsStatuses =
            new WordHandlerSettings().SpeechPartsStatuses;

        [Test]
        public void NormilizeAndExcludeBoringWords_AllWordsInteresting_ReturnsAllInputWords()
        {
            var sentence =
                "Утром в лесу запели два звонких соловья и защебетали воробьи, чуть-чуть умолкая.".Split(" ");
            var wordHandlerSettings = new WordHandlerSettings {SpeechPartsStatuses = defaultSpeechPartsStatuses};
            new WordHandler(wordHandlerSettings)
                .NormalizeAndExcludeBoringWords(sentence)
                .Should()
                .BeEquivalentTo(
                    "утро",
                    "лес",
                    "запевать",
                    "два",
                    "звонкий",
                    "соловей",
                    "защебетать",
                    "воробей",
                    "умолкать"
                );
        }

        [Test]
        public void NormilizeAndExcludeBoringWords_AllWordsBoring_ReturnsAllInputWords()
        {
            var sentence = "Утром в лесу запели два звонких соловья и защебетали воробьи, чуть-чуть умолкая."
                .Split(" ");
            var wordHandlerSettings = new WordHandlerSettings
            {
                SpeechPartsStatuses = defaultSpeechPartsStatuses
                    .ToDictionary(pair => pair.Key, pair => false)
            };
            new WordHandler(wordHandlerSettings)
                .NormalizeAndExcludeBoringWords(sentence)
                .Should()
                .BeEquivalentTo();
        }

        [Test]
        public void NormilizeAndExcludeBoringWords_BoringWordsIsNounAndConjunctions_ReturnsOnlyAdjective()
        {
            var settings = new WordHandlerSettings
            {
                SpeechPartsStatuses = new Dictionary<PartsOfSpeech, bool>
                {
                    [PartsOfSpeech.Adjective] = true, [PartsOfSpeech.Noun] = false, [PartsOfSpeech.Conjunction] = false
                }
            };
            var words = "Красивое и сочное яблоко".Split(" ");
            var interestingWords = new WordHandler(settings).NormalizeAndExcludeBoringWords(words).ToArray();
            interestingWords.Should().Contain("красивый", "сочный");
            interestingWords.Should().NotContain("и", "яблоко");
        }
    }
}