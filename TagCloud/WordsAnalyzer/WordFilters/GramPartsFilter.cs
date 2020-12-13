using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using YandexMystem.Wrapper;
using YandexMystem.Wrapper.Enums;

namespace TagCloud.WordsAnalyzer.WordFilters
{
    public class GramPartsFilter : IWordFilter
    {
        private Regex cyrillicRegex;
        private Mysteam mystem;

        private HashSet<GramPartsEnum> allowedGramParts;
        
        public GramPartsFilter(params GramPartsEnum[] allowedGramParts)
        {
            this.allowedGramParts = allowedGramParts.ToHashSet();
            cyrillicRegex = new Regex("\\p{IsCyrillic}");
            try
            {
                mystem = new Mysteam();
            }
            catch
            {
                mystem = null;
            }
        }
        
        public Result<bool> ShouldExclude(string word)
        {
            if (mystem == null)
                return Result.Fail<bool>("Failed to load external library \"Mystem\"");
            
            if (!IsCyrillicWord(word))
                return Result.Ok(false);
            
            var models = mystem.GetWords(word);
            if (models.Count != 1)
                return Result.Ok(false);

            var model = models[0];
            return Result.Ok(!allowedGramParts.Contains(model.Lexems[0].GramPart));
        }

        private bool IsCyrillicWord(string word)
        {
            return cyrillicRegex.IsMatch(word);
        }
    }
}