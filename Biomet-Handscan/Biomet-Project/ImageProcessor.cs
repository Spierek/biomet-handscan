using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.BitFilters;
using Kaliko.ImageLibrary.FastFilters;
using Kaliko.ImageLibrary.Filters;
using System.Drawing;

namespace Biomet_Project
{
    public class ImageProcessor
    {
        public KalikoImage GetProcessedImage(Bitmap bitmap, bool saveResult = false)
        {
            KalikoImage image = new KalikoImage(bitmap);
            return GetProcessedImage(image, saveResult);
        }

        public KalikoImage GetProcessedImage(KalikoImage image, bool saveResult = false)
        {
            image.Resize(430, 500);        // for faster operations / debugging

            // operating on image
            NormalizationFilter normalization = new NormalizationFilter();
            image.ApplyFilter(normalization);

            FastGaussianBlurFilter gaussian = new FastGaussianBlurFilter(5f);
            image.ApplyFilter(gaussian);

            Histogram histogram = new Histogram(image);
            byte filteredThreshold = histogram.GetThresholdLevel();

            ThresholdFilter threshold = new ThresholdFilter(filteredThreshold);
            image.ApplyFilter(threshold);

            // operating on bit matrix for faster calculations
            BitMatrix matrix = new BitMatrix(image);

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

        public KalikoImage DEBUG_LoadMarkerScan()
        {
            KalikoImage image = new KalikoImage(@"C:\Projects\Biomet-Handscan\markers_color.jpg");
            return image;
        }

        public KalikoImage DEBUG_LoadImageScan()
        {
            KalikoImage image = new KalikoImage(@"C:\Projects\Biomet-Handscan\hand_color.jpg");
            return image;
        }
    }
}
