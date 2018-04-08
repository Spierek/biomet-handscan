using Kaliko.ImageLibrary.BitFilters;
using System.Collections.Generic;
using System.Drawing;

namespace Biomet_Project
{
    public class PathDetector
    {
        public List<Point> FindLongestPath(BitMatrix matrix)
        {
            List<Point> path = new List<Point>();
            //for (int i = 0; i < )

            return path;
        }

        private bool IsEdgePoint(BitMatrix matrix, Point point)
        {
            return IsEdgePoint(matrix, point.X, point.Y);
        }

        // for every white point, check if any neighboring pixels are black
        private bool IsEdgePoint(BitMatrix matrix, int x, int y)
        {
            if (matrix[x, y])
            {
                if (x != 0 && !matrix[x - 1, y]) return true;
                if (x != matrix.RowCount - 1 && !matrix[x + 1, y]) return true;
                if (y != 0 && !matrix[x, y - 1]) return true;
                if (y != matrix.ColumnCount - 1 && !matrix[x, y + 1]) return true;
            }

            return false;
        }
    }
}
