using System.Diagnostics;
using System.Drawing;

namespace SnakeAI.Algorithms
{
    public class IMap
    {
        public Node[,] Grid { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public IMap(short[,] grid)
        {
            Width = grid.GetLength(0);
            Height = grid.GetLength(1);

            Node[,] nodeGrid = new Node[Width, Height];

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    nodeGrid[x, y] = new Node(new Point(x, y));
                    if (grid[x, y] == 0)
                        nodeGrid[x, y].Walkable = true;
                    else
                        nodeGrid[x, y].Walkable = false;
                }
            }

            Grid = nodeGrid;
        }
    }
}