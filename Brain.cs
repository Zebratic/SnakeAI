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
using SnakeAI.Algorithms;

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
                    returnValue = Direct2D.CalculateNextMove(headlocation, applelocation);
                    break;
                case Algorithm.AStar:
                    returnValue = TestAStar.CalculateNextMove(headlocation, applelocation, new IMap(Helper.GetTilemap(Snake.Gameinstance.ScreenWidth, Snake.Gameinstance.ScreenHeight, Snake.Gameinstance.DrawPoints)));
                    break;
                case Algorithm.Braindead:
                    break;
                default:
                    break;
            }

            short[,] tilemap = Helper.GetTilemap(Snake.Gameinstance.ScreenWidth, Snake.Gameinstance.ScreenHeight, Snake.Gameinstance.DrawPoints);
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

            //Debug.WriteLine(content);

            return returnValue;
        }
    }
}