using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class AStarPoint : Point
    {
        public AStarPoint() : base()
        {

        }
        public AStarPoint(int x, int y, AStarPoint parentPoint) : base(x, y)
        {
            ParentPoint = parentPoint;
        }
        public double DistanceFromStart { get; set; }
        public double ExpectedDistanceToTarget { get; set; }
        public AStarPoint ParentPoint { get; set; }
        public bool IsChecked { get; set; }
    }
}
