namespace AdventOfCode2024.Tasks
{
    public class Task16 : AdventTask
    {
        public Task16()
        {
            Filename += "16.txt";
        }

        public override void Solve1(string input)
        {
            long result = 0;
            var map = GetMatrixArray(input);
            (int Row, int Col, Direction dir) start = (0, 0, Direction.East);
            var end = (0, 0);
            var queue = new Queue<(int, int, Direction, long, HashSet<(int, int)>)>();
            for (int i = 0; i < map.Length; i++)
            {
                for(int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 'S')
                    {
                        queue.Enqueue((i, j, Direction.East, 0, new HashSet<(int, int)> { (i, j) }));
                        break;
                    }
                }
                if (queue.Any())
                    break;
            }

            var dict = new Dictionary<(int, int, Direction), long>();
            var pathsDict = new Dictionary<HashSet<(int, int)>, long>();
            while (queue.TryDequeue(out var position))
            {
                var (row, col, dir, score, path) = position;
                if (map[row][col] == 'E' && (result == 0 || score <= result))
                {
                    result = score;
                    pathsDict.Add(path, score);
                    continue;
                }
                if (dict.ContainsKey((row, col, dir)))
                {
                    if (dict[(row, col, dir)] < score)
                        continue;
                    else
                        dict[(row, col, dir)] = score;
                }
                else
                    dict.Add((row, col, dir), score);

                path.Add((row, col));
                if (result != 0 && score > result)
                    continue; // Skip paths that are already more costly
                var (nextR, nextC) = MakeMove((row, col), dir);
                
                if (!CheckIfIndexOutsideMatrix<char>(map, nextR, nextC) && map[nextR][nextC] != '#')
                    queue.Enqueue((nextR, nextC, dir, score + 1, path.ToHashSet()));
                
                foreach(var nextDir in Enum.GetValues(typeof(Direction)).Cast<Direction>().Where(d => d != dir))
                {
                    (nextR, nextC) = MakeMove((row, col), nextDir);
                    if (!CheckIfIndexOutsideMatrix<char>(map, nextR, nextC) && map[nextR][nextC] != '#')
                        queue.Enqueue((nextR, nextC, nextDir, score + 1001, path.ToHashSet()));
                }
            }

            var tilesOnPaths = new HashSet<(int, int)>();
            foreach(var key in pathsDict.Keys)
            {
                if (pathsDict[key] > result)
                    continue;
                foreach (var tile in key)
                    tilesOnPaths.Add(tile);
            }

            //for (int i = 0; i < map.Length; i++)
            //{
            //    for (int j = 0; j < map[i].Length; j++)
            //    {
            //        if (map[i][j] == '#')
            //            Console.Write("#");
            //        else if (map[i][j] == '.')
            //        {
            //            if (tilesOnPaths.Contains((i, j)))
            //                Console.Write("O");
            //            else
            //                Console.Write(".");
            //        }
            //    }
            //    Console.WriteLine();
            //}
            Console.WriteLine(result);
            Console.WriteLine(tilesOnPaths.Count + 1);
        }

        public override void Solve2(string input)
        {
            long result = 0;
            Console.WriteLine(result);
        }
    }    
}
