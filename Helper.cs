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

        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }
    }
}