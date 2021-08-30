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
            Direct,
            Smart,
            Braindead
        }

        public class Pathfinding
        {
            public static Snake.Direction GetNextMove(Brain brain, Point headlocation, List<Point> bodylocation, Point applelocation)
            {
                Snake.Direction returnValue = Snake.Gameinstance.SnakeDirection;
                switch (brain.IQ)
                {
                    case IQ.Direct:
                        returnValue = CalculateNextDirectMove(headlocation, bodylocation, applelocation);
                        break;
                    case IQ.Smart:
                        break;
                    case IQ.Braindead:
                        break;
                    default:
                        break;
                }

                return returnValue;
            }

            public static Snake.Direction CalculateNextDirectMove(Point headlocation, List<Point> bodylocation, Point applelocation)
            {
                fix:
                Snake.Direction returnValue = Snake.Gameinstance.SnakeDirection;
                try
                {
                    Point UpLoc = new Point(headlocation.X, headlocation.Y - 1);
                    Point DownLoc = new Point(headlocation.X, headlocation.Y + 1);
                    Point LeftLoc = new Point(headlocation.X - 1, headlocation.Y);
                    Point RightLoc = new Point(headlocation.X + 1, headlocation.Y);

                    bool BlockedUp = false;
                    bool BlockedDown = false;
                    bool BlockedLeft = false;
                    bool BlockedRight = false;

                    if (Snake.IsWall(UpLoc) || Snake.IsBody(UpLoc))
                        BlockedUp = true;
                    if (Snake.IsWall(DownLoc) || Snake.IsBody(DownLoc))
                        BlockedDown = true;
                    if (Snake.IsWall(LeftLoc) || Snake.IsBody(LeftLoc))
                        BlockedLeft = true;
                    if (Snake.IsWall(RightLoc) || Snake.IsBody(RightLoc))
                        BlockedRight = true;

                    // half broken
                    #region Check if apple is behind snake head direction
                    bool Flip = false;
                    if (Snake.Gameinstance.SnakeDirection == Snake.Direction.Up)
                        if (GetDistance(DownLoc, applelocation) < GetDistance(headlocation, applelocation))
                            Flip = true;

                    if (Snake.Gameinstance.SnakeDirection == Snake.Direction.Down)
                        if (GetDistance(UpLoc, applelocation) < GetDistance(headlocation, applelocation))
                            Flip = true;

                    if (Snake.Gameinstance.SnakeDirection == Snake.Direction.Left)
                        if (GetDistance(RightLoc, applelocation) < GetDistance(headlocation, applelocation))
                            Flip = true;

                    if (Snake.Gameinstance.SnakeDirection == Snake.Direction.Right)
                        if (GetDistance(LeftLoc, applelocation) < GetDistance(headlocation, applelocation))
                            Flip = true;

                    if (Flip)
                    {
                        Debug.WriteLine("FLIP");
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

                            if (GetDistance(UpLoc, applelocation) < GetDistance(DownLoc, applelocation) && !BlockedRight)
                                returnValue = Snake.Direction.Right;
                            else if (!BlockedUp)
                                returnValue = Snake.Direction.Up;
                            else
                                returnValue = Snake.Direction.Left;

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

                            if (GetDistance(UpLoc, applelocation) < GetDistance(DownLoc, applelocation) && !BlockedLeft)
                                returnValue = Snake.Direction.Left;
                            else if (!BlockedDown)
                                returnValue = Snake.Direction.Down;
                            else
                                returnValue = Snake.Direction.Right;

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

                            if (GetDistance(UpLoc, applelocation) < GetDistance(DownLoc, applelocation) && !BlockedUp)
                                returnValue = Snake.Direction.Up;
                            else if (!BlockedLeft)
                                returnValue = Snake.Direction.Left;
                            else
                                returnValue = Snake.Direction.Down;

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

                            if (GetDistance(UpLoc, applelocation) < GetDistance(DownLoc, applelocation) && !BlockedUp)
                                returnValue = Snake.Direction.Up;
                            else if (!BlockedRight)
                                returnValue = Snake.Direction.Right;
                            else
                                returnValue = Snake.Direction.Down;

                            return returnValue;
                        }
                    }

                    #endregion

                    double TempDistance = GetDistance(headlocation, applelocation);

                    redo:
                    TempDistance = 1000000;
                    Point newPnt;

                    if (BlockedUp && BlockedDown && BlockedLeft && BlockedRight)
                        return Snake.Gameinstance.SnakeDirection;

                    if (GetDistance(UpLoc, applelocation) < TempDistance && Snake.Gameinstance.SnakeDirection != Snake.Direction.Down && !BlockedUp)
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
                    if (GetDistance(DownLoc, applelocation) < TempDistance && Snake.Gameinstance.SnakeDirection != Snake.Direction.Up && !BlockedDown)
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
                    if (GetDistance(LeftLoc, applelocation) < TempDistance && Snake.Gameinstance.SnakeDirection != Snake.Direction.Right && !BlockedLeft)
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
                    if (GetDistance(RightLoc, applelocation) < TempDistance && Snake.Gameinstance.SnakeDirection != Snake.Direction.Left && !BlockedRight)
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

                    // fail safe check
                    if (returnValue == Snake.Direction.Up && Snake.IsBody(UpLoc) || returnValue == Snake.Direction.Up && Snake.IsWall(UpLoc))
                    {
                        BlockedUp = true;
                        goto redo;
                    }
                    else if (returnValue == Snake.Direction.Down && Snake.IsBody(DownLoc) || returnValue == Snake.Direction.Down && Snake.IsWall(DownLoc))
                    {
                        BlockedDown = true;
                        goto redo;
                    }
                    else if (returnValue == Snake.Direction.Left && Snake.IsBody(LeftLoc) || returnValue == Snake.Direction.Left && Snake.IsWall(LeftLoc))
                    {
                        BlockedLeft = true;
                        goto redo;
                    }
                    else if (returnValue == Snake.Direction.Right && Snake.IsBody(RightLoc) || returnValue == Snake.Direction.Right && Snake.IsWall(RightLoc))
                    {
                        BlockedRight = true;
                        goto redo;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ERROR:\n" + ex);
                    goto fix;
                }

                return returnValue;
            }

            public static double GetDistance(Point p1, Point p2)
            {
                return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
            }
        }
    }
}