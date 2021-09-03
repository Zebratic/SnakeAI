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
        public bool Walkable{ get; set; }
        public Point Position { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Node Parent { get; set; }

        public Node(Point position, Node parent = null, int cost = 1)
        {
            Position = position;
            Parent = parent;
            Cost = cost;
        }

        public bool IsWalkable() => Walkable;

        public void SetDistance(Point target)
        {
            this.Distance = Math.Abs(target.X - this.Position.X) + Math.Abs(target.Y - this.Position.Y);
        }
    }
}