using System;
using System.Collections.Generic;
using System.Drawing;

namespace Kaliko.ImageLibrary.BitFilters
{
    // via http://www.pvladov.com/2012/05/bit-matrix-in-c-sharp.html
    public class BitMatrix
    {
        private byte[] m_Data;

        private int m_Width;
        public int Width { get { return m_Width; } }

        private int m_Height;
        public int Height { get { return m_Height; } }

        public BitMatrix(int width, int height)
        {
            m_Width = width;
            m_Height = height;

            // Allocate the needed number of bytes
            int byteCount = GetByteCount();
            m_Data = new byte[byteCount];
        }

        public BitMatrix(KalikoImage image) : this(image.Width, image.Height)
        {
            byte[] byteArray = image.ByteArray;
            for (int y = 0; y < image.Height; ++y)
            {
                for (int x = 0; x < image.Width; ++x)
                {
                    int offset = (x + y * m_Width) * 4;
                    this[x, y] = (byteArray[offset] > 128);
                }
            }
        }

        public BitMatrix(BitMatrix bm) : this(bm.Width, bm.Height)
        {
            for (int i = 0; i < bm.m_Data.Length; ++i)
            {
                m_Data[i] = bm.m_Data[i];
            }
        }

        public KalikoImage ToImage()
        {
            KalikoImage image = new KalikoImage(Width, Height);
            byte[] byteArray = image.ByteArray;

            for (int y = 0; y < m_Height; ++y)
            {
                for (int x = 0; x < m_Width; ++x)
                {
                    int offset = (x + y * m_Width) * 4;
                    byte value = this[x, y] ? byte.MaxValue : byte.MinValue;

                    byteArray[offset] = value;
                    byteArray[offset + 1] = value;
                    byteArray[offset + 2] = value;
                    byteArray[offset + 3] = byte.MaxValue;
                }
            }

            image.ByteArray = byteArray;

            return image;
        }
        /// <summary>
        /// Gets/Sets the value at the specified row and column index.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= m_Width)
                    throw new ArgumentOutOfRangeException("x");

                if (y < 0 || y >= m_Height)
                    throw new ArgumentOutOfRangeException("y");

                int pos = x + y * m_Width;
                int index = pos % 8;
                pos >>= 3;
                return (m_Data[pos] & (1 << index)) != 0;
            }
            set
            {
                if (x < 0 || x >= m_Width)
                    throw new ArgumentOutOfRangeException("x");

                if (y < 0 || y >= m_Height)
                    throw new ArgumentOutOfRangeException("y");

                int pos = x + y * m_Width;
                int index = pos % 8;
                pos >>= 3;
                m_Data[pos] &= (byte)(~(1 << index));

                if (value)
                {
                    m_Data[pos] |= (byte)(1 << index);
                }
            }
        }

        public void Crop(int x, int y, int width, int height)
        {
            // data validation
            if (width < 1)
            {
                throw new ArgumentOutOfRangeException("width");
            }
            else if (height < 1)
            {
                throw new ArgumentOutOfRangeException("height");
            }

            byte[] oldData = m_Data;
            int oldWidth = m_Width;
            int oldHeight = m_Height;

            m_Width = width;
            m_Height = height;

            int byteCount = GetByteCount();
            m_Data = new byte[byteCount];

            // fill in the new data
            for (int j = 0; j < height; ++j)
            {
                for (int i = 0; i < width; ++i)
                {
                    int oldX = x + i;
                    int oldY = y + j;

                    // check if this byte index is contained within the old data matrix
                    if (oldX >= 0 && oldX < oldWidth &&
                        oldY >= 0 && oldY < oldHeight)
                    {
                        int pos = oldX + oldY * oldWidth;
                        int index = pos % 8;
                        pos >>= 3;

                        this[i, j] = ((oldData[pos] & (1 << index)) != 0);
                    }
                }
            }
        }

        public void SetPoint(Point point, bool set)
        {
            if (point != null && IsPointCorrect(point.X, point.Y))
            {
                this[point.X, point.Y] = set;
            }
        }

        public void SetPoints(List<Point> points, bool set)
        {
            if (points != null)
            {
                for (int i = 0; i < points.Count; ++i)
                {
                    Point p = points[i];
                    if (IsPointCorrect(p.X, p.Y))
                    {
                        this[p.X, p.Y] = set;
                    }
                }
            }
        }

        private int GetByteCount()
        {
            int bitCount = m_Width * m_Height;
            int byteCount = bitCount >> 3;
            if (bitCount % 8 != 0)
            {
                byteCount++;
            }

            return byteCount;
        }

        public void ApplyFilter(IBitFilter filter)
        {
            filter.Run(this);
        }

        public bool IsPointCorrect(int x, int y)
        {
            return (x >= 0 && x < Width && y >= 0 && y < Height);
        }
    }
}
