using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Vector
    {
        public Vector(Point aPoint, Point bPoint)
        {
            APoint = aPoint;
            BPoint = bPoint;
        }
        public Point APoint { get; set; }
        public Point BPoint { get; set; }

        public override string ToString()
        {
            return $"From {APoint.ToString()} To {BPoint.ToString()}";
        }

    }
}
