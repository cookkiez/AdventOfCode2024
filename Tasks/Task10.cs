namespace AdventOfCode2024.Tasks
{
    public class Task10 : AdventTask
    {
        public Task10()
        {
            Filename += "10.txt";
        }

        public override void Solve1(string input)
        {
            long result = 0;
            var map = GetMatrixIntArray(input);
            for (var row = 0; row < map.Length; row++)
            {
                for (var col = 0; col < map[row].Length; col++)
                {
                    var nines = new HashSet<(int, int)>();
                    if (map[row][col] == 0)
                        IsTrailValid((row, col), map, new Dictionary<(int, int), int>(), nines);
                    result += nines.Count();
                }
            }
            Console.WriteLine(result);
        }

        public override void Solve2(string input)
        {
            long result = 0;
            var map = GetMatrixIntArray(input);
            for (var row = 0; row < map.Length; row++)
            {
                for (var col = 0; col < map[row].Length; col++)
                {
                    if (map[row][col] == 0)
                    {
                        var paths = new HashSet<string>();
                        GetAllTrails((row, col), map, new Dictionary<(int, int), int>(), paths, new List<(int, int)>());
                        var filteredPaths = paths.Distinct().Select(p => p.Split("(")).Where(p => p.Count() == 11).ToList();
                        result += filteredPaths.Count();
                    }
                }
            }
            Console.WriteLine(result);
        }

        private void GetAllTrails((int Row, int Col) position, int[][] map,
            Dictionary<(int, int), int> visitedPositions, HashSet<string> paths, List<(int, int)> currentPath)
        {
            if (CheckIfIndexOutsideMatrix<int>(map, position.Row, position.Col))
                return;

            currentPath.Add(position);
            var stringOfPath = string.Join("", currentPath);
            if (paths.Contains(stringOfPath))
                return;

            paths.Add(stringOfPath);
            if (map[position.Row][position.Col] == 9)
            {
                currentPath.Remove(position);
                return;
            }

            var result = 0;
            foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
            {
                //if (visitedPositions.ContainsKey(position))
                //    return;
                var nextPos = MakeMove(position, direction);
                if (CheckIfIndexOutsideMatrix<int>(map, nextPos.Row, nextPos.Col))
                    continue;
                var delta = map[nextPos.Row][nextPos.Col] - map[position.Row][position.Col];
                if (delta == 1)
                    GetAllTrails(MakeMove(position, direction), map, visitedPositions, paths, currentPath);
            }

            currentPath.Remove(position);
            //visitedPositions.Add(position, result);

            return;
        }

        private void IsTrailValid((int Row, int Col) position, int[][] map, 
            Dictionary<(int, int), int> visitedPositions, HashSet<(int, int)> nines)
        {
            if (CheckIfIndexOutsideMatrix<int>(map, position.Row, position.Col))
                return;

            if (map[position.Row][position.Col] == 9)
                nines.Add(position);

            var result = 0;
            foreach(var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
            {
                if (visitedPositions.ContainsKey(position))
                    return;
                var nextPos = MakeMove(position, direction);
                if (CheckIfIndexOutsideMatrix<int>(map, nextPos.Row, nextPos.Col))
                    continue;
                var delta = map[nextPos.Row][nextPos.Col] - map[position.Row][position.Col];
                if (delta == 1)
                    IsTrailValid(MakeMove(position, direction), map, visitedPositions, nines);
            }

            visitedPositions.Add(position, result);

            return;
        }
    }
}
