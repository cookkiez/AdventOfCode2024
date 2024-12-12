using System.ComponentModel;
using System.IO;

namespace AdventOfCode2024.Tasks
{
    public class Task12 : AdventTask
    {
        public Task12()
        {
            Filename += "12.txt";
        }

        public override void Solve1(string input) {
            long result = 0;
            long result2 = 0;
            var matrix = GetMatrixArray(input);
            var visited = new HashSet<(int, int)>();
            for(var i = 0; i < matrix.Length; i++)
            {
                for (var j = 0; j < matrix[i].Length; j++)
                {
                    if (visited.Contains((i, j)))
                        continue;
                    var queue = new Queue<(int Row, int Col)>();
                    queue.Enqueue((i, j));
                    var path = new HashSet<(int, int)>();
                    var polygonDict = new Dictionary<(int, int), int>();
                    var currChar = matrix[i][j];
                    while (queue.TryDequeue(out var currPos))
                    {
                        if (visited.Contains(currPos))
                            continue;
                        path.Add(currPos);
                        visited.Add(currPos);
                        CheckAllDirectionsAndAddToQueue(currPos, matrix, currChar, visited, queue, polygonDict);
                    }

                    
                    var corners = 0;
                    foreach (var position in path)
                    {
                        foreach (var (dir1, dir2) in new List<(Direction, Direction)>
                        {
                            (Direction.South, Direction.West),
                            (Direction.South, Direction.East),
                            (Direction.North, Direction.West),
                            (Direction.North, Direction.East),
                        })
                        {
                            if (!path.Contains(MakeMove(position, dir1)) && 
                                !path.Contains(MakeMove(position, dir2)))
                                corners++;
                        }
                        foreach (var (dir1, dir2, diag) in new List<(Direction, Direction, DirectionDiag)>
                        {
                            (Direction.South, Direction.West, DirectionDiag.SouthWest),
                            (Direction.South, Direction.East, DirectionDiag.SouthEast),
                            (Direction.North, Direction.West, DirectionDiag.NorthWest),
                            (Direction.North, Direction.East, DirectionDiag.NorthEast),
                        })
                        {
                            if (path.Contains(MakeMove(position, dir1)) && 
                                path.Contains(MakeMove(position, dir2)) && 
                                !path.Contains(MakeMove(position, diag)))
                                corners++;
                        }
                    }

                    var perimiter = polygonDict.Values.Sum(val => 4 - val);
                    perimiter = perimiter == 0 ? 4 : perimiter;
                    result += path.Count * perimiter;
                    result2 += path.Count * corners;
                }
            }
            Console.WriteLine(result);
            Console.WriteLine(result2);
        }

        public override void Solve2(string input)
        {
            long result = 0;
            Console.WriteLine(result);
        }

        private void AddToDict(Dictionary<(int, int), int> dict, (int, int) key, int value)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, 0);
            dict[key] += value;
        }

        private void CheckAllDirectionsAndAddToQueue((int, int) currPos, char[][] matrix, char currChar,
            HashSet<(int, int)> visited, Queue<(int, int)> queue, Dictionary<(int, int), int> polygonDict)
        {
            foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
            {
                var nextPos = MakeMove(currPos, direction);
                if (CheckIfIndexOutsideMatrix<char>(matrix, nextPos.Row, nextPos.Col) ||
                    visited.Contains(nextPos))
                    continue;
                if (matrix[nextPos.Row][nextPos.Col] == currChar)
                {
                    queue.Enqueue(nextPos);
                    AddToDict(polygonDict, currPos, 1);
                    AddToDict(polygonDict, nextPos, 1);
                }
            }
        }
    }
}
