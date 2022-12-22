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

           //HelperMethods.removeDuplicatePoints(ref outPoints);
           Point currentBestPoint = getSmallestYPoint(ref points);
           outPoints.Add(currentBestPoint);
            do
            {
                Point nextBestHullPoint = getNextBestHullPoint(ref points, currentBestPoint, ref outPoints);
                outPoints.Add(nextBestHullPoint);
                currentBestPoint = (Point)nextBestHullPoint.Clone();

            } while(!outPoints.First().Equals(outPoints.Last()));
            outPoints.RemoveAt(outPoints.Count - 1);
        }
        
        private Point getNextBestHullPoint(ref List<Point> points, Point currentBestPoint,ref List<Point> outpoints)
        {
            double angle, radians,largestAngle = double.MinValue;
            Point nextBestHullPoint = null;
            int removedPoint = -1;
            for (int index = 0; index < points.Count; index++)
            {
                if (!currentBestPoint.Equals(points[index]))
                {
                    radians = Math.Atan2(points[index].Y - currentBestPoint.Y, points[index].X - currentBestPoint.X);
                    angle = radians * 180 / Math.PI;
                    if (angle > largestAngle )
                    {
                        largestAngle = angle;
                        removedPoint = index;
                        nextBestHullPoint = (Point)points[index].Clone();
                    }
                }
            }
            points.RemoveAt(removedPoint);
            return nextBestHullPoint;
        }
        private Point getSmallestYPoint(ref List<Point> points)
        {
            Point smallestPoint = null;
            double smallestY = double.MaxValue;
            for (int index =0; index < points.Count; index++)
            {
                if (points[index].Y < smallestY)
                {
                    smallestPoint = points[index];
                    smallestY = points[index].Y;
                }
                if (points[index].Y == smallestY)
                {
                    if (points[index].X < smallestPoint.X)
                    {
                        smallestPoint = points[index];
                        smallestY = points[index].Y;
                    }
                }
            }
            return (Point)smallestPoint.Clone();
        }
        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
