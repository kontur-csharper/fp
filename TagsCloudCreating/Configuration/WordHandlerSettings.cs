﻿using System.Collections.Generic;
 using TagsCloudCreating.Core.WordProcessors;

 namespace TagsCloudCreating.Configuration
{
    public class WordHandlerSettings
    {
        public Dictionary<PartsOfSpeech, bool> SpeechPartsStatuses { get; set; } = new Dictionary<PartsOfSpeech, bool>
        {
            [PartsOfSpeech.Adjective] = true,
            [PartsOfSpeech.Adverb] = false,
            [PartsOfSpeech.PronounAdverb] = false,
            [PartsOfSpeech.NumeralAdjective] = true,
            [PartsOfSpeech.PronounAdjective] = false,
            [PartsOfSpeech.PartCompoundWord] = true,
            [PartsOfSpeech.Conjunction] = false,
            [PartsOfSpeech.Interjection] = true,
            [PartsOfSpeech.Numeral] = true,
            [PartsOfSpeech.Particle] = false,
            [PartsOfSpeech.Preposition] = false,
            [PartsOfSpeech.Noun] = true,
            [PartsOfSpeech.PronounNoun] = false,
            [PartsOfSpeech.Verb] = true
        };
    }
}