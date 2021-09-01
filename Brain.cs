using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SnakeAI.Algorithms.AStar;
using static SnakeAI.Algorithms.Direct2D.Pathfinding;

namespace SnakeAI
{
    public class SnakeAI
    {
        public class Brain
        {
            public Algorithm IQ { get; set; }
            public Brain(Algorithm iq)
            {
                IQ = iq;
            }
        }

        public enum Algorithm
        {
            Direct2D,
            AStar,
            Braindead
        }

        public static Snake.Direction GetNextMove(Brain brain, Point headlocation, List<Point> bodylocation, Point applelocation)
        {
            Snake.Direction returnValue = Snake.Gameinstance.SnakeDirection;
            switch (brain.IQ)
            {
                case Algorithm.Direct2D:
                    returnValue = CalculateNextDirectMove(headlocation, bodylocation, applelocation);
                    break;
                case Algorithm.AStar:
                    List<List<Node>> nodes = Algorithms.Helper.GetNodes(Algorithms.Helper.GetTilemap(Snake.Gameinstance.ScreenWidth, Snake.Gameinstance.ScreenHeight, Snake.Gameinstance.DrawPoints));
                    
                    Astar astar = new Astar(nodes);
                    Stack<Node> path = astar.FindPath(Snake.Gameinstance.SnakeLocationHead, Snake.Gameinstance.AppleLocation);
                    
                    Debug.WriteLine(path.First().Center);
                    
                    if (path.First().Center == new Point(Snake.Gameinstance.SnakeLocationHead.X, Snake.Gameinstance.SnakeLocationHead.Y - 1))
                        returnValue = Snake.Direction.Up;
                    else if (path.First().Center == new Point(Snake.Gameinstance.SnakeLocationHead.X, Snake.Gameinstance.SnakeLocationHead.Y + 1))
                        returnValue = Snake.Direction.Down;
                    else if (path.First().Center == new Point(Snake.Gameinstance.SnakeLocationHead.X - 1, Snake.Gameinstance.SnakeLocationHead.Y))
                        returnValue = Snake.Direction.Left;
                    else if (path.First().Center == new Point(Snake.Gameinstance.SnakeLocationHead.X + 1, Snake.Gameinstance.SnakeLocationHead.Y))
                        returnValue = Snake.Direction.Right;
                    break;
                case Algorithm.Braindead:
                    break;
                default:
                    break;
            }

            short[,] tilemap = Algorithms.Helper.GetTilemap(Snake.Gameinstance.ScreenWidth, Snake.Gameinstance.ScreenHeight, Snake.Gameinstance.DrawPoints);
            string content = "";

            content += "§";
            for (int xx = 1; xx < tilemap.GetLength(0); xx++)
                content += "-";

            content += "§\n";

            for (int y = 1; y < tilemap.GetLength(1); y++)
            {
                content += "|";
                for (int x = 1; x < tilemap.GetLength(0); x++)
                {
                    content += tilemap[x, y];
                }
                content += "|\n";
            }

            content += "§";

            for (int xx = 1; xx < tilemap.GetLength(0); xx++)
                content += "-";

            content += "§\n\n";

            Debug.WriteLine(content);

            return returnValue;
        }
    }
}