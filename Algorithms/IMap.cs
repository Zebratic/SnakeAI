namespace SnakeAI.Algorithms
{
    public class IMap
    {
        public Node[,] Grid { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public IMap(short[,] grid)
        {
            Rows = grid.GetLength(0);
            Columns = grid.GetLength(1);

            Node[,] nodeGrid = new Node[Rows, Columns];

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x, y] == 0)
                        nodeGrid[x, y].Walkable = true; // null?
                    else
                        nodeGrid[x, y].Walkable = false;
                }
            }

            Grid = nodeGrid;
        }
    }
}
