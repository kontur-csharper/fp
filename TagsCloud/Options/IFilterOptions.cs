using System.Collections.Generic;

namespace TagsCloud.Options
{
    public interface IFilterOptions
    {
        public string MystemLocation { get; set; }

        public IEnumerable<string> BoringWords { get; set; }
    }
}