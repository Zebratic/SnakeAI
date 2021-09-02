using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAI
{
    public class Node
    {
        public Node Parent;
        public Point Location { get; set; }
        public Point Center
        {
            get
            {
                return new Point(Location.X / 2, Location.Y / 2);
            }
        }
        public float DistanceToTarget;
        public float Cost;
        public float Weight;
        public float F
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                else
                    return -1;
            }
        }
        public bool Walkable { get; set; }

        public Node(Point location, bool walkable)
        {
            Location = location;
            Walkable = walkable;
        }
    }
}