using System.Collections.Generic;
using System.Linq;
using System;
using ResultOf;

namespace TagCloud.WordsMetrics
{
    public static class WordsMetricAssosiation
    {
        public const string count = "count";
        private static readonly Dictionary<string, IWordsMetric> metrics =
            new Dictionary<string, IWordsMetric>
            {
                [count] = new CountWordMetric()
            };

        public static readonly HashSet<string> metricsName = metrics.Keys.ToHashSet();

        public static Result<IWordsMetric> GetMetric(string name)
        {
            if (!metrics.ContainsKey(name))
            {
                return new Result<IWordsMetric>($"doesn't have metric with name {name}\n" +
                    $"List of metric names:\n{string.Join('\n', metricsName)}");
            }
            IWordsMetric metric;
            try
            {
                metric = metrics[name];
            }
            catch (Exception e)
            {
                return new Result<IWordsMetric>($"something was wrong: {e.Message}");
            }
            return new Result<IWordsMetric>(null, metric);
        }
    }
}
