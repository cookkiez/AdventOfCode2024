namespace AdventOfCode2024.Tasks
{
    public class Task11 : AdventTask
    {
        // For this task the problem is the second part as it evolves exponentialy. The key was to figure out that we do not need to track the order of the stones and 
        // that we only need to track occurences of each - this was done with the dictionary
        public Task11()
        {
            Filename += "11.txt";
        }

        // Performs operation on the current stone
        private List<string> ChangeStone(string stone) 
        {
            if (stone == "0") 
                return new List<string> { "1" };
            else if (stone.Length % 2 == 0)
            {
                var firstHalf = double.Parse(stone.Substring(0, stone.Length / 2));
                var secondHalf = double.Parse(stone.Substring(stone.Length / 2));
                return new List<string> { $"{firstHalf}", $"{secondHalf}" };
            }
            else
                return new List<string> { $"{long.Parse(stone) * 2024}" };
        }

        public override void Solve1(string input) => Console.WriteLine(SolveBoth(input, 25));

        public override void Solve2(string input) => Console.WriteLine(SolveBoth(input, 75));

        private long SolveBoth(string input, int iterations)
        {
            // Create initial stones dictionary with starting values
            var stones = input.Split(" ").ToList();
            var stonesDict = new Dictionary<string, long>();
            foreach (var stone in stones)
                AddToDict(stonesDict, stone, 1);

            for (var i = 0; i < iterations; i++)
            {
                // Use a temporary dictionary to store intermediate result. 
                // Perform operation on each stone type and add it to temporary dictionary. 
                var newStonesDict = new Dictionary<string, long>();
                foreach (var key in stonesDict.Keys)
                {
                    var newStones = ChangeStone(key);
                    foreach (var newStone in newStones)
                        AddToDict(newStonesDict, newStone, stonesDict[key]);
                }
                stonesDict = newStonesDict;
            }
            return stonesDict.Values.Sum();
        }

        private void AddToDict(Dictionary<string, long> dict, string key, long value)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, 0);
            dict[key] += value;
        }
    }
}
