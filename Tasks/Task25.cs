namespace AdventOfCode2024.Tasks
{
    public class Task25: AdventTask
    {
        public Task25()
        {
            Filename += "25.txt";
        }

        public override void Solve1(string input)
        {
            long result = 0;
            var maps = input.Split("\r\n\r\n");
            var locks = new List<List<int>>();
            var keys = new List<List<int>>();
            var maxRows = 0;
            foreach (var map in maps)
            {
                var lines = GetLinesList(map);
                maxRows = lines.Count - 2;
                if (lines[0] == new string ('#', lines[0].Length))
                {
                    locks.Add(GetHeights(lines));
                } else
                {
                    lines.Reverse();
                    keys.Add(GetHeights(lines));
                }
            }

            foreach(var doorLock in locks)
                foreach(var key in keys)
                    if(!doorLock.Zip(key)
                        .Select(combination => combination.First + combination.Second)
                        .Any(val => val > maxRows))
                        result++;
            Console.WriteLine(result);
        }

        public override void Solve2(string input)
        {
            long result = 0;
            Console.WriteLine(result);
        }

        private List<int> GetHeights(List<string> lines)
        {
            var heigthsList = new List<int>();
            var col = 0;
            while (col < lines[0].Length)
            {
                for (var row = 1; row < lines.Count; row++)
                {
                    if (lines[row][col] == '.')
                    {
                        heigthsList.Add(row - 1);
                        break;
                    }
                }
                col++;
            }
            return heigthsList;
        }
    }
}
