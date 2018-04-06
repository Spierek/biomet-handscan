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
            KalikoImage processedImage = image.Clone();

            NormalizationFilter normalization = new NormalizationFilter();
            processedImage.ApplyFilter(normalization);

            FastGaussianBlurFilter gaussian = new FastGaussianBlurFilter(5f);
            processedImage.ApplyFilter(gaussian);

            Histogram histogram = new Histogram(processedImage);
            byte filteredThreshold = histogram.GetThresholdLevel();

            ThresholdFilter threshold = new ThresholdFilter(filteredThreshold);
            processedImage.ApplyFilter(threshold);

            AutoCropFilter autoCrop = new AutoCropFilter();
            processedImage.ApplyFilter(autoCrop);

            if (saveResult)
            {
                processedImage.SaveJpg("hand_blurred.jpg", 90);
            }

            return processedImage;
        }
    }
}
