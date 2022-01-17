using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class AStarAlgorithm
    {
        public static async Task<List<Point>> GetPath(Point startPoint, Point targetPoint, List<Point> obstackles, int scale, int boardWidth, int boardHeight)
        {
            List<AStarPoint> neighbours = new List<AStarPoint>();
            bool cantFindPath = false;
            await Task.Factory.StartNew(() =>
            {
                AStarPoint aStartStartPoint = new AStarPoint(startPoint.X, startPoint.Y, null);
                aStartStartPoint.IsChecked = true;
                aStartStartPoint.DistanceFromStart = 0;
                aStartStartPoint.ExpectedDistanceToTarget = aStartStartPoint.GetDistance(targetPoint);
                List<AStarPoint> resultPoints = new List<AStarPoint>();
                neighbours = GetNeighbours(aStartStartPoint, targetPoint, obstackles, scale, boardWidth, boardHeight);
                while (true)
                {
                    if (neighbours.All(d => d.IsChecked)) // if all neighbours are checked - theres no path between points
                    {
                        cantFindPath = true;
                        break;
                    }
                    //search new possible path point and set it as checked
                    var newPoint = neighbours.Where(d => !d.IsChecked).OrderBy(d => d.DistanceFromStart + d.ExpectedDistanceToTarget).First();
                    if (newPoint.ExpectedDistanceToTarget == 0)
                        break;
                    newPoint.IsChecked = true;
                    neighbours.AddRange(GetNeighbours(newPoint, targetPoint, obstackles.Concat(neighbours).ToList(), scale, boardWidth, boardHeight));

                }
            });
            if (cantFindPath)
                return null;
            var lastPoint = neighbours.Single(d => d.ExpectedDistanceToTarget == 0);
            var parents = GetParents(lastPoint).Select(d => new Point(d.X, d.Y)).ToList();
            return parents;

        }
        public static List<AStarPoint> GetNeighbours(AStarPoint startPoint, Point targetPoint, List<Point> disabledPoints, int scale, int boardWidth, int boardHeight)
        {
            List<AStarPoint> neighbours = new List<AStarPoint>();


            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++) //loops for searching adjacent points
                {
                    if (i == 0 && j == 0)
                        continue;
                    AStarPoint neighbourPoint = new AStarPoint(startPoint.X + i * scale, startPoint.Y + j * scale, startPoint);
                    if ((neighbourPoint.X > boardWidth || neighbourPoint.Y > boardHeight ||neighbourPoint.X < 0 || neighbourPoint.Y < 0) || disabledPoints.Any(d => d.Equals(neighbourPoint))) // validate points
                        continue;
                    else
                    {
                        neighbourPoint.DistanceFromStart = startPoint.DistanceFromStart + startPoint.GetDistance(neighbourPoint);
                        neighbourPoint.ExpectedDistanceToTarget = targetPoint.GetDistance(neighbourPoint);
                        neighbours.Add(neighbourPoint);
                    }
                }
            }
            return neighbours;
        }

        public static List<AStarPoint> GetParents(AStarPoint point)
        {
            var list = new List<AStarPoint>();
            if (point.ParentPoint == null)
                return list;
            else
            {
                GetParents(point, list);
            }
            return list;
        }
        private static void GetParents(AStarPoint point, List<AStarPoint> points)
        {

            points.Add(point);
            if(point.ParentPoint != null)
                GetParents(point.ParentPoint, points);
        }
    }
}
