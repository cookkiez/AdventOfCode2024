using static System.Net.Mime.MediaTypeNames;
using System.IO;
using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode2024.Tasks
{
    public class Task20: AdventTask
    {
        public Task20()
        {
            Filename += "20.txt";
        }

        public override void Solve1(string input)
        {
            long result = 0;
            var map = GetMatrixArray(input);
            var start = (0, 0);
            for (int i = 0; i < map.Length; i++)
            {
                var trigger = false;
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 'S')
                    {
                        start = (i, j);
                        trigger = true;
                        break;
                    }
                }
                if (trigger)
                    break;
            }
            var (bestResult,  path) = DoBfs(start, 0, map, new HashSet<(int, int)>());
            var visited = new HashSet<(int, int)>();
            var score = 0;
            var saves = new Dictionary<long, long>();
            foreach (var key in path.Keys)
            {
                var (pos, posScore) = (key, path[key]);
                visited.Add(pos);
                foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    var (nextR, nextC) = MakeMove((pos.Row, pos.Col), direction);
                    if (!path.Keys.Contains((nextR, nextC)))
                    {
                        (nextR, nextC) = MakeMove((nextR, nextC), direction);
                        if (path.Keys.Contains((nextR, nextC)))
                        {
                            var temp = path[(nextR, nextC)] - path[(pos.Row, pos.Col)];
                            if (path[(nextR, nextC)] - path[(pos.Row, pos.Col)] > 100)
                            {
                                result++;
                            }
                            //(var tempResult, _) = DoBfs((nextR, nextC), score + 2, map, visited.ToHashSet());
                            //if (tempResult < long.MaxValue)
                            //{
                            //    var save = bestResult - tempResult;
                            //    if (!saves.ContainsKey(save))
                            //        saves.Add(save, 0);
                            //    saves[save] += 1;
                            //}
                        }
                    }  
                }
                score++;
            }
            //result = saves.Where(kvp => kvp.Key > 100).Sum(kvp => kvp.Value);
            Console.WriteLine(result);
        }

        public override void Solve2(string input)
        {
            long result = 0;

            Console.WriteLine(result);
        }

        private (long, Dictionary<(int Row, int Col), long>) DoBfs((int Row, int Col) start, long score, char[][] map, HashSet<(int, int)> visited)
        {
            var queue = new Queue<(int Row, int Col, long Score, Dictionary<(int Row, int Col), long> Path)>();
            queue.Enqueue((start.Row, start.Col, score, new Dictionary<(int Row, int Col), long>()));
            var bestScore = long.MaxValue;
            var bestPath = new Dictionary<(int Row, int Col), long>();
            while (queue.TryDequeue(out var pos))
            {
                if (map[pos.Row][pos.Col] == 'E')
                {
                    if (pos.Score < bestScore)
                    {
                        bestScore = pos.Score;
                        pos.Path.Add((pos.Row, pos.Col), pos.Score);
                        bestPath = pos.Path.ToDictionary();
                    }
                    continue;
                }
                if (pos.Score > bestScore)
                    continue;

                if (visited.Contains((pos.Row, pos.Col)))
                    continue;
                visited.Add((pos.Row, pos.Col));
                pos.Path.Add((pos.Row, pos.Col), pos.Score);

                foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    var (nextR, nextC) = MakeMove((pos.Row, pos.Col), direction);
                    if (!CheckIfIndexOutsideMatrix<char>(map, nextR, nextC) && map[nextR][nextC] != '#')
                        queue.Enqueue((nextR, nextC, pos.Score + 1, pos.Path.ToDictionary()));
                }
            }
            return (bestScore, bestPath);
        }
    }    
}
