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

namespace SnakeAI
{
    public class SnakeAI
    {
        public class Brain
        {
            public IQ IQ { get; set; }
            public Brain(IQ iq)
            {
                IQ = iq;
            }
        }

        public enum IQ
        {
            Pro,
            Smart,
            Average,
            Dumb,
            Braindead
        }

        public class Pathfinding
        {
            public static Snake.Direction GetNextMove(Brain brain, Point headlocation, List<Point> bodylocation, Point applelocation)
            {
                Snake.Direction returnValue = Snake.Gameinstance.SnakeDirection;
                switch (brain.IQ)
                {
                    case IQ.Pro:
                        returnValue = CalculateNextMove(headlocation, bodylocation, applelocation);
                        break;
                    case IQ.Smart:
                        break;
                    case IQ.Average:
                        break;
                    case IQ.Dumb:
                        break;
                    case IQ.Braindead:
                        break;
                    default:
                        break;
                }

                return returnValue;
            }

            public static Snake.Direction CalculateNextMove(Point headlocation, List<Point> bodylocation, Point applelocation)
            {
                Snake.Direction returnValue = Snake.Gameinstance.SnakeDirection;

                Point UpLoc = new Point(headlocation.X, headlocation.Y - 1);
                Point DownLoc = new Point(headlocation.X, headlocation.Y + 1);
                Point LeftLoc = new Point(headlocation.X - 1, headlocation.Y);
                Point RightLoc = new Point(headlocation.X + 1, headlocation.Y);

                bool BlockedUp = false;
                bool BlockedDown = false;
                bool BlockedLeft = false;
                bool BlockedRight = false;

                if (IsWall(UpLoc))
                    BlockedUp = true;
                if (IsWall(DownLoc))
                    BlockedDown = true;
                if (IsWall(LeftLoc))
                    BlockedLeft = true;
                if (IsWall(RightLoc))
                    BlockedRight = true;

                // half broken
                #region Check if apple is behind snake head direction
                bool Flip = false;
                if (Snake.Gameinstance.SnakeDirection == Snake.Direction.Up)
                    if (GetDistance(DownLoc, applelocation) < GetDistance(new Point(headlocation.X, headlocation.Y), applelocation))
                        Flip = true;

                if (Snake.Gameinstance.SnakeDirection == Snake.Direction.Down)
                    if (GetDistance(UpLoc, applelocation) < GetDistance(new Point(headlocation.X, headlocation.Y), applelocation))
                        Flip = true;

                if (Snake.Gameinstance.SnakeDirection == Snake.Direction.Left)
                    if (GetDistance(RightLoc, applelocation) < GetDistance(new Point(headlocation.X, headlocation.Y), applelocation))
                        Flip = true;

                if (Snake.Gameinstance.SnakeDirection == Snake.Direction.Right)
                    if (GetDistance(LeftLoc, applelocation) < GetDistance(new Point(headlocation.X, headlocation.Y), applelocation))
                        Flip = true;

                if (Flip)
                {
                    if (Snake.Gameinstance.SnakeDirection == Snake.Direction.Up)
                    {
                        foreach (Point bodyPnt in bodylocation)
                        {
                            if (bodyPnt == RightLoc)
                            {
                                BlockedRight = true;
                                if (bodyPnt == LeftLoc)
                                    BlockedLeft = true;
                            }
                        }

                        if (!BlockedRight && !BlockedLeft)
                        {
                            if (GetDistance(RightLoc, applelocation) < GetDistance(LeftLoc, applelocation))
                                returnValue = Snake.Direction.Right;
                            else
                                returnValue = Snake.Direction.Left;
                        }
                        else
                            returnValue = Snake.Direction.Up;

                        return returnValue;
                    }
                    if (Snake.Gameinstance.SnakeDirection == Snake.Direction.Down)
                    {
                        foreach (Point bodyPnt in bodylocation)
                        {
                            if (bodyPnt == LeftLoc)
                            {
                                BlockedLeft = true;
                                if (bodyPnt == RightLoc)
                                    BlockedRight = true;
                            }
                        }

                        if (!BlockedLeft && !BlockedRight)
                        {
                            if (GetDistance(LeftLoc, applelocation) < GetDistance(RightLoc, applelocation))
                                returnValue = Snake.Direction.Left;
                            else
                                returnValue = Snake.Direction.Right;
                        }
                        else
                            returnValue = Snake.Direction.Left;

                        return returnValue;
                    }
                    if (Snake.Gameinstance.SnakeDirection == Snake.Direction.Left)
                    {
                        foreach (Point bodyPnt in bodylocation)
                        {
                            if (bodyPnt == UpLoc)
                            {
                                BlockedUp = true;
                                if (bodyPnt == DownLoc)
                                    BlockedDown = true;
                            }
                        }

                        if (!BlockedUp && !BlockedDown)
                        {
                            if (GetDistance(UpLoc, applelocation) < GetDistance(DownLoc, applelocation))
                                returnValue = Snake.Direction.Up;
                            else
                                returnValue = Snake.Direction.Down;
                        }
                        else
                            returnValue = Snake.Direction.Left;

                        return returnValue;
                    }
                    if (Snake.Gameinstance.SnakeDirection == Snake.Direction.Right)
                    {
                        foreach (Point bodyPnt in bodylocation)
                        {
                            if (bodyPnt == DownLoc)
                            {
                                BlockedDown = true;
                                if (bodyPnt == UpLoc)
                                    BlockedUp = true;
                            }
                        }

                        if (!BlockedUp && !BlockedDown)
                        {
                            if (GetDistance(UpLoc, applelocation) < GetDistance(DownLoc, applelocation))
                                returnValue = Snake.Direction.Up;
                            else
                                returnValue = Snake.Direction.Down;
                        }
                        else
                            returnValue = Snake.Direction.Right;

                        return returnValue;
                    }
                }

                #endregion

                redo:
                if (BlockedUp && BlockedDown && BlockedLeft && BlockedRight)
                    return Snake.Gameinstance.SnakeDirection;

                Point newPnt;
                double TempDistance = GetDistance(headlocation, applelocation);
                if (GetDistance(new Point(headlocation.X, headlocation.Y - 1), applelocation) < TempDistance && Snake.Gameinstance.SnakeDirection != Snake.Direction.Down && !BlockedUp)
                {
                    newPnt = new Point(headlocation.X, headlocation.Y - 1);
                    TempDistance = GetDistance(newPnt, applelocation);
                    returnValue = Snake.Direction.Up;
                    foreach (Point bodyPnt in bodylocation)
                    {
                        if (bodyPnt == newPnt)
                        {
                            BlockedUp = true;
                            goto redo;
                        }
                    }
                }
                if (GetDistance(new Point(headlocation.X, headlocation.Y + 1), applelocation) < TempDistance && Snake.Gameinstance.SnakeDirection != Snake.Direction.Up && !BlockedDown)
                {
                    newPnt = new Point(headlocation.X, headlocation.Y + 1);
                    TempDistance = GetDistance(newPnt, applelocation);
                    returnValue = Snake.Direction.Down;
                    foreach (Point bodyPnt in bodylocation)
                    {
                        if (bodyPnt == newPnt)
                        {
                            BlockedDown = true;
                            goto redo;
                        }
                    }
                }
                if (GetDistance(new Point(headlocation.X - 1, headlocation.Y), applelocation) < TempDistance && Snake.Gameinstance.SnakeDirection != Snake.Direction.Right && !BlockedLeft)
                {
                    newPnt = new Point(headlocation.X - 1, headlocation.Y);
                    TempDistance = GetDistance(newPnt, applelocation);
                    returnValue = Snake.Direction.Left;
                    foreach (Point bodyPnt in bodylocation)
                    {
                        if (bodyPnt == newPnt)
                        {
                            BlockedLeft = true;
                            goto redo;
                        }
                    }
                }
                if (GetDistance(new Point(headlocation.X + 1, headlocation.Y), applelocation) < TempDistance && Snake.Gameinstance.SnakeDirection != Snake.Direction.Left && !BlockedRight)
                {
                    newPnt = new Point(headlocation.X + 1, headlocation.Y);
                    TempDistance = GetDistance(newPnt, applelocation);
                    returnValue = Snake.Direction.Right;
                    foreach (Point bodyPnt in bodylocation)
                    {
                        if (bodyPnt == newPnt)
                        {
                            BlockedRight = true;
                            goto redo;
                        }
                    }
                }

                return returnValue;
            }

            public static double GetDistance(Point p1, Point p2)
            {
                return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
            }

            public static bool IsWall(Point pnt)
            {
                if (pnt.X > Snake.Gameinstance.ScreenWidth || pnt.X < 0 || pnt.Y > Snake.Gameinstance.ScreenHeight || pnt.Y < 0)
                    return true;
                else
                    return false;
            }
        }
    }
}