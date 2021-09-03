using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace SnakeAI.Algorithms
{
    class TestAStar
    {
        public static Snake.Direction CalculateNextMove(Point headLocation, Point appleLocation, IMap imap)
        {
            Snake.Direction returnValue = Snake.Gameinstance.SnakeDirection;
            var start = new Node(headLocation);
            var end = new Node(appleLocation);

            start.SetDistance(end.Position);

            var activeNodes = new List<Node>();
            activeNodes.Add(start);
            var visitedNodes = new List<Node>();

            while (activeNodes.Any())
            {
                Node checkNode = activeNodes.OrderBy(x => x.CostDistance).First();

                if (checkNode.Position == end.Position)
                {
                    var node = checkNode;
                    while (true)
                    {
                        Node nextNode = activeNodes.OrderBy(x => x.CostDistance).ToList().First();
                        if (nextNode.Walkable && !Snake.IsBody(nextNode.Position) && !Snake.IsWall(nextNode.Position))
                        {

                            activeNodes.Remove(nextNode);
                            Debug.WriteLine($"NODE: {nextNode.Position} | START: {headLocation}");
                            if (nextNode.Position == new Point(headLocation.X, headLocation.Y - 1))
                                returnValue = Snake.Direction.Down;
                            else if (nextNode.Position == new Point(headLocation.X, headLocation.Y + 1))
                                returnValue = Snake.Direction.Up;
                            else if (nextNode.Position == new Point(headLocation.X - 1, headLocation.Y))
                                returnValue = Snake.Direction.Right;
                            else if (nextNode.Position == new Point(headLocation.X + 1, headLocation.Y))
                                returnValue = Snake.Direction.Left;

                            Snake.Gameinstance.DrawString(nextNode.Position, Snake.Gameinstance.SnakeBody, ConsoleColor.Magenta);

                            return returnValue;
                        }
                        node = node.Parent;
                        if (node == null)
                        {
                            Debug.WriteLine("Done!");
                            return returnValue;
                        }
                    }
                }

                visitedNodes.Add(checkNode);
                activeNodes.Remove(checkNode);

                List<Node> walkableNodes = GetWalkableNodes(imap, checkNode, end);

                foreach (Node walkableNode in walkableNodes)
                {
                    //We have already visited this tile so we don't need to do so again!
                    if (visitedNodes.Any(x => x.Position == walkableNode.Position))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                    if (activeNodes.Any(x => x.Position == walkableNode.Position))
                    {
                        var existingTile = activeNodes.First(x => x.Position == walkableNode.Position);
                        if (existingTile.CostDistance > checkNode.CostDistance)
                        {
                            activeNodes.Remove(existingTile);
                            activeNodes.Add(walkableNode);

                            if (activeNodes.First().Position == new Point(headLocation.X, headLocation.Y - 1))
                                returnValue = Snake.Direction.Up;
                            else if (activeNodes.First().Position == new Point(headLocation.X, headLocation.Y + 1))
                                returnValue = Snake.Direction.Down;
                            else if (activeNodes.First().Position == new Point(headLocation.X - 1, headLocation.Y))
                                returnValue = Snake.Direction.Left;
                            else if (activeNodes.First().Position == new Point(headLocation.X + 1, headLocation.Y))
                                returnValue = Snake.Direction.Right;

                            Snake.Gameinstance.DrawString(activeNodes.First().Position, Snake.Gameinstance.SnakeBody, ConsoleColor.Magenta);

                            return returnValue;
                        }
                    }
                    else
                    {
                        //We've never seen this tile before so add it to the list. 
                        activeNodes.Add(walkableNode);
                    }
                }
            }


            Debug.WriteLine("No Path Found!");
            return Snake.Gameinstance.SnakeDirection;
        }

        private static List<Node> GetWalkableNodes(IMap imap, Node currentNode, Node targetNode)
        {
            List<Node> walkableNodes = new List<Node>();

            foreach (Node node in imap.Grid)
            {
                if (!Snake.IsWall(node.Position) && !Snake.IsBody(node.Position))
                {
                    if (node.Position == new Point(currentNode.Position.X, currentNode.Position.Y - 1))
                        walkableNodes.Add(node);
                    else if (node.Position == new Point(currentNode.Position.X, currentNode.Position.Y + 1))
                        walkableNodes.Add(node);
                    else if (node.Position == new Point(currentNode.Position.X - 1, currentNode.Position.Y))
                        walkableNodes.Add(node);
                    else if (node.Position == new Point(currentNode.Position.X + 1, currentNode.Position.Y))
                        walkableNodes.Add(node);
                }
            }

            foreach (Node node in walkableNodes)
            {
                Snake.Gameinstance.DrawString(node.Position, "*", ConsoleColor.Red);
            }

            walkableNodes.ForEach(node => node.SetDistance(targetNode.Position));

            return walkableNodes;

            /*
            return possibleNodes
                .Where(node => node.Position.X >= 0 && node.Position.X <= imap.Width)
                .Where(node => node.Position.Y >= 0 && node.Position.Y <= imap.Height)
                .Where(node => imap.Grid[node.Position.X, node.Position.Y].IsWalkable() || imap.Grid[node.Position.X, node.Position.Y].Position == targetNode.Position)
                .ToList();
            */
        }
    }
}