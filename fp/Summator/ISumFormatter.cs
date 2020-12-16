using System;
using System.Linq;

namespace FP
{
    public interface ISumFormatter
    {
        string Format(int[] nums, int sum);
    }

    public class HexSumFormatter : ISumFormatter
    {
        public string Format(int[] nums, int sum)
        {
            return string.Format("Sum({0}) = {1}",
                string.Join(" ", nums.Select(n => Convert.ToString(n, 16))),
                Convert.ToString(sum, 16));
        }
    }
}