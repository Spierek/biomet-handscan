namespace Kaliko.ImageLibrary
{
    public static class ImageHelper
    {
        public static byte GetGrayscalePixel(byte[] channels)
        {
            return GetGrayscalePixel(channels[0], channels[1], channels[2]);
        }

        public static byte GetGrayscalePixel(byte r, byte g, byte b)
        {
            // using RGB -> YUV sampling for improved results
            float value = r * 0.299f + g * 0.587f + b * 0.114f;
            return (byte)value;
        }
    }
}
