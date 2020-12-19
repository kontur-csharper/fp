using System.Collections.Generic;
using System.Linq;
using System;
using ResultOf;

namespace TagCloud.CloudLayoters
{
    public static class CloudLayoterAssosiation
    {
        public const string density = "density";
        public const string identity = "identity";
        private static readonly Dictionary<string, ICloudLayoter> cloudLayoters =
            new Dictionary<string, ICloudLayoter>
            {
                [density] = new DensityCloudLayouter(),
                [identity] = new IdentityCloudLayoter()
            };
        public static readonly HashSet<string> layoters = cloudLayoters.Keys.ToHashSet();

        public static Result<ICloudLayoter> GetCloudLayoter(string nameLayoter)
        {
            if (!cloudLayoters.ContainsKey(nameLayoter))
            {
                return new Result<ICloudLayoter>($"doesn't have processor with name {nameLayoter}\n" +
                    $"List of layoter names:\n{string.Join('\n', layoters)}");
            }
            return new Result<ICloudLayoter>(null, cloudLayoters[nameLayoter]);
        } 
    }
}
