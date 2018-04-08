using System;

namespace Kaliko.ImageLibrary.BitFilters
{
    // via http://www.pvladov.com/2012/05/bit-matrix-in-c-sharp.html
    public class BitMatrix
    {
        private KalikoImage m_Image;
        private byte[] m_Data;

        private int m_RowCount;
        public int RowCount { get { return m_RowCount; } }

        private int m_ColumnCount;
        public int ColumnCount { get { return m_ColumnCount; } }

        public BitMatrix(int rowCount, int columnCount)
        {
            m_Image = new KalikoImage(rowCount, columnCount);

            m_RowCount = rowCount;
            m_ColumnCount = columnCount;

            // Allocate the needed number of bytes
            int byteCount = GetByteCount();
            m_Data = new byte[byteCount];
        }

        public BitMatrix(KalikoImage image) : this(image.Width, image.Height)
        {
            m_Image = image.Clone();    // this is not exactly nice but for unknown reasons creating a new image does not work properly

            byte[] byteArray = m_Image.ByteArray;
            for (int y = 0; y < image.Height; ++y)
            {
                for (int x = 0; x < image.Width; ++x)
                {
                    int offset = (x + y * m_RowCount) * 4;
                    this[x, y] = (byteArray[offset] > 128);
                }
            }
        }

        public KalikoImage ToImage()
        {
            byte[] byteArray = m_Image.ByteArray;

            for (int y = 0; y < m_ColumnCount; ++y)
            {
                for (int x = 0; x < m_RowCount; ++x)
                {
                    int offset = (x + y * m_RowCount) * 4;
                    byte value = this[x, y] ? byte.MaxValue : byte.MinValue;

                    byteArray[offset] = value;
                    byteArray[offset + 1] = value;
                    byteArray[offset + 2] = value;
                    byteArray[offset + 3] = byte.MaxValue;
                }
            }

            return m_Image;
        }
        /// <summary>
        /// Gets/Sets the value at the specified row and column index.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public bool this[int rowIndex, int columnIndex]
        {
            get
            {
                if (rowIndex < 0 || rowIndex >= m_RowCount)
                    throw new ArgumentOutOfRangeException("rowIndex");

                if (columnIndex < 0 || columnIndex >= m_ColumnCount)
                    throw new ArgumentOutOfRangeException("columnIndex");

                int pos = rowIndex * m_ColumnCount + columnIndex;
                int index = pos % 8;
                pos >>= 3;
                return (m_Data[pos] & (1 << index)) != 0;
            }
            set
            {
                if (rowIndex < 0 || rowIndex >= m_RowCount)
                    throw new ArgumentOutOfRangeException("rowIndex");

                if (columnIndex < 0 || columnIndex >= m_ColumnCount)
                    throw new ArgumentOutOfRangeException("columnIndex");

                int pos = rowIndex * m_ColumnCount + columnIndex;
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
            int oldWidth = m_RowCount;
            int oldHeight = m_ColumnCount;

            m_RowCount = width;
            m_ColumnCount = height;

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
                        int pos = oldX * m_ColumnCount + oldY;
                        int index = pos % 8;
                        pos >>= 3;

                        this[i, j] = ((oldData[pos] & (1 << index)) != 0);
                    }
                }
            }

            m_Image.Crop(x, y, width, height);
        }

        private int GetByteCount()
        {
            int bitCount = m_RowCount * m_ColumnCount;
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
    }
}
