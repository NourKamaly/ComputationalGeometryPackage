using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        private List<Point> globalinput, globaloutput;
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }
            globalinput = points;
            globaloutput = outPoints;
            int counter; 
            Point north = null, south=null,east=null,west=null;
            getSmallestPoints(ref points,ref north,ref south,ref east,ref west);
            outPoints.Add(north); points.Remove(north);
            outPoints.Add(south); points.Remove(south);
            outPoints.Add(east);  points.Remove(east);
            outPoints.Add(west); points.Remove(west);
            List<Line> hullLines = new List<Line>();
            hullLines.Add(new Line(south, east));
            hullLines.Add(new Line(east, north)); 
            hullLines.Add(new Line(north, west));
            hullLines.Add(new Line(west, south));
            Enums.TurnType t;
            // remove all points in between hull polygon
            for (int index = 0; index < points.Count; index++)
            {
                counter = 0;
                for (int i = 0;i< 4; i++)
                {
                    if (HelperMethods.CheckTurn(hullLines[i], points[index]) == Enums.TurnType.Left) {
                        counter++;
                    }
                }
                if (counter == 4)
                {
                    points.RemoveAt(index);
                    index--;
                }
            }
            //south east north west
            quickHull(south, east);
            quickHull(east, north);
            quickHull(north, west);
            quickHull(west,south);
            HelperMethods.removeDuplicatePoints(ref globaloutput);

        }
        
        private void quickHull(Point hull, Point hull2)
        {
            if (globalinput.Count == 0)
            {
                return;
            }
            Line l = new Line(hull,hull2); Point nextHullPoint = null;
            double maxDistance = double.MinValue, distance;
            for (int i = 0;i< globalinput.Count; i++)
            {
                if (HelperMethods.CheckTurn(l,globalinput[i])==Enums.TurnType.Right)
                {
                    distance = euclideanDistance(ref hull,globalinput[i]) + euclideanDistance(ref hull2, globalinput[i]);
                    if (distance> maxDistance)
                    {
                        maxDistance = distance;
                        nextHullPoint = globalinput[i];

                    }
                }
            }
            if (nextHullPoint != null)
            {
                
                globaloutput.Add(nextHullPoint);
                
                globalinput.Remove(nextHullPoint);
                for (int i = 0; i < globalinput.Count; i++)
                {
                    if (HelperMethods.PointInTriangle(globalinput[i], hull, hull2, nextHullPoint) != Enums.PointInPolygon.Outside)
                    {
                        globalinput.Remove(globalinput[i]);
                        i--;
                    }
                }
                quickHull(hull, nextHullPoint);
                quickHull(nextHullPoint, hull2);
            }
        } 
        private double euclideanDistance(ref Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.Y - a.Y, 2)+ Math.Pow(b.X - a.X, 2));
        }
        private void getSmallestPoints(ref List<Point> points, ref Point north, ref Point south, ref Point east, ref Point west)
        {
            double smallestY = double.MaxValue, smallestX = double.MaxValue, largestY=double.MinValue,largestX=double.MinValue;
            for (int index = 0; index < points.Count; index++)
            {
                if (points[index].Y < smallestY)
                {
                    south = (Point)points[index].Clone();
                    smallestY = points[index].Y;
                }
                if (points[index].X < smallestX)
                {
                    west = (Point)points[index].Clone();
                    smallestX = points[index].X;
                }
                if (points[index].X > largestX)
                {
                    east = (Point)points[index].Clone();
                    largestX = points[index].X;

                }
                if (points[index].Y > largestY)
                {
                    north = (Point)points[index].Clone();
                    largestY = points[index].Y;
                }
            }
           
        }
        private double getDistanceBetweenPointAndLine(Point convexHullOne, Point convexHullTwo, Point comparedPoint)
        {
            return new double();
        }
        
            public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
