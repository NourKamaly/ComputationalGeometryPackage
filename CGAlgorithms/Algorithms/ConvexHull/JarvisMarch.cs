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
            //double test = calculateAngleBetweenTwoLines(new Point(1, 1), new Point(5, 1), new Point(6, 5));
            //HelperMethods.removeDuplicatePoints(ref points);
            Point pointA = new Point(0.0, 0.0), pointB = getSmallestYCoordinatePoint(points),nextBestPoint=null;
            outPoints.Add(pointB);
            double largestAngle, angle;
            do
            {
                largestAngle = double.MinValue;
                for (int index = 0;index< points.Count; index++)
                {
                    if (!points[index].Equals(pointA) && !points[index].Equals(pointB))
                    {
                        /*if (HelperMethods.PointOnSegment(points[index], pointA, pointB))
                        {
                            points.RemoveAt(index);
                            index--;
                        }*/
                       // else
                        //{
                            angle = calculateAngleBetweenTwoLines(pointA, pointB, points[index]);
                            if (angle > largestAngle)
                            {
                                largestAngle = angle;
                                nextBestPoint = points[index];
                            }
                        //}
                    }
                }
                outPoints.Add((Point)nextBestPoint.Clone());
                pointA = (Point)pointB.Clone();
                pointB = (Point)nextBestPoint.Clone();

            }while(!outPoints.First().Equals(outPoints.Last()));
            outPoints.Remove(outPoints.Last());
            
        }
      
        private double calculateAngleBetweenTwoLines(Point pointA, Point pointB, Point potentialNextBestPoint)
        {
            double angle, slopeAB,slopeBPotentilNextBestPoint, tanTheta;
            bool isPerpendicular = false;
            slopeAB = calculateSlope(pointA, pointB,ref isPerpendicular);
            /*if (isPerpendicular)
            {
                return 90;
            }*/
            slopeBPotentilNextBestPoint = calculateSlope(pointB, potentialNextBestPoint, ref isPerpendicular);
            /*if (isPerpendicular)
            {
                return 90;
            }*/
            tanTheta = Math.Abs((slopeBPotentilNextBestPoint - slopeAB) / (1 + slopeAB * slopeBPotentilNextBestPoint));
            // This equation calculates the acute angle betwen two lines, so to get the desired angle we should subtract the outputed 
            // number from 180 degress
            angle = Math.Atan(tanTheta)*180/Math.PI;
            //angle = 180 - angle;
            return angle;
        }
       
        private double calculateSlope(Point pointA, Point pointB, ref bool isPerpendicular)
        {
            if (pointB.X - pointA.X == 0)
            {
                isPerpendicular= true;
                return new double();
            }
            double slope = (double)(pointB.Y-pointA.Y)/(double)(pointB.X-pointA.X);
            return slope;
        }
        private Point getSmallestYCoordinatePoint(List<Point> points)
        {
            Point smallestPoint = null; int comparator = int.MaxValue;
            for (int index =0;index < points.Count; index++)
            {
                if (points[index].X<comparator)
                {
                    smallestPoint = points[index];
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
