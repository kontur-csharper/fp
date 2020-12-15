using System.Drawing;
using ResultOf;
using TagsCloudContainer.Infrastructure.CloudGenerator;
using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.App.CloudGenerator
{
    internal class CloudLayouterFactory : ICloudLayouterFactory
    {
        private readonly ILayouterAlgorithmSettingsHolder layouterSettings;
        private readonly IImageSizeSettingsHolder sizeSettings;

        public CloudLayouterFactory(ILayouterAlgorithmSettingsHolder layouterSettings,
            IImageSizeSettingsHolder sizeSettings)
        {
            this.layouterSettings = layouterSettings;
            this.sizeSettings = sizeSettings;
        }

        public Result<ICloudLayouter> CreateCloudLayouter()
        {
            if (layouterSettings.LayouterAlgorithm == CloudLayouterAlgorithm.CircularCloudLayouter)
                return new CircularCloudLayouter(new Point(sizeSettings.Width / 2,
                    sizeSettings.Height / 2));

            return Result.Fail<ICloudLayouter>("No found this layouter algorithm");
        }
    }
}