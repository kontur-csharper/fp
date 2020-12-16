using System;
using System.Collections.Generic;
using System.Drawing;
using HomeExercise.Helpers;
using ResultOf;

namespace HomeExercise
{
    public class WordCloud : IWordCloud
    {
        public List<ISizedWord> SizedWords { get; }
        public Point Center { get;}
        public Size Size { get; private set; }
        private ICircularCloudLayouter layouter;
        private readonly IWordsProcessor wordsProcessor;
        private List<Rectangle> rectangleInCloud = new List<Rectangle>();

        public WordCloud(ICircularCloudLayouter layouter,  IWordsProcessor wordsProcessor)
        {
            Center = layouter.Center;
            SizedWords = new List<ISizedWord>();
            this.layouter = layouter;
            this.wordsProcessor = wordsProcessor;
        }

       public void BuildCloud()
       {
           var resultWords = wordsProcessor.HandleWords();
           if (!resultWords.IsSuccess)
           {
               Console.WriteLine(resultWords.Error);
               return;
           }
           
           foreach (var word in resultWords.Value)
           {
               var resultRectangle = GetRectangle(word, word.Size).OnFail(Console.WriteLine);
               if (!resultRectangle.IsSuccess)
                   return;
               SizedWords.Add(new SizedWord(word, word.Size, word.Font, resultRectangle.Value));
           }

           var sizeResult = Result.Of(GetCloudSize).OnFail(Console.WriteLine);
           Size = sizeResult.IsSuccess ? sizeResult.Value : default;
       }

        private Result<Rectangle> GetRectangle(IWord word, int size)
        {
            var rectangle= layouter
                .PutNextRectangle(GraphicsHelper.MeasureString(word.Text, new Font(word.Font, size)));

            if (!rectangle.IsSuccess)
                return rectangle;
            
            rectangleInCloud.Add(rectangle.Value);

            return rectangle;
        }

        private Size GetCloudSize()
        {
            var top = 0;
            var bottom = 0;
            var left = 0;
            var right = 0;
            foreach (var rectangle in rectangleInCloud)
            {
                top = rectangle.Top < top ? rectangle.Top : top;
                bottom = rectangle.Bottom > bottom ? rectangle.Bottom : bottom;
                left = rectangle.Left < left ? rectangle.Left : left;
                right = rectangle.Right > right ? rectangle.Right : right;
            }
            
            return new Size(GetDistance(bottom, top), GetDistance(left, right));
        }

        private int GetDistance(int first, int second)
        {
            var res = first > second ? first - second : second - first;
            return res;
        }
    }
}