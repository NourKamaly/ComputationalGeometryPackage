using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        private int pointA, pointB, pointC, comparedPoint;
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            for (pointA=0;pointA<points.Count; pointA++)
            {
                for (pointB= pointA+1;pointB<points.Count; pointB++)
                {
                    for(pointC= pointB+1;pointC<points.Count; pointC++)
                    {
                        for (comparedPoint = 0; comparedPoint < points.Count; comparedPoint++)
                        {
                            if (points[comparedPoint]!= points[pointA] && points[comparedPoint]!= points[pointB] && points[comparedPoint]!= points[pointC])
                            {
                                if (HelperMethods.PointInTriangle(points[comparedPoint], points[pointA], points[pointB], points[pointC])!= Enums.PointInPolygon.Outside)
                                {
                                    points.Remove(points[comparedPoint]);
                                    updateIndices(ref pointA,ref pointB, ref pointC, comparedPoint);
                                }
                            }
                        }
                    }
                }
            }
            outPoints = points;

        }
        private void updateIndices(ref int indexA,ref int indexB,ref int indexC,int indexOfRemovedPoint)
        {
            if (indexA > indexOfRemovedPoint)
            {
                indexA--;
            }
            if (indexB > indexOfRemovedPoint)
            {
                indexB--;
            }
            if(indexC > indexOfRemovedPoint)
            {
                indexC--;
            }
            
        }
        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
