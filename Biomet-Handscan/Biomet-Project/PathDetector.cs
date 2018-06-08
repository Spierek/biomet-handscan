using Kaliko.ImageLibrary.BitFilters;
using System.Collections.Generic;
using System.Drawing;

namespace Biomet_Project
{
    public class PathDetector
    {
        // multiple path ver., breaks during path merging
        public List<Point> FindLongestPath(BitMatrix matrix)
        {
            List<List<Point>> paths = new List<List<Point>>();
            List<bool> pathMerged = new List<bool>();
            SortedSet<int> activepaths = new SortedSet<int>();

            int lastCol = matrix.Height - 1;
            int[,] pathInPoint = new int[matrix.Width, matrix.Height];

            // prepare path matrix
            for (uint r = 0; r < matrix.Width; ++r)
            {
                for (uint c = 0; c < matrix.Height; ++c)
                {
                    pathInPoint[r, c] = int.MaxValue;
                }
            }

            // find edge points in final row
            for (int r = 0; r < matrix.Width - 1; ++r)
            {
                if (IsEdgePoint(matrix, r, lastCol))
                {
                    List<Point> newPath = new List<Point>()
                    {
                        new Point(r, lastCol),
                    };

                    activepaths.Add(paths.Count);
                    pathMerged.Add(false);
                    pathInPoint[r, lastCol] = paths.Count;
                    paths.Add(newPath);
                }
            }

            // iterate over active paths
            List<int> toRemove = new List<int>();
            while (activepaths.Count > 0)
            {
                foreach (int it in activepaths)
                {
                    List<Point> points = paths[it];
                    int x = points[points.Count - 1].X;
                    int y = points[points.Count - 1].Y;
                    int nx = x;
                    int ny = y;

                    // find an 8-neighbor edge point that's not a part of this path
                    if (IsEdgePoint(matrix, x - 1, y - 1) && pathInPoint[x - 1, y - 1] != it) { --nx; --ny; }
                    else if (IsEdgePoint(matrix, x - 1, y) && pathInPoint[x - 1, y] != it) { --nx; }
                    else if (IsEdgePoint(matrix, x - 1, y + 1) && pathInPoint[x - 1, y + 1] != it) { --nx; ++ny; }
                    else if (IsEdgePoint(matrix, x, y + 1) && pathInPoint[x, y + 1] != it) { ++ny; }
                    else if (IsEdgePoint(matrix, x + 1, y + 1) && pathInPoint[x + 1, y + 1] != it) { ++nx; ++ny; }
                    else if (IsEdgePoint(matrix, x + 1, y) && pathInPoint[x + 1, y] != it) { ++nx; }
                    else if (IsEdgePoint(matrix, x + 1, y - 1) && pathInPoint[x + 1, y - 1] != it) { ++nx; --ny; }
                    else if (IsEdgePoint(matrix, x, y - 1) && pathInPoint[x, y - 1] != it) { --ny; }

                    // if no neighbors have been found, abandon current path and start analyzing next one
                    if (nx == x && ny == y)
                    {
                        toRemove.Add(it);
                    }
                    else
                    {
                        // if this matrix point has not been explored yet, set its value
                        if (pathInPoint[nx, ny] == int.MaxValue)
                        {
                            pathInPoint[nx, ny] = it;
                            paths[it].Add(new Point(nx, ny));
                        }
                        // otherwise try to merge paths
                        else
                        {
                            int op = it;                    // old path
                            int np = pathInPoint[nx, ny];   // new path

                            // check if examined point has not been merged with any other path yet
                            if (!pathMerged[np])
                            {
                                paths[np].InsertRange(0, paths[op]);

                                //for (int i = paths[op].Count - 1; i >= 0; i--)
                                //{
                                //    paths[np].Add(paths[op][i]);
                                //}

                                paths[op].Clear();
                                //toRemove.Add(np);
                                pathMerged[np] = true;
                            }

                            pathMerged[op] = true;
                            toRemove.Add(it);
                        }
                    }
                }

                // remove explored paths
                if (toRemove.Count > 0)
                {
                    for (int i = 0; i < toRemove.Count; ++i)
                    {
                        int idx = toRemove[i];
                        activepaths.Remove(idx);
                    }
                    toRemove.Clear();
                }
            }

            int maxLength = paths[0].Count;
            int chosenPath = 0;

            for (int i = 1; i < paths.Count; ++i)
            {
                if (paths[i].Count > maxLength)
                {
                    maxLength = paths[i].Count;
                    chosenPath = i;
                }
            }

            return paths[chosenPath];
        }

