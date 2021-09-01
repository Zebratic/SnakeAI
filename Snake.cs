using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SnakeAI.SnakeAI;

namespace SnakeAI
{
    public class Snake
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public static Snake Gameinstance = new Snake();
        public Random rnd = new Random();
        public bool GameRunning = true;
        public bool KeyReset = true;
        public bool AIEnabled { get; set; }
        public int SnakeSpeed { get; set; }
        public Brain AIBrain { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public string SnakeBody = "█";
        public int SnakeLength { get; set; }
        public Point SnakeLocationHead { get; set; }
        public Direction SnakeDirection { get; set; }
        public List<Point> DrawPoints = new List<Point>();
        public Point AppleLocation { get; set; }
        public Point GetRandomPoint() => new Point(rnd.Next(1, ScreenWidth + 2), rnd.Next(1, ScreenHeight));
        public Direction GetRandomDirection()
        {
            int i = rnd.Next(0, 4);
            switch (i)
            {
                case 0:
                    return Direction.Up;
                case 1:
                    return Direction.Left;
                case 2:
                    return Direction.Down;
                case 3:
                    return Direction.Right;
            }
            return Direction.Up;
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Gameinstance = new Snake();
                Gameinstance.AIBrain = new Brain(Algorithm.AStar);
                Gameinstance.AIEnabled = true;
                Gameinstance.ScreenWidth = 16;
                Gameinstance.ScreenHeight = 8;
                Gameinstance.SnakeSpeed = 250;
                Gameinstance.StartGame();
            }
        }

        public void StartGame()
        {
            Console.CursorVisible = false;
            Console.WindowWidth = Gameinstance.ScreenWidth + 3;
            Console.WindowHeight = Gameinstance.ScreenHeight + 1;
            //Console.SetBufferSize(Gameinstance.ScreenWidth, Gameinstance.ScreenHeight);
            SnakeLocationHead = GetRandomPoint();
            SnakeDirection = GetRandomDirection();
            SnakeLength = 3;
            SpawnApple();

            Thread moveThread = new Thread(RegisterMove);
            moveThread.Start();
            while (GameRunning)
            {
                if (AIEnabled)
                    SnakeDirection = GetNextMove(AIBrain, SnakeLocationHead, DrawPoints, AppleLocation);

                DrawBorder();
                RenderFrame();
                Thread.Sleep(Gameinstance.SnakeSpeed);
            }
            moveThread.Abort();

            string gameoverstring = "Game Over";
            string pointstring = $"{Gameinstance.SnakeLength} Points";
            Console.SetCursorPosition((Gameinstance.ScreenWidth / 2) - (gameoverstring.Length / 2) + 1, (Gameinstance.ScreenHeight / 2) - 1);
            Console.Write(gameoverstring);
            Console.SetCursorPosition((Gameinstance.ScreenWidth / 2) - (pointstring.Length / 2) + 1, (Gameinstance.ScreenHeight / 2));
            Console.Write(pointstring);
            Console.ReadLine();
        }

        public void DrawString(Point pnt, string str, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(pnt.X, pnt.Y);
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
        }

