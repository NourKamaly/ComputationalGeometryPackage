using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {

        public List<Point> SwapPoints(Point p1, Point p2)
        {
            Point temp_p = p1;
            p1 = p2;
            p2 = temp_p;

            List<Point> NewPoints_After_swap = new List<Point>();
            NewPoints_After_swap.Add(p1);
            NewPoints_After_swap.Add(p2);
            return NewPoints_After_swap;
        }

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> uniqPoints = new List<Point>();
            uniqPoints.Add(points[0]);
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Equals(points[i - 1]))
                {
                    continue;
                }
                uniqPoints.Add(points[i]);
            }
            points = uniqPoints;



            if (points.Count == 1)
            {
                outPoints = points;
                return;
            }

            int lower_pp = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y < points[lower_pp].Y)
                    lower_pp = i;

            }
            List<Point> swapedPoints = new List<Point>();
            swapedPoints = SwapPoints(points[0], points[lower_pp]);
            points[0] = swapedPoints[0];
            points[lower_pp] = swapedPoints[1];



            points = points.OrderBy(point => point.Y).ToList();
            points = points.OrderBy(point => Math.Atan2(point.Y - points[0].Y, point.X - points[0].X)).ToList();


            List<Point> servived_points = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                if (i == 0 || i == 1 || i == 2)
                    servived_points.Add(points[i]);
                else
                {
                    servived_points.Add(points[i]);
                    Point p1, p2, p3;

                    // P3 -> P2 -> P1 
                    while (true)
                    {
                        p1 = servived_points[servived_points.Count - 1];
                        p2 = servived_points[servived_points.Count - 2];
                        p3 = servived_points[servived_points.Count - 3];

                        Line tempLine = new Line(p3, p2);
                       
                        if (HelperMethods.CheckTurn(tempLine, p1) == Enums.TurnType.Left)
                            break;
                        servived_points.Remove(p2);

                    }

                }
            }

            HelperMethods.removeDuplicatePoints(ref servived_points);
            outPoints = servived_points;

            ////////
            Point p_1  = servived_points[0];
            for (int i = 1;i< servived_points.Count-1;i++)
            {
                Point p_2 = servived_points[i];
                    bool onsegment = HelperMethods.PointOnSegment(p_1, p_2, servived_points[i+1]);
                        Enums.TurnType t = HelperMethods.CheckTurn(p_2, servived_points[i + 1]);

                        if (t == Enums.TurnType.Colinear||onsegment)
                        {
                        double d1 = Math.Sqrt(Math.Pow((p_1.X - p_2.X), 2) + Math.Pow((p_1.Y - p_2.Y), 2));
                        double d2 = Math.Sqrt(Math.Pow((p_2.X - servived_points[i+1].X), 2) + Math.Pow((p_2.Y - servived_points[i + 1].Y), 2));
                            if (d1 < d2)
                            {
                             Console.WriteLine("Removing");
                            Console.WriteLine(servived_points[i - 1].X);
                            Console.WriteLine(servived_points[i - 1].Y);
                        Console.WriteLine("////////////");
                        servived_points.Remove(servived_points[i -1]);
                            }
                        }
                //Console.WriteLine("Afteeeer");
                //Console.WriteLine(p_1.X);
                //Console.WriteLine(p_1.Y);
                //Console.WriteLine(p_2.X);
                //Console.WriteLine(p_2.Y);
                //Console.WriteLine("one iteration done");

            }
            servived_points = servived_points.OrderBy(point => point.Y).ToList();
            outPoints = servived_points;
            foreach (var p in outPoints)
            {
                Console.WriteLine(p.X);
                Console.WriteLine(p.Y);
            }

             ////////
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
