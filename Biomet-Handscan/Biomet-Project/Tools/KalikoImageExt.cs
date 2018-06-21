using System.Collections.Generic;
using System.Drawing;

namespace Kaliko.ImageLibrary
{
    public static class KalikoImageExt
    {
        public static bool IsPointCorrect(this KalikoImage ki, int x, int y)
        {
            return (x >= 0 && y >= 0 && x < ki.Width - 1 && y < ki.Height - 1);
        }

        public static void SetPixel(this KalikoImage ki, int x, int y, Color col)
        {
            SetPixel(ki, x, y, col.R, col.G, col.B, col.A);
        }

        public static void SetPixel(this KalikoImage ki, int x, int y, byte r, byte g, byte b, byte a)
        {
            byte[] byteArray = ki.ByteArray;

            int offset = (x + y * ki.Width) * 4;

            byteArray[offset] = b;
            byteArray[offset + 1] = g;
            byteArray[offset + 2] = r;
            byteArray[offset + 3] = a;

            ki.ByteArray = byteArray;       // assignment creates proper bitmaps & handles other operations
        }

        public static void SetPixels(this KalikoImage ki, List<Point> points, Color col)
        {
            byte[] byteArray = ki.ByteArray;

            for (int i = 0; i < points.Count; ++i)
            {
                Point p = points[i];
                if (p != null)
                {
                    int offset = (p.X + p.Y * ki.Width) * 4;

                    byteArray[offset] = col.B;
                    byteArray[offset + 1] = col.G;
                    byteArray[offset + 2] = col.R;
                    byteArray[offset + 3] = col.A;
                }
            }

            ki.ByteArray = byteArray;
        }

        public static void DrawMarker(this KalikoImage ki, Point p, Color col, int radius = 1)
        {
            List<Point> points = new List<Point>();
            for (int j = -radius; j < radius; ++j)
            {
                for (int i = -radius; i < radius; ++i)
                {
                    int x = p.X + i;
                    int y = p.Y + j;
                    if (ki.IsPointCorrect(x, y))
                    {
                        points.Add(new Point(x, y));
                    }
                }
            }

            SetPixels(ki, points, col);
        }
    }
}
