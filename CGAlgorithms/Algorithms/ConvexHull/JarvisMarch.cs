using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }

            Point smallestPoint = HelperMethods.getSmallestYPoint(ref points);
            Point currentBestPoint = (Point)smallestPoint.Clone(), potentialNextBestPoint = (Point)points[0].Clone();


            do
            {
                outPoints.Add(currentBestPoint);

                for (int index = 0; index < points.Count; index++)
                {
                    Point VectorCurrentPotential = new Point(currentBestPoint.X - potentialNextBestPoint.X, currentBestPoint.Y - potentialNextBestPoint.Y);
                    Point VectorCurrentNextPotential = new Point(currentBestPoint.X - points[index].X, currentBestPoint.Y - points[index].Y);

                    if (HelperMethods.CheckTurn(VectorCurrentPotential, VectorCurrentNextPotential) == Enums.TurnType.Left || (HelperMethods.CheckTurn(VectorCurrentPotential, VectorCurrentNextPotential) == Enums.TurnType.Colinear && !HelperMethods.PointOnSegment(points[index], currentBestPoint, potentialNextBestPoint)))
                    {
                        potentialNextBestPoint = points[index];
                    }
                }
                currentBestPoint = potentialNextBestPoint;

            } while (!potentialNextBestPoint.Equals(smallestPoint));


        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}