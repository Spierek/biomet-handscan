namespace Kaliko.ImageLibrary
{
    public static class FilterHelper
    {
        public static byte GetGrayscalePixel(byte[] channels)
        {
            return GetGrayscalePixel(channels[0], channels[1], channels[2]);
        }

        public static byte GetGrayscalePixel(byte r, byte g, byte b)
        {
            float value = (float)r + (float)g + (float)b;
            value /= 3f;
            return (byte)value;
        }
    }
}
