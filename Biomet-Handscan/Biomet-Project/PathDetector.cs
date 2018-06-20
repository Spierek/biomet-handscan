using Kaliko.ImageLibrary.BitFilters;
using LSTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
                                paths[np].Reverse();
                                paths[np].InsertRange(0, paths[op]);

                                paths[op].Clear();
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

        // returns 5 local max points (1 for each fingertip) and 4 local min points (spaces between fingers)
        public void FindFingerPoints(List<Point> path, Point centroid, out List<APair<int, double>> maximums, out List<APair<int, double>> minimums)
        {
            // find distances to centroid for each path point
            List<double> distances = new List<double>();
            for (int i = 0; i < path.Count; ++i)
            {
                Point p = path[i];
                if (p != null)
                {
                    double dist = Distance(p, centroid);
                    distances.Add(dist);
                }
            }

            List<double> blurredDistances = ListBlur(distances);

            maximums = FindMaximums(blurredDistances);
            minimums = FindMinimums(blurredDistances, maximums);
        }

        private void DetectFeatures()
        {
            // #TODO LS
        }


        // √((x2−x1)^2+(y2−y1)^2)
        private double Distance(Point a, Point b)
        {
            double x = Math.Pow((b.X - a.X), 2);
            double y = Math.Pow((b.Y - a.Y), 2);
            return Math.Sqrt(x + y);
        }

        // blur contents to get rid of sudden max/min spikes
        private List<double> ListBlur(List<double> data)
        {
            List<double> blurred = new List<double>(data.Count);
            blurred.AddRange(data);
            for (int i = 2; i < data.Count - 2; ++i)
            {
                blurred[i] = data[i] * 4 + (data[i + 1] + data[i - 1]) * 3 + data[i + 2] + data[i - 2];
                blurred[i] /= 11.0;
            }

            return blurred;
        }

        private List<APair<int, double>> FindMaximums(List<double> distances)
        {
            List<APair<int, double>> maximums = new List<APair<int, double>>();
            // find maximums (starting from a small offset to avoid rogue results)
            for (int i = (int)(distances.Count * 0.05f); i < distances.Count - 1; ++i)
            {
                // check if higher than next neighbors
                if (distances[i] >= distances[i - 1] && distances[i] >= distances[i + 1])
                {
                    // if within 32 pixels from last found maximum and the value is higher, replace
                    if (maximums.Count > 0 && Math.Abs(maximums[maximums.Count - 1].First - i) < 32)
                    {
                        APair<int, double> lastMax = maximums[maximums.Count - 1];
                        if (distances[i] > lastMax.Second)
                        {
                            maximums.Remove(lastMax);
                            maximums.Add(new APair<int, double>(i, distances[i]));
                        }
                    }
                    else
                    {
                        maximums.Add(new APair<int, double>(i, distances[i]));
                    }
                }
            }

            // narrow down to 5 highest results
            maximums = maximums.OrderByDescending(o => o.Second).ToList();
            maximums.RemoveRange(5, maximums.Count - 5);

            // re-sort by occurence
            maximums = maximums.OrderBy(o => o.First).ToList();

            return maximums;
        }

        private List<APair<int, double>> FindMinimums(List<double> distances, List<APair<int, double>> maximums)
        {
            List<APair<int, double>> minimums = new List<APair<int, double>>();
            for (int i = 0; i < 4; ++i)
            {
                int minJ = maximums[i].First;
                double minV = maximums[i].Second;

                // search between maximums
                for (int j = maximums[i].First; j < maximums[i + 1].First; ++j)
                {
                    if (distances[j] < minV)
                    {
                        minV = distances[j];
                        minJ = j;
                    }
                }

                minimums.Add(new APair<int, double>(minJ, minV));
            }

            return minimums;
        }
    }
}
