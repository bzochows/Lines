using System;

namespace Shared
{
    public class Point
    {
        public Point()
        {

        }
        public Point(int x, int y)
        {
            X = x;
            Y= y;
        }
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return $"X: {X}, Y:{Y}";
        }

        public override bool Equals(object obj)
        {
            var point = obj as Point;

            if (point == null)
            {
                return false;
            }
            return point.X == X && point.Y == Y;
        }

        public double GetDistance(Point targetPoint)
        {
            return Math.Sqrt(Math.Pow(this.X - targetPoint.X, 2) + Math.Pow(this.Y - targetPoint.Y, 2));
        }

        public void ScalePosition(int scale)
        {
            this.X = (X / scale) * scale;
            this.Y = (Y / scale) * scale;
        }
    }
}