using TagsCloud.ResultOf;

namespace TagsCloud
{
    public interface ITagCloud
    {
        public Result<None> MakeTagCloud();

        public Result<None> SaveTagCloud();
    }
}