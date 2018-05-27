using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.BitFilters;
using Kaliko.ImageLibrary.FastFilters;
using Kaliko.ImageLibrary.Filters;
using System.Drawing;

namespace Biomet_Project
{
    public class ImageProcessor
    {
        public KalikoImage GetProcessedMarkers(Bitmap bitmap)
        {
            KalikoImage image = new KalikoImage(bitmap);
            ProcessImage(image);

            return image;
        }

        public KalikoImage GetProcessedImage(Bitmap bitmap, KalikoImage markers)
        {
            KalikoImage image = new KalikoImage(bitmap);
            ProcessImage(image);

            // remove markers & crop image, operating on bit matrix for faster calculations
            BitMatrix matrix = new BitMatrix(image);

            SubtractFilter subFilter = new SubtractFilter(markers);
            matrix.ApplyFilter(subFilter);

            AutoCropFilter autoCrop = new AutoCropFilter();
            matrix.ApplyFilter(autoCrop);

            // after performing bit matrix operations, get a new image
            KalikoImage bitImage = matrix.ToImage();
            return bitImage;
        }

        private void ProcessImage(KalikoImage image)
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
