using Priority_Queue;
using System;

namespace Kaliko.ImageLibrary
{
    public class Histogram
    {
        public readonly uint[] Data = new uint[DATA_ARRAY_SIZE];

        private const int DATA_ARRAY_SIZE = byte.MaxValue + 1;

        public Histogram(KalikoImage image)
        {
            Calculate(image.ByteArray);
        }

        private void Calculate(byte[] byteArray)
        {
            for (int i = 0, l = byteArray.Length; i < l; i += 4)
            {
                byte grayscale = ImageHelper.GetGrayscalePixel(byteArray[i + 2], byteArray[i + 1], byteArray[i]);
                Data[grayscale]++;
            }
        }

        private class ThresholdPoint : FastPriorityQueueNode
        {
            public int Index;
            public uint Value;

            public ThresholdPoint(int index, uint value)
            {
                Index = index;
                Value = value;
            }
        }

        public byte GetThresholdLevel()
        {
            // blend together groups of pixels to average their data and get rid of local spikes
            int weightedArraySize = DATA_ARRAY_SIZE - 4;
            uint[] weightedData = new uint[weightedArraySize];
            for (int i = 0; i < weightedArraySize; ++i)
            {
                weightedData[i] =
                    4 * Data[i + 2] +
                    3 * (Data[i + 1] + Data[i + 3]) +
                    Data[i] + Data[i + 4];
            }

            // find highest local maximum (biggest concentration of pixels of a single color
            int queueSize = (int)Math.Ceiling(weightedArraySize / 2f);
            FastPriorityQueue<ThresholdPoint> queue = new FastPriorityQueue<ThresholdPoint>(queueSize);
            for (int i = 1; i < weightedArraySize - 1; ++i)
            {
                // check if current point is higher than both of its neighbors
                if (weightedData[i] > weightedData[i + 1] && weightedData[i] > weightedData[i - 1])
                {
                    queue.Enqueue(new ThresholdPoint(i, weightedData[i]), -weightedData[i]);        // reversing queue priority to store higher values in front
                }
            }

            // find two highest value groups separated by a certain minimal distance
            ThresholdPoint a = queue.Dequeue();
            ThresholdPoint b = queue.Dequeue();
            int minPixelDistance = 16;
            while (Math.Abs(a.Index - b.Index) < minPixelDistance)
            {
                b = queue.Dequeue();
            }

            // switch points around if necessary so that 'a' represents a lower index
            if (a.Index > b.Index)
            {
                ThresholdPoint t = a;
                a = b;
                b = t;
            }

            // find the lowest value between these two groups
            uint minVal = a.Value;
            int minPos = a.Index;

            for (int i = a.Index + 1; i < b.Index; ++i)
            {
                if (weightedData[i] < minVal)
                {
                    minVal = weightedData[i];
                    minPos = i;
                }
            }

            return (byte)(minPos + 2);
        }
    }
}
