using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        private int pointA, pointB,comparedPoint,leftAndRightRotation,colinear;
        Line ab;
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            var extremePoints = new HashSet<Point>();
            for (pointA=0;pointA<points.Count; pointA++)
            {
                for (pointB=pointA+1;pointB<points.Count; pointB++)
                {
                    ab = new Line(points[pointA],points[pointB]);
                    leftAndRightRotation = 0;
                    colinear = 0;
                    for (comparedPoint=0;comparedPoint<points.Count; comparedPoint++)
                    {
                        if (points[comparedPoint]!=points[pointA] && points[comparedPoint] != points[pointB])
                        {
                            switch (HelperMethods.CheckTurn(ab, points[comparedPoint]))
                            {
                                case Enums.TurnType.Left: leftAndRightRotation--; break;
                                case Enums.TurnType.Right: leftAndRightRotation++; break;
                                default:colinear++; break;
                            }
                        }
                    }
                    if (Math.Abs(leftAndRightRotation) == (points.Count - 2 - colinear))
                    {
                        extremePoints.Add(points[pointA]);
                        extremePoints.Add(points[pointB]);
                    }
                }
            }
            outPoints = extremePoints.ToList();
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