        private bool IsEdgePoint(BitMatrix matrix, Point point)
        {
            return IsEdgePoint(matrix, point.X, point.Y);
        }

        // check if any of the 4 neighboring pixels are black
        private bool IsEdgePoint(BitMatrix matrix, int x, int y)
        {
            if (matrix.IsPointCorrect(x, y) && matrix[x, y])
            {
                if (x > 0 && !matrix[x - 1, y]) return true;
                if (x < matrix.Width - 1 && !matrix[x + 1, y]) return true;
                if (y > 0 && !matrix[x, y - 1]) return true;
                if (y < matrix.Height - 1 && !matrix[x, y + 1]) return true;
            }

            return false;
        }

        // also treats pixels outside of image bounds as black
        //private bool IsEdgePointBounds(BitMatrix matrix, int x, int y)
        //{
        //    if (matrix.IsPointCorrect(x, y) && matrix[x, y])
        //    {
        //        if (x == 0 || (x > 0 && !matrix[x - 1, y])) return true;
        //        if (x == matrix.Width - 1 || (x < matrix.Width - 1 && !matrix[x + 1, y])) return true;
        //        if (y == 0 || (y > 0 && !matrix[x, y - 1])) return true;
        //        if (y == matrix.Height - 1 || (y < matrix.Height - 1 && !matrix[x, y + 1])) return true;
        //    }

        //    return false;
        //}

        public Point FindCentroid(BitMatrix bm, List<Point> path)
        {
            List<Point> thisLayer = new List<Point>(path);
            List<Point> nextLayer;

            Point lastPoint = new Point();
            BitMatrix fbm = new BitMatrix(bm);

            // remove outline
            for (int i = 0; i < thisLayer.Count; ++i)
            {
                fbm[thisLayer[i].X, thisLayer[i].Y] = false;
            }

            // iterate over the outline in order to narrow it down to the center point
            while (thisLayer.Count > 0)
            {
                nextLayer = new List<Point>();
                lastPoint = thisLayer[0];
                for (int i = 0; i < thisLayer.Count; ++i)
                {
                    if (thisLayer[i].Y > 0 && fbm[thisLayer[i].X, thisLayer[i].Y - 1])
                    {
                        fbm[thisLayer[i].X, thisLayer[i].Y - 1] = false;
                        nextLayer.Add(new Point(thisLayer[i].X, thisLayer[i].Y - 1));
                    }
                    if (thisLayer[i].X < bm.Width - 1 && fbm[thisLayer[i].X + 1, thisLayer[i].Y])
                    {
                        fbm[thisLayer[i].X + 1, thisLayer[i].Y] = false;
                        nextLayer.Add(new Point(thisLayer[i].X + 1, thisLayer[i].Y));
                    }
                    if (thisLayer[i].Y < bm.Height - 1 && fbm[thisLayer[i].X, thisLayer[i].Y + 1])
                    {
                        fbm[thisLayer[i].X, thisLayer[i].Y + 1] = false;
                        nextLayer.Add(new Point(thisLayer[i].X, thisLayer[i].Y + 1));
                    }
                    if (thisLayer[i].X > 0 && fbm[thisLayer[i].X - 1, thisLayer[i].Y])
                    {
                        fbm[thisLayer[i].X - 1, thisLayer[i].Y] = false;
                        nextLayer.Add(new Point(thisLayer[i].X - 1, thisLayer[i].Y));
                    }
                }

                thisLayer = nextLayer;
            }

            return lastPoint;
        }
    }
}