        public void RegisterMove()
        {
            while (true)
            {
                if (Console.KeyAvailable && KeyReset)
                {
                    if (AIEnabled)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        KeyReset = false;
                        if (key.Key == ConsoleKey.UpArrow)
                            Gameinstance.SnakeSpeed += 10;
                        else if (key.Key == ConsoleKey.DownArrow && Gameinstance.SnakeSpeed != 0)
                            Gameinstance.SnakeSpeed -= 10;

                        Debug.WriteLine(Gameinstance.SnakeSpeed);
                    }
                    else
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        KeyReset = false;
                        if (key.Key == ConsoleKey.UpArrow && SnakeDirection != Direction.Down)
                            SnakeDirection = Direction.Up;
                        else if (key.Key == ConsoleKey.LeftArrow && SnakeDirection != Direction.Right)
                            SnakeDirection = Direction.Left;
                        else if (key.Key == ConsoleKey.DownArrow && SnakeDirection != Direction.Up)
                            SnakeDirection = Direction.Down;
                        else if (key.Key == ConsoleKey.RightArrow && SnakeDirection != Direction.Left)
                            SnakeDirection = Direction.Right;
                    }
                }
            }
        }

        public void SpawnApple()
        {
            regenerate:
            Point NewAppleLocation = GetRandomPoint();
            foreach (Point pnt in DrawPoints)
                if (pnt == NewAppleLocation)
                    goto regenerate;

            AppleLocation = NewAppleLocation;
        } 

        public void RenderFrame()
        {
            //Console.Clear();
            
            // move
            if (SnakeDirection == Direction.Up)
                SnakeLocationHead = new Point(SnakeLocationHead.X, SnakeLocationHead.Y - 1);
            else if (SnakeDirection == Direction.Left)
                SnakeLocationHead = new Point(SnakeLocationHead.X - 1, SnakeLocationHead.Y);
            else if (SnakeDirection == Direction.Down)
                SnakeLocationHead = new Point(SnakeLocationHead.X, SnakeLocationHead.Y + 1);
            else if (SnakeDirection == Direction.Right)
                SnakeLocationHead = new Point(SnakeLocationHead.X + 1, SnakeLocationHead.Y);

            if (DrawPoints.Count > SnakeLength)
                DrawPoints.RemoveAt(0);


            if (IsWall(SnakeLocationHead) || IsBody(SnakeLocationHead))
            {
                GameRunning = false;
                return;
            }

            DrawString(AppleLocation, "X", ConsoleColor.Green);

            // draw body
            foreach (Point pnt in DrawPoints)
                DrawString(pnt, SnakeBody);

            if (SnakeDirection == Direction.Up)
                DrawString(SnakeLocationHead, "^", ConsoleColor.Red);
            else if (SnakeDirection == Direction.Down)
                DrawString(SnakeLocationHead, "v", ConsoleColor.Red);
            else if (SnakeDirection == Direction.Left)
                DrawString(SnakeLocationHead, "<", ConsoleColor.Red);
            else if (SnakeDirection == Direction.Right)
                DrawString(SnakeLocationHead, ">", ConsoleColor.Red);

            // check collision
            foreach (Point pnt in DrawPoints)
            {
                if (SnakeLocationHead == pnt && pnt != DrawPoints[0])
                {
                    Debug.WriteLine(SnakeDirection.ToString());
                    GameRunning = false;
                    return;
                }
                else if (SnakeLocationHead == AppleLocation)
                {
                    SnakeLength++;
                    SpawnApple();
                }
            }

            DrawPoints.Add(SnakeLocationHead);

            KeyReset = true;
        }

        public static bool IsBody(Point pnt)
        {
            try
            {
                foreach (Point bodyPnt in Snake.Gameinstance.DrawPoints)
                {
                    if (bodyPnt == pnt)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public void DrawBorder()
        {
            #region TOP
            DrawString(new Point(0, 0), "§", ConsoleColor.Cyan);

            for (int x = 1; x < Snake.Gameinstance.ScreenWidth + 2; x++)
                DrawString(new Point(x, 0), "-", ConsoleColor.Cyan);

            DrawString(new Point(Snake.Gameinstance.ScreenWidth + 2, 0), "§", ConsoleColor.Cyan);
            #endregion

            #region MID
            for (int y = 1; y < Gameinstance.ScreenHeight + 1; y++)
            {
                string line = "|";
                for (int i = 0; i < Gameinstance.ScreenWidth + 1; i++)
                    line += " ";
                line += "|";

                DrawString(new Point(0, y), line, ConsoleColor.Cyan);
            }
            #endregion

            #region BOTTOM
            DrawString(new Point(0, Snake.Gameinstance.ScreenHeight), "§", ConsoleColor.Cyan);

            for (int x = 1; x < Snake.Gameinstance.ScreenWidth + 2; x++)
                DrawString(new Point(x, Snake.Gameinstance.ScreenHeight), "-", ConsoleColor.Cyan);

            DrawString(new Point(Snake.Gameinstance.ScreenWidth + 2, Snake.Gameinstance.ScreenHeight), "§", ConsoleColor.Cyan);
            #endregion

            /*              §--------------------§
             *              |                    |
             *              |                    |
             *              |                    |
             *              |                    |
             *              |                    |
             *              |                    |
             *              |                    |
             *              |                    |
             *              §--------------------§
             */
        }

        public static bool IsWall(Point pnt)
        {
            if (pnt.X > Gameinstance.ScreenWidth + 1 || pnt.X < 1 || pnt.Y > Gameinstance.ScreenHeight - 1 || pnt.Y < 1)
                return true;
            else
                return false;
        }
    }
}