using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SnakeAI.Algorithms.Helper;
using Node = SnakeAI.Node;


namespace SnakeAI.Algorithms.ok
{
    public class AStar
    {
        /*
        public static List<Node> Path = new List<Node>();
        public static Snake.Direction CalculateNextMove(Point headLocation, Point appleLocation, IMap imap)
        {
            Snake.Direction returnValue = Snake.Gameinstance.SnakeDirection;
            Node Start = new Node(headLocation, true);
            Node End = new Node(appleLocation, true);

            if (Path == null)
                Path = GetPath(imap, Start, End);

            Node nextNode = Path[0];

            // get direction
            if (nextNode.Location == new Point(headLocation.X, headLocation.Y - 1))
                returnValue = Snake.Direction.Up;
            else if (nextNode.Location == new Point(headLocation.X, headLocation.Y + 1))
                returnValue = Snake.Direction.Down;
            else if (nextNode.Location == new Point(headLocation.X - 1, headLocation.Y))
                returnValue = Snake.Direction.Left;
            else if (nextNode.Location == new Point(headLocation.X + 1, headLocation.Y))
                returnValue = Snake.Direction.Right;

            return returnValue;
        }

        private static List<Node> GetAdjacentNodes(IMap imap, Node node)
        {
            List<Node> temp = new List<Node>();

            int row = node.Location.X;
            int col = node.Location.Y;

            if (col + 1 < imap.Columns)
            {
                temp.Add(imap.Grid[row, col + 1]);
            }
            if (col - 1 >= 0)
            {
                temp.Add(imap.Grid[row, col - 1]);
            }
            if (row - 1 >= 0)
            {
                temp.Add(imap.Grid[row - 1, col]);
            }
            if (row + 1 < imap.Rows)
            {
                temp.Add(imap.Grid[row + 1, col]);
            }

            return temp;
        }

        public static List<Node> GetPath(IMap imap, Node start, Node end)
        {
            List<Node> Path = new List<Node>();
            List<Node> OpenList = new List<Node>();
            List<Node> ClosedList = new List<Node>();
            List<Node> adjacencies;
            Node current = start;

            // add start node to Open List
            OpenList.Add(start);

            while (OpenList.Count != 0 && !ClosedList.Exists(x => x.Location == end.Location))
            {
                current = OpenList[0];
                OpenList.Remove(current);
                ClosedList.Add(current);
                adjacencies = GetAdjacentNodes(imap, current);

                foreach (Node n in adjacencies)
                {
                    if (!ClosedList.Contains(n) && n.Walkable)
                    {
                        if (!OpenList.Contains(n))
                        {
                            n.Parent = current;
                            n.DistanceToTarget = Math.Abs(n.Location.X - end.Location.X) + Math.Abs(n.Location.Y - end.Location.Y);
                            n.Cost = n.Weight + n.Parent.Cost;
                            OpenList.Add(n);
                            Debug.WriteLine("bruh");
                            OpenList = OpenList.OrderBy(node => node.F).ToList<Node>();
                        }
                    }
                }
            }

            // construct path, if end was not closed return null
            if (!ClosedList.Exists(x => x.Location == end.Location))
            {
                return null;
            }

            // if all good, return path
            Node temp = ClosedList[ClosedList.IndexOf(current)];
            if (temp != null)
            {
                Path.Add(temp);
                temp = temp.Parent;
            }
            //while (temp != start && temp != null);
            return Path;
        }
        */
    }
}