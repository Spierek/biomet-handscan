﻿using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.BitFilters;
using Kaliko.ImageLibrary.FastFilters;
using Kaliko.ImageLibrary.Filters;
using System.Drawing;

namespace Biomet_Project
{
    public class ImageProcessor
    {
        private const int XSIZE = 420;
        private const int YSIZE = 594;      // A4

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

            //AutoCropFilter autoCrop = new AutoCropFilter();
            //matrix.ApplyFilter(autoCrop);

            // after performing bit matrix operations, get a new image
            KalikoImage bitImage = matrix.ToImage();
            return bitImage;
        }

        private void ProcessImage(KalikoImage image)
        {
            image.Resize(XSIZE, YSIZE);        // for faster operations / debugging

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
    }
}
