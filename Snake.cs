using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public bool AIEnabled = true;
        public SnakeAI.Brain AIBrain { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public string SnakeBody = "█";
        public int SnakeLength { get; set; }
        public Point SnakeLocationHead { get; set; }
        public Direction SnakeDirection { get; set; }
        public List<Point> DrawPoints = new List<Point>();
        public Point AppleLocation { get; set; }
        public Point GetRandomPoint() => new Point(rnd.Next(0, ScreenWidth), rnd.Next(0, ScreenHeight));
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
            Gameinstance = new Snake();
            Gameinstance.AIBrain = new SnakeAI.Brain(SnakeAI.IQ.Pro);
            Gameinstance.AIEnabled = true;
            Gameinstance.StartGame();
        }

        public void StartGame()
        {
            while (true)
            {
                Console.WindowHeight = 16;
                Console.WindowWidth = 32;
                Console.CursorVisible = false;
                ScreenWidth = Console.WindowWidth;
                ScreenHeight = Console.WindowHeight;
                Console.SetBufferSize(ScreenWidth +1, ScreenHeight +1);
                SnakeLocationHead = GetRandomPoint();
                SnakeDirection = GetRandomDirection();
                SnakeLength = 2;
                SpawnApple();

                Thread moveThread = new Thread(RegisterMove);
                moveThread.Start();
                while (GameRunning)
                {
                    RenderFrame();
                    Thread.Sleep(50);
                }
                moveThread.Abort();

                Console.Clear();
                Console.WriteLine("Game Over");
                Console.ReadLine();
            }
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
                if (Console.KeyAvailable && KeyReset && !AIEnabled)
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
                else if (KeyReset && AIEnabled)
                {
                    SnakeDirection = SnakeAI.Pathfinding.GetNextMove(AIBrain, SnakeLocationHead, DrawPoints, AppleLocation);
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
            Console.Clear();

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

            DrawString(AppleLocation, "o", ConsoleColor.Green);

            // check collision
            foreach (Point pnt in DrawPoints)
            {
                if (SnakeLocationHead == pnt && pnt != DrawPoints[0])
                {
                    GameRunning = false;
                    return;
                }
                else if (SnakeLocationHead == AppleLocation)
                {
                    SnakeLength++;
                    SpawnApple();
                }
            }

            if (SnakeLocationHead.X >= ScreenWidth || SnakeLocationHead.Y >= ScreenHeight || SnakeLocationHead.X < 0 || SnakeLocationHead.Y < 0)
            {
                GameRunning = false;
                return;
            }

            // draw body
            foreach (Point pnt in DrawPoints)
                DrawString(pnt, SnakeBody);

            DrawString(SnakeLocationHead, SnakeBody, ConsoleColor.Red);
            
            DrawPoints.Add(SnakeLocationHead);

            KeyReset = true;
        }
    }
}