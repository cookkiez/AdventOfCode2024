using static System.Net.Mime.MediaTypeNames;
using System.IO;
using static System.Formats.Asn1.AsnWriter;
using System.Numerics;
using System.ComponentModel;

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
                                result++;
                        }
                    }  
                }
                score++;
            }
            Console.WriteLine(result);
        }

        public override void Solve2(string input)
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
            var (bestResult, path) = DoBfs(start, 0, map, new HashSet<(int, int)>());
            //var finalCheats = new Dictionary<long, long>();
            foreach (var key in path.Keys)
            {
                var (pos, posScore) = (key, path[key]);
                foreach (var key2 in path.Keys)
                {
                    if (key == key2)
                        continue;
                    var (pos2, posScore2) = (key2, path[key2]);
                    var manhatan = Math.Abs(pos.Row - pos2.Row) + Math.Abs(pos.Col - pos2.Col);
                    if (manhatan <= 20 && posScore2 - posScore - manhatan >= 100)
                        result++;

                }
                // Missed attempt. Keeping for sanity
                //var (pos, posScore) = (key, path[key]);
                //var cheats = FindCheats(pos, 0, map, path);
                //foreach(var cheatVal in cheats.Values)
                //{
                //    if (!finalCheats.ContainsKey(cheatVal))
                //        finalCheats.Add(cheatVal, 0);
                //    finalCheats[cheatVal]++;
                //} 
            }
            Console.WriteLine(result);
        }

        // Missed attempt. Keeping for sanity
        private Dictionary<(int, int), long> FindCheats((int Row, int Col) start, long score, char[][] map, Dictionary<(int Row, int Col), long> bestPath)
        {
            var cheats = new Dictionary<(int, int), long>();
            var queue = new Queue<(int Row, int Col, long Score, bool HasCheated)>();
            queue.Enqueue((start.Row, start.Col, 0, false));
            var visited = new HashSet<(int Row, int Col, long Score, bool HasCheated)>();
            while (queue.TryDequeue(out var pos))
            {
                var posTuple = (pos.Row, pos.Col);
                var posScore = bestPath[start] + pos.Score;
                if (pos.HasCheated && (map[pos.Row][pos.Col] == 'E' || bestPath.ContainsKey(posTuple)))
                { 
                    var cheatScore = bestPath[posTuple] - posScore;
                        
                    if (cheatScore >= 50 && (!cheats.ContainsKey(posTuple) ||
                        (cheats.ContainsKey(posTuple) && cheats[posTuple] < posScore)))
                        cheats[posTuple] = cheatScore;
                }
                if (pos.Score > 20)
                    continue;

                if (visited.Contains(pos))
                    continue;
                visited.Add(pos);

                foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    var (nextR, nextC) = MakeMove((pos.Row, pos.Col), direction);
                    if (!CheckIfIndexOutsideMatrix<char>(map, nextR, nextC))
                        queue.Enqueue((nextR, nextC, pos.Score + 1, pos.HasCheated || map[nextR][nextC] == '#'));
                }
            }


            return cheats;
        }

        private (long, Dictionary<(int Row, int Col), long>) DoBfs((int Row, int Col) start, long score, char[][] map, 
            HashSet<(int, int)> visited)
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
