using System;

namespace Kaliko.ImageLibrary.Filters
{
    // Find lowest pixel value, subtract from all pixels, then multiply by difference between highest and 255
    public class NormalizationFilter : IFilter
    {
        public virtual void Run(KalikoImage image)
        {
            Normalize(image);
        }

        private void Normalize(KalikoImage image)
        {
            byte[] byteArray = image.ByteArray;

            // subtract lowest values from all pixels
            byte[] lowest = FindLowestValues(image);
            for (int i = 0, l = byteArray.Length; i < l; i += 4)
            {
                byteArray[i] -= lowest[0];      // b
                byteArray[i + 1] -= lowest[1];  // g
                byteArray[i + 2] -= lowest[2];  // r
            }

            // multiply all pixels by (255 / highest value for that channel)
            byte[] highest = FindHighestValues(image);
            float[] multipliers = GetMultipliers(highest);
            for (int i = 0, l = byteArray.Length; i < l; i += 4)
            {
                byteArray[i] = (byte)Math.Round(byteArray[i] * multipliers[0]);             // b
                byteArray[i + 1] = (byte)Math.Round(byteArray[i + 1] * multipliers[1]);     // g
                byteArray[i + 2] = (byte)Math.Round(byteArray[i + 2] * multipliers[2]);     // r
            }

            image.ByteArray = byteArray;
        }

        private byte[] FindLowestValues(KalikoImage image)
        {
            byte[] byteArray = image.ByteArray;
            byte[] lowest = new byte[4] { byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue};
            for (int i = 0, l = byteArray.Length; i < l; i += 4)
            {
                if (byteArray[i] < lowest[0])
                {
                    lowest[0] = byteArray[i];
                }
                if (byteArray[i + 1] < lowest[1])
                {
                    lowest[1] = byteArray[i + 1];
                }
                if (byteArray[i + 2] < lowest[2])
                {
                    lowest[2] = byteArray[i + 2];
                }
            }

            return lowest;
        }

        private byte[] FindHighestValues(KalikoImage image)
        {
            byte[] byteArray = image.ByteArray;
            byte[] highest = new byte[4] { byte.MinValue, byte.MinValue, byte.MinValue, byte.MinValue};
            for (int i = 0, l = byteArray.Length; i < l; i += 4)
            {
                if (byteArray[i] > highest[0])
                {
                    highest[0] = byteArray[i];
                }
                if (byteArray[i + 1] > highest[1])
                {
                    highest[1] = byteArray[i + 1];
                }
                if (byteArray[i + 2] > highest[2])
                {
                    highest[2] = byteArray[i + 2];
                }
            }

            return highest;
        }

        private float[] GetMultipliers(byte[] values)
        {
            float[] multipliers = new float[4]
            {
                byte.MaxValue / (float)values[0],
                byte.MaxValue / (float)values[1],
                byte.MaxValue / (float)values[2],
                byte.MaxValue / (float)values[3]
            };

            return multipliers;
        }
    }
}