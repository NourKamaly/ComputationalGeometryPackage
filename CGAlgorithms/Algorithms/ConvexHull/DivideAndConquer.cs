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
            CGUtilities.HelperMethods.removeDuplicatePoints(ref points);
            if (points.Count < 8)
            {
                var ch = new ExtremeSegments();
                ch.Run(points, lines, polygons, ref outPoints, ref outLines, ref outPolygons);
                return;
            }
            points = points.OrderBy(point => point.X).ThenBy(point => point.Y).ToList();
            outPoints = Divide(points);
        }
        private List<Point> Divide(List<Point> points)
        {
            if (points.Count < 2)
            {
                return points;
            }

            int MI = (points.Count / 2);

            List<Point> left = Divide(points.GetRange(0, MI));
            List<Point> right = Divide(points.GetRange(MI, points.Count - MI));

            return Merge(left, right);
        }
        private Point getNextPoint(Point value, List<Point>points)
        {
            return points[(points.IndexOf(value) + 1) % points.Count];
        }

        private Point getPrevPoint(Point value, List<Point> points)
        {
            return (points.IndexOf(value) == 0 ? points[points.Count - 1] : points[points.IndexOf(value) - 1]);
        }

        private List<Point> Merge(List<Point> left, List<Point> right)
        {
            Point pointOnLeft = left.OrderByDescending(point => point.X).ThenByDescending(point => point.Y).FirstOrDefault();
            Point pointOnRight = right.OrderBy(point => point.X).ThenBy(point => point.Y).FirstOrDefault();
            
            // Up Supporting line
            Point upOnLeft = pointOnLeft;
            Point upOnRight = pointOnRight;
            bool rightChange, leftChange;

            do
            {
                rightChange = leftChange = false;
                while (CGUtilities.HelperMethods.CheckTurn(new Line(upOnRight, upOnLeft), getNextPoint(upOnLeft, left)) == Enums.TurnType.Right)
                {
                    upOnLeft = getNextPoint(upOnLeft, left);
                    leftChange = true;
                }

                if (!leftChange && CGUtilities.HelperMethods.CheckTurn(new Line(upOnRight, upOnLeft), getNextPoint(upOnLeft, left)) == Enums.TurnType.Colinear)
                    upOnLeft = getNextPoint(upOnLeft, left);

                while (CGUtilities.HelperMethods.CheckTurn(new Line(upOnLeft, upOnRight), getPrevPoint(upOnRight, right)) == Enums.TurnType.Left)
                {
                    upOnRight = getPrevPoint(upOnRight, right);
                    rightChange = true;
                }

                if (!rightChange && CGUtilities.HelperMethods.CheckTurn(new Line(upOnLeft, upOnRight), getPrevPoint(upOnRight, right)) == Enums.TurnType.Colinear)
                    upOnRight = getPrevPoint(upOnRight, right);
            }
            while (rightChange||leftChange);

            // Down Supporting Line
            Point downOnLeft = pointOnLeft;
            Point downOnRight = pointOnRight;
            do
            {
                rightChange = leftChange = false;
                while (CGUtilities.HelperMethods.CheckTurn(new Line(downOnRight, downOnLeft), getPrevPoint(downOnLeft, left)) == Enums.TurnType.Left)
                {
                    downOnLeft = getPrevPoint(downOnLeft, left);
                    leftChange = true;
                }

                if (!leftChange && CGUtilities.HelperMethods.CheckTurn(new Line(downOnRight, downOnLeft),
                    getPrevPoint(downOnLeft, left)) == Enums.TurnType.Colinear) downOnLeft = getPrevPoint(downOnLeft, left);

                while (CGUtilities.HelperMethods.CheckTurn(new Line(downOnLeft, downOnRight), getNextPoint(downOnRight, right)) == Enums.TurnType.Right)
                {
                    downOnRight = getNextPoint(downOnRight, right);
                    rightChange = true;
                }

                if (!rightChange && CGUtilities.HelperMethods.CheckTurn(new Line(downOnLeft, downOnRight),
                    getNextPoint(downOnRight, right)) == Enums.TurnType.Colinear) downOnRight = getNextPoint(downOnRight, right);
            }
            while (leftChange||rightChange);


            List<Point> ret = new List<Point>();

            Point v = upOnLeft;
            ret.Add(v);
            while (!v.Equals(downOnLeft))
            {
                v = getNextPoint(v, left);
                ret.Add(v);
            }

            v = downOnRight;
            ret.Add(v);
            while (!v.Equals(upOnRight))
            {
                v = getNextPoint(v, right);
                ret.Add(v);
            }

            return ret;
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}
