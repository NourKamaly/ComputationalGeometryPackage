using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            outPoints = RunDivideAndConquer(points, lines, polygons, ref outPoints, ref outLines, ref outPolygons);
        }
        private List<Point> RunDivideAndConquer(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            points = points.OrderBy(point => point.X).ToList();
            return Divide(points, lines, polygons, ref outPoints, ref outLines, ref outPolygons);
        }
        private List<Point> Divide(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count < 6)
            {
                var ch = new ExtremeSegments();
                ch.Run(points, lines, polygons,ref outPoints, ref outLines, ref outPolygons);
                return outPoints;
            }

            int MI = (points.Count / 2) - 1;
            List<Point> LCH = Divide(points.GetRange(0, MI), lines, polygons, ref outPoints, ref outLines, ref outPolygons);
            List<Point> RCH = Divide(points.GetRange(MI, points.Count - MI), lines, polygons, ref outPoints, ref outLines, ref outPolygons);

            return Merge(LCH, RCH);
        }

        private List<Point> Merge(List<Point> RCH, List<Point> LCH)
        {
            Point MLP = LCH.OrderBy(point => point.X).LastOrDefault(); // Min
            Point MRP = RCH.OrderBy(point => point.X).FirstOrDefault(); // Max

            // Up Supporting Line
            Point ULP = MLP;
            Point URP = MRP;
            Point NextLP = LCH[LCH.IndexOf(ULP) + 1];
            Point PreRP = RCH[RCH.IndexOf(URP) -1];

            Boolean URPChange, ULPChange;

            do
            {
                URPChange = ULPChange = false;
                while (CGUtilities.HelperMethods.CheckTurn(new Line(URP, ULP), NextLP)==Enums.TurnType.Right)
                {
                    ULP = NextLP;
                    NextLP = LCH[LCH.IndexOf(ULP) + 1];
                    ULPChange = true;
                }
                while(CGUtilities.HelperMethods.CheckTurn(new Line(ULP, URP), PreRP) == Enums.TurnType.Left)
                {
                    URP = PreRP;
                    PreRP = RCH[RCH.IndexOf(URP) - 1];
                    URPChange = true;
                }
            } 
            while (URPChange || ULPChange);

            // Down Supporting Line
            Point DLP = MLP;
            Point DRP = MRP;
            Point PreLP = LCH[LCH.IndexOf(DLP) - 1];
            Point NextRP = RCH[RCH.IndexOf(DRP) + 1];

            Boolean DRPChange, DLPChange;
            do
            {
                DRPChange = DLPChange = false;
                while (CGUtilities.HelperMethods.CheckTurn(new Line(DRP, DLP), PreLP) == Enums.TurnType.Left)
                {
                    DLP = PreLP;
                    PreLP = LCH[LCH.IndexOf(DLP) - 1];
                    DLPChange = true;
                }
                while (CGUtilities.HelperMethods.CheckTurn(new Line(DLP, DRP), NextRP) == Enums.TurnType.Right)
                {
                    DRP = NextRP;
                    NextRP = RCH[RCH.IndexOf(DRP) + 1];
                    DRPChange = true;
                }
            }
            while (DLPChange || DRPChange);

            List<Point> ret = new List<Point>();
            int startIndex = LCH.IndexOf(ULP);
            int endIndex = LCH.IndexOf(DLP);

            ret.AddRange(LCH.GetRange(startIndex, endIndex - startIndex));

            startIndex = LCH.IndexOf(DRP);
            endIndex = LCH.IndexOf(URP);

            ret.AddRange(LCH.GetRange(startIndex, endIndex - startIndex));
            
            return ret;
        }
        private Point getFirstLowerPoint(List<Point>points, Point currentPoint)
        {
            return points.Where(point => point.Y < currentPoint.Y).FirstOrDefault();
        }
        private Point getFirstHigherPoint(List<Point> points, Point currentPoint)
        {
            return points.Where(point => point.Y > currentPoint.Y).FirstOrDefault();
        }
        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}
