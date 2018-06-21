using LSTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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

        // √((x2−x1)^2+(y2−y1)^2)
        public static double Distance(Point a, Point b)
        {
            double x = Math.Pow((b.X - a.X), 2);
            double y = Math.Pow((b.Y - a.Y), 2);
            return Math.Sqrt(x + y);
        }

        // iteratively fills an area around the specified point until reaching any edges
        public int FillArea(Point p)
        {
            List<Point> thisLayer = new List<Point>() { p };
            List<Point> nextLayer = new List<Point>();

            int ret = this[p.X, p.Y] ? 0 : 1;
            this[p.X, p.Y] = true;

            while (thisLayer.Count > 0)
            {
                nextLayer.Clear();
                for (int i = 0; i < thisLayer.Count; ++i)
                {
                    if (thisLayer[i].X > 0 && !this[thisLayer[i].X - 1, thisLayer[i].Y])
                    {
                        this[thisLayer[i].X - 1, thisLayer[i].Y] = true;
                        ++ret;
                        nextLayer.Add(new Point(thisLayer[i].X - 1, thisLayer[i].Y));
                    }
                    if (thisLayer[i].Y < Height - 1 && !this[thisLayer[i].X, thisLayer[i].Y + 1])
                    {
                        this[thisLayer[i].X, thisLayer[i].Y + 1] = true;
                        ++ret;
                        nextLayer.Add(new Point(thisLayer[i].X, thisLayer[i].Y + 1));
                    }
                    if (thisLayer[i].X < Width - 1 && !this[thisLayer[i].X + 1, thisLayer[i].Y])
                    {
                        this[thisLayer[i].X + 1, thisLayer[i].Y] = true;
                        ++ret;
                        nextLayer.Add(new Point(thisLayer[i].X + 1, thisLayer[i].Y));
                    }
                    if (thisLayer[i].Y > 0 && !this[thisLayer[i].X, thisLayer[i].Y - 1])
                    {
                        this[thisLayer[i].X, thisLayer[i].Y - 1] = true;
                        ++ret;
                        nextLayer.Add(new Point(thisLayer[i].X, thisLayer[i].Y - 1));
                    }
                }

                thisLayer = new List<Point>();
                thisLayer.InsertRange(0, nextLayer);
            }

            return ret;
        }

        public int GetSurface(BitMatrix fbm, Point a, Point b, out BitMatrix cbm)
        {
            Point[,] previous = new Point[fbm.Width, fbm.Height];
            BitMatrix cfbm = new BitMatrix(fbm);
            List<Point> thisLayer = new List<Point>() { a };
            List<Point> nextLayer = new List<Point>();
            List<Point> path = new List<Point>();

            while (thisLayer.Count > 0)
            {
                nextLayer.Clear();

                for (int i = 0; i < thisLayer.Count; ++i)
                {
                    int x = thisLayer[i].X;
                    int y = thisLayer[i].Y;

                    List<APair<Point, double>> considerable = new List<APair<Point, double>>();
                    if (x > 0 && IsPointCorrect(x, y - 1) && cfbm[x, y - 1])
                    {
                        Point t = new Point(x, y - 1);
                        cfbm[t.X, t.Y] = false;
                        previous[t.X, t.Y] = thisLayer[i];
                        considerable.Add(new APair<Point, double>(t, Distance(t, b)));
                    }
                    if (y < Height - 1 && IsPointCorrect(x + 1, y) && cfbm[x + 1, y])
                    {
                        Point t = new Point(x + 1, y);
                        cfbm[t.X, t.Y] = false;
                        previous[t.X, t.Y] = thisLayer[i];
                        considerable.Add(new APair<Point, double>(t, Distance(t, b)));
                    }
                    if (x < Width - 1 && IsPointCorrect(x, y + 1) && cfbm[x, y + 1])
                    {
                        Point t = new Point(x, y + 1);
                        cfbm[t.X, t.Y] = false;
                        previous[t.X, t.Y] = thisLayer[i];
                        considerable.Add(new APair<Point, double>(t, Distance(t, b)));
                    }
                    if (y > 0 && IsPointCorrect(x - 1, y) && cfbm[x - 1, y])
                    {
                        Point t = new Point(x - 1, y);
                        cfbm[t.X, t.Y] = false;
                        previous[t.X, t.Y] = thisLayer[i];
                        considerable.Add(new APair<Point, double>(t, Distance(t, b)));
                    }

                    considerable = considerable.OrderBy(o => o.Second).ToList();

                    for (int j = 0; j < considerable.Count; ++j)
                    {
                        nextLayer.Add(considerable[j].First);
                    }
                }

                thisLayer = new List<Point>();
                thisLayer.InsertRange(0, nextLayer);
            }

            cbm = new BitMatrix(this);

            for (Point t = b; t.X != a.X || t.Y != a.Y; )
            {
                path.Add(t);
                cbm[t.X, t.Y] = true;
                t = previous[t.X, t.Y];
            }
            cbm.FillArea(new Point(0, 0));

            List<int> blackSurfaces = new List<int>();
            for (int r = 0; r < cbm.Width; ++r)
            {
                for (int c = 0; c < cbm.Height; ++c)
                {
                    if (!cbm[r, c])
                    {
                        blackSurfaces.Add(cbm.FillArea(new Point(c, r)));
                    }
                }
            }

            blackSurfaces.Sort();

            //return blackSurfaces[blackSurfaces.Count - 2];
            return 0;
        }
    }
}
