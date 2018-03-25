using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.FastFilters;
using Kaliko.ImageLibrary.Filters;

namespace Biomet_Project
{
    public class ImageProcessor
    {
        public void LoadImage()
        {
            KalikoImage image = new KalikoImage(@"C:\Projects\Biomet-Handscan\hand_mono.jpg");

            NormalizationFilter normalization = new NormalizationFilter();
            image.ApplyFilter(normalization);

            FastGaussianBlurFilter gaussian = new FastGaussianBlurFilter(3f);
            image.ApplyFilter(gaussian);

            image.SaveJpg("hand_blurred.jpg", 90);
        }
    }
}
