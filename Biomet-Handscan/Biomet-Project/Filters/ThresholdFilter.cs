namespace Kaliko.ImageLibrary.Filters
{
    public class ThresholdFilter : IFilter
    {
        private byte m_Threshold = THRESHOLD_DEFAULT;

        private const byte THRESHOLD_MIN = 0;
        private const byte THRESHOLD_MAX = 255;
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
                byteArray[i] = GetThresholdValue(byteArray[i]);             // b
                byteArray[i + 1] = GetThresholdValue(byteArray[i + 1]);     // g
                byteArray[i + 2] = GetThresholdValue(byteArray[i + 2]);     // r
            }

            image.ByteArray = byteArray;
        }

        private byte GetThresholdValue(byte input)
        {
            if (input < m_Threshold)
            {
                return THRESHOLD_MIN;
            }
            else
            {
                return THRESHOLD_MAX;
            }
        }
    }
}
