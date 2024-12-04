namespace AdventOfCode2024.Tasks
{
    public abstract class AdventTask
    {
        public string Filename = $"Inputs\\";

        public abstract void Solve1(string input);
        public abstract void Solve2(string input);

        public List<string> GetLinesList(string input)
        {
            return input.Split("\n").Select(l => l.Trim()).ToList();
        }

        public bool CheckIfIndexOutsideMatrix<T>(T[][] matrix, int row, int col)
        {
            return row >= matrix.Length || col >= matrix[0].Length || row < 0 || col < 0;
        }

        public string[] GetLinesArray(string input)
        {
            return input.Split("\n").Select(l => l.Trim()).ToArray();
        }
        public char[][] GetMatrixArray(string input)
        {
            return input.Split("\n").Select(l => l.Trim().ToCharArray()).ToArray();
        }

        protected enum Direction
        {
            North, West, South, East
        }

        protected (int Row, int Col) MakeMove((int Row, int Col) block, Direction movingDirection) =>
            movingDirection switch
            {
                Direction.West => (block.Row, block.Col - 1),
                Direction.East => (block.Row, block.Col + 1),
                Direction.South => (block.Row + 1, block.Col),
                Direction.North => (block.Row - 1, block.Col),
                _ => throw new Exception()
            };

        protected Direction GetPreviousDirection(Direction direction) =>
            direction switch
            {
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.West => Direction.East,
                Direction.East => Direction.West,
                _ => throw new Exception()
            };
    }
}
