namespace AdventOfCode2024.Tasks{
    public class Task18 : AdventTask
    {
        public Task18()
        {
            Filename += "18.txt";
        }

        public override void Solve1(string input)
        {
            var (maxCol, maxRow) = (71, 71);
            var byteIndexes = GetLinesList(input)
                .Select(line => (GetCoordinate(line, 1), GetCoordinate(line, 0)))
                .ToList();
            var goal = (maxRow - 1, maxCol - 1);
            byteIndexes = byteIndexes.Take(1024).ToList();

            Console.WriteLine(GetBestPath(byteIndexes, (0,0), 0, goal, maxRow, maxCol).Count);
        }

        public override void Solve2(string input)
        {
            var (maxCol, maxRow) = (71, 71);
            var byteIndexes = GetLinesList(input)
                .Select(line => (GetCoordinate(line, 0), GetCoordinate(line, 1)))
                .ToList();
            var goal = (maxRow - 1, maxCol - 1);
            var bestPath = GetBestPath(byteIndexes.Take(1024).ToList(), (0, 0), 0, goal, maxRow, maxCol);
            for (int i = 1025; i < byteIndexes.Count; i++)
            {
                if (bestPath.Contains(byteIndexes[i - 1]))
                {
                    bestPath = GetBestPath(byteIndexes.Take(i).ToList(), (0, 0), 0, goal, maxRow, maxCol);
                    if (bestPath.Count == 0)
                    {
                        Console.WriteLine(byteIndexes[i - 1]);
                        break;
                    }
                }
            }
        }

        private HashSet<(int, int)> GetBestPath(List<(int, int)> byteIndexes, (int, int) startPosition, long startScore, (int, int) goal, int maxRow, int maxCol)
        {
            var queue = new Queue<((int Col, int Row), HashSet<(int, int)>)>();
            queue.Enqueue((startPosition, new HashSet<(int, int)>()));
            var visited = new HashSet<(int, int)>();
            while (queue.TryDequeue(out var curr))
            {
                var (position, path) = curr;
                if (visited.Contains(position) ||
                    CheckIfIndexOutsideMatrix(maxRow, maxCol, position.Row, position.Col) ||
                    byteIndexes.Contains(position))
                    continue;

                var currPath = path.ToHashSet();
                currPath.Add(position);

                if (position == goal)
                {
                    return path;
                }
                

                visited.Add(position);

                foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    var newPos = MakeMove(position, direction);
                    queue.Enqueue((newPos, currPath));
                }
            }

            return new HashSet<(int, int)>();
        }

        private int GetCoordinate(string input, int index) => int.Parse(input.Split(",")[index]);
    }    
}
