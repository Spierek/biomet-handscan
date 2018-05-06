using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.BitFilters;
using Kaliko.ImageLibrary.FastFilters;
using Kaliko.ImageLibrary.Filters;

namespace Biomet_Project
{
    public class ImageProcessor
    {
        public KalikoImage GetProcessedImage(KalikoImage image, bool saveResult = false)
        {
            KalikoImage processedImage = image.Clone();

            processedImage.Resize(430, 500);        // for faster operations / debugging

            // operating on image
            NormalizationFilter normalization = new NormalizationFilter();
            processedImage.ApplyFilter(normalization);

            FastGaussianBlurFilter gaussian = new FastGaussianBlurFilter(5f);
            processedImage.ApplyFilter(gaussian);

            Histogram histogram = new Histogram(processedImage);
            byte filteredThreshold = histogram.GetThresholdLevel();

            ThresholdFilter threshold = new ThresholdFilter(filteredThreshold);
            processedImage.ApplyFilter(threshold);

            // operating on bit matrix for faster calculations
            BitMatrix matrix = new BitMatrix(processedImage);

            AutoCropFilter autoCrop = new AutoCropFilter();
            matrix.ApplyFilter(autoCrop);

            // after performing bit matrix operations, get a new image
            KalikoImage resultingImage = matrix.ToImage();

            if (saveResult)
            {
                resultingImage.SaveJpg("hand_blurred.jpg", 90);
            }

            return resultingImage;
        }

        public KalikoImage DEBUG_LoadImage()
        {
            KalikoImage image = new KalikoImage(@"C:\Projects\Biomet-Handscan\hand_color.jpg");
            return image;
        }
    }
}
