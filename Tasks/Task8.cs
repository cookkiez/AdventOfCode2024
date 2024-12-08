namespace AdventOfCode2024.Tasks
{
    public class Task8 : AdventTask
    {
        public Task8()
        {
            Filename += "8.txt";
        }

        public override void Solve1(string input)
        {
            var (matrix, antennas) = GetMatrixAndAntennas(input);
            var antinodes = GetAntinodes(matrix, antennas, false);
            Console.WriteLine(antinodes.Count);
        }

        public override void Solve2(string input)
        {
            var (matrix, antennas) = GetMatrixAndAntennas(input);
            var antinodes = GetAntinodes(matrix, antennas, true);
            Console.WriteLine(antinodes.Count);
        }

        private HashSet<(int, int)> GetAntinodes(char[][] matrix, Dictionary<char, List<(int, int)>> antennas, bool part2)
        {
            var visitedAntennas = new HashSet<((int, int), (int, int))>();
            var antinodes = new HashSet<(int, int)>();

            // Iterate over all antennas in the map, skipping the ones that appear only once
            foreach (var (_, antennaList) in antennas)
            {
                if (antennaList.Count < 2)
                    continue;
                foreach (var antenna in antennaList)
                {
                    foreach (var antenna2 in antennaList)
                    {
                        // If the antenna pair has already been visited, then don't examine it
                        if (antenna == antenna2 || visitedAntennas.Contains((antenna, antenna2)) ||
                            visitedAntennas.Contains((antenna2, antenna)))
                            continue;

                        // Get the delta for antenna pair to calculate antinodes from
                        var (drow, dcol) = (antenna.Item1 - antenna2.Item1, antenna.Item2 - antenna2.Item2);
                        visitedAntennas.Add((antenna, antenna2));
                        
                        // For part 1 we only need one antinode per pair, for part 2 we need all that fit in the grid. 
                        if (part2)
                        {
                            IterateOverAntenna(antenna, drow, dcol, matrix, 1, antinodes);
                            IterateOverAntenna(antenna2, drow, dcol, matrix, -1, antinodes);
                        } else
                        {
                            ApplyDeltaAndCheckIfAntinodeInMap(antenna, drow, dcol, matrix, 1, antinodes);
                            ApplyDeltaAndCheckIfAntinodeInMap(antenna2, drow, dcol, matrix, -1, antinodes);
                        }

                    }
                }
            }
            return antinodes;
        }

        private void IterateOverAntenna((int, int) antenna, int drow, int dcol, char[][] matrix, int multiplier, HashSet<(int, int)> antinodes) 
        {
            // while the antenna/antinode is in the map, apply delta and generate new antinode 
            antinodes.Add(antenna);
            while (IsAntinodeInMatrix(antenna, matrix))
                antenna = ApplyDeltaAndCheckIfAntinodeInMap(antenna, drow, dcol, matrix, multiplier, antinodes);
        }

        private (int, int) ApplyDeltaAndCheckIfAntinodeInMap((int, int) antenna, int drow, int dcol, char[][] matrix, int multiplier, HashSet<(int, int)> antinodes)
        {
            // Calculates the antinode based on the index and delta that are given to the function.
            // If the new antinode is in the map, add it to the list of antinodes
            var antinode = ApplyDelta(multiplier * drow, multiplier * dcol, antenna);
            if (IsAntinodeInMatrix(antinode, matrix))
                antinodes.Add(antinode);
            return antinode;
        }

        private bool IsAntinodeInMatrix((int, int) antinode, char[][] matrix) => !CheckIfIndexOutsideMatrix<char>(matrix, antinode.Item1, antinode.Item2);

        private (int, int) ApplyDelta(int drow, int dcol, (int, int) antenna) => (drow + antenna.Item1, dcol + antenna.Item2);

        private (char[][], Dictionary<char, List<(int, int)>>) GetMatrixAndAntennas(string input)
        {
            var matrix = GetMatrixArray(input);
            var antennas = new Dictionary<char, List<(int, int)>>();
            for (var row = 0; row < matrix.Length; row++)
            {
                for (var col = 0; col < matrix[row].Length; col++)
                {
                    var currChar = matrix[row][col];
                    if (currChar != '.')
                    {
                        if (!antennas.ContainsKey(currChar))
                            antennas[currChar] = new List<(int, int)>();
                        antennas[currChar].Add((row, col));
                    }
                }
            }
            return (matrix, antennas);
        }
    }
}
