namespace Kaliko.ImageLibrary.Filters
{
    public class ThresholdFilter : IFilter
    {
        private byte m_Threshold = THRESHOLD_DEFAULT;

        private const byte THRESHOLD_DEFAULT = 128;

        public ThresholdFilter(byte threshold = THRESHOLD_DEFAULT)
        {
            m_Threshold = threshold;
        }

        public virtual void Run(KalikoImage image)
        {
            ApplyThreshold(image);
        }

        private void ApplyThreshold(KalikoImage image)
        {
            byte[] byteArray = image.ByteArray;

            for (int i = 0, l = byteArray.Length; i < l; i += 4)
            {
                byte grayscale = FilterHelper.GetGrayscalePixel(byteArray[i + 2], byteArray[i + 1], byteArray[i]);
                byte value = GetThresholdValue(grayscale);
                byteArray[i] = value;       // b
                byteArray[i + 1] = value;   // g
                byteArray[i + 2] = value;   // r
            }

            image.ByteArray = byteArray;
        }

        // return 0 if below threshold or 1 if equals/above threshold
        private byte GetThresholdValue(byte input)
        {
            if (input < m_Threshold)
            {
                return byte.MinValue;
            }
            else
            {
                return byte.MaxValue;
            }
        }
    }
}
