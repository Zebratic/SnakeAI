using System;
using System.Collections.Generic;
using System.Drawing;

namespace SnakeAI.Algorithms
{
    public class Helper
    {
        public static short[,] GetTilemap(int width, int height, List<Point> Obstacles)
        {
            short[,] tilemap = new short[width + 2, height];

            for (int x = 0; x < width + 2; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (Obstacles.Contains(new Point(x, y)))
                        tilemap[x, y] = 1;
                    else
                        tilemap[x, y] = 0;
                }
            }

            return tilemap;
        }

        public static List<List<Node>> GetNodes(short[,] tilemap)
        {
            List<List<Node>> nodes = new List<List<Node>>();

            for (int x = 0; x < tilemap.GetLength(0); x++)
            {
                for (int y = 0; y < tilemap.GetLength(1); y++)
                {
                    bool walkable = true;
                    if (tilemap[x, y] == 1)
                        walkable = false;

                    Node node = new Node(new Point(x, y), walkable);
                    List<Node> nodelist = new List<Node>();
                    nodelist.Add(node);
                    nodes.Add(nodelist);
                }
            }

            return nodes;
        }

        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }
    }
}