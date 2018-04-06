using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.FastFilters;
using Kaliko.ImageLibrary.Filters;

namespace Biomet_Project
{
    public class ImageProcessor
    {
        public KalikoImage LoadImage()
        {
            KalikoImage image = new KalikoImage(@"C:\Projects\Biomet-Handscan\hand_color.jpg");
            return image;
        }

        public KalikoImage GetProcessedImage(KalikoImage image, bool saveResult = false)
        {
            NormalizationFilter normalization = new NormalizationFilter();
            image.ApplyFilter(normalization);

            FastGaussianBlurFilter gaussian = new FastGaussianBlurFilter(3f);
            image.ApplyFilter(gaussian);

            Histogram histogram = new Histogram(image);
            byte filteredThreshold = histogram.GetThresholdLevel();

            ThresholdFilter threshold = new ThresholdFilter(filteredThreshold);
            image.ApplyFilter(threshold);

            if (saveResult)
            {
                image.SaveJpg("hand_blurred.jpg", 90);
            }

            return image;
        }
    }
}
