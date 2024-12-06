namespace AdventOfCode2024.Tasks
{
    public class Task6 : AdventTask
    {
        public Task6()
        {
            Filename += "6.txt";
        }
        
        private class LoopException : Exception { }

        public override void Solve1(string input)
        {
            var result = 0;
            var (startPoint, map) = GetStartPointAndMap(input);

            var visitedPoints = GetVisitedPoints(startPoint, map);
            result = visitedPoints.Distinct().Count();
            Console.WriteLine(result);
        }

        private ((int Row, int Col), List<List<char>>) GetStartPointAndMap(string input)
        {
            // Iterate over the matrix to get the starting point of the guard
            var map = GetMatrixList(input);
            (int Row, int Col) startPoint = (0, 0);
            for (var row = 0; row < map.Count; row++)
            {
                var startCol = map[row].IndexOf('^');
                if (startCol != -1)
                {
                    startPoint = (row, startCol);
                    break;
                }

            }
            return (startPoint, map);
        }

        private Direction GetNextDirection(Direction prev) => prev switch 
        { 
            Direction.North => Direction.East,
            Direction.South => Direction.West,
            Direction.East => Direction.South,
            Direction.West => Direction.North,
            _ => throw new NotImplementedException(),
        };

        private List<(int Row, int Col)> GetVisitedPoints((int Col, int Row) startPoint, List<List<char>> map, bool part2 = false) 
        {
            // Loop that walks through the matrix by the given rules - when hitting '#' turn right for 90° and continue until
            // you leave the map. In part 2 we need to discover a loop in the walkthrough.
            // For visited points we need row, col and direction to detect a loop in the second part.
            var visitedPoints = new HashSet<((int Row, int Col), Direction)>();
            var movingDirection = Direction.North;
            while (!CheckIfIndexOutsideListMatrix(map, startPoint.Row, startPoint.Col))
            {
                visitedPoints.Add((startPoint, movingDirection));
                var nextPoint = MakeMove(startPoint, movingDirection);

                //  if we hit an obstacle in the next point, we iterate over all directions to find the next logical move
                var checkedDirections = new HashSet<Direction> { movingDirection };
                while (!CheckIfIndexOutsideListMatrix(map, nextPoint.Row, nextPoint.Col) && map[nextPoint.Row][nextPoint.Col] == '#')
                {
                    movingDirection = GetNextDirection(movingDirection);
                    if (checkedDirections.Contains(movingDirection))
                        break;
                    nextPoint = MakeMove(startPoint, movingDirection);
                }
                // Found a loop, throw exception to easily catch it outside and increment result
                if (part2 && visitedPoints.Contains((nextPoint, movingDirection)))
                    throw new LoopException();
                startPoint = nextPoint;
            }
            // For first part we want to know the number of distinct visited points
            return visitedPoints.Select(p => p.Item1).Distinct().ToList();
        }

        public override void Solve2(string input)
        {
            var result = 0;
            var (startPoint, map) = GetStartPointAndMap(input);
            var visitedPoints = GetVisitedPoints(startPoint, map);
            visitedPoints.Remove(startPoint);
            foreach (var point in visitedPoints)
            {
                // Replace point in original walkthrough with an obstacle and go over the walk again to detect a loop
                map[point.Row][point.Col] = '#';
                try
                {
                    GetVisitedPoints(startPoint, map, true);
                } catch (LoopException ex) 
                {
                    result++;
                }
                map[point.Row][point.Col] = '.';
            }
            Console.WriteLine(result);
        }
    }
}
