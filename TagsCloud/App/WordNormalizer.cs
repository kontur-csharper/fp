﻿namespace TagsCloud.App
{
    public class WordNormalizer : IWordNormalizer
    {
        public string Normalize(string word)
        {
            return word.ToLower();
        }
    }
}