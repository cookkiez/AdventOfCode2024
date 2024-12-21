namespace AdventOfCode2024.Tasks
{
    public class Task19 : AdventTask
    {
        public Task19()
        {
            Filename += "19.txt";
        }

        private void SolveBoth(string input, bool returnFirst)
        {
            long result = 0;
            var lines = GetLinesList(input);
            var towels = lines[0].Split(", ").OrderByDescending(t => t.Length).ToList();
            var towelCounter = new Dictionary<string, long>();
            foreach (var wantedTowel in lines.Skip(2))
            {
                result += IsTowelPossible(wantedTowel, towels, towelCounter, returnFirst);
            }
            Console.WriteLine(result);
        }

        public override void Solve1(string input) => SolveBoth(input, true);

        public override void Solve2(string input) => SolveBoth(input, false);

        private long IsTowelPossible(string wantedTowel, List<string> towels, Dictionary<string, long> towelCounter, bool returnFirst = true)
        {
            if (wantedTowel == "")
                return 1;
            if (towelCounter.ContainsKey(wantedTowel))
                return towelCounter[wantedTowel];
            long possibleTowels = 0;
            foreach(var towel in towels.Where(t => t.Length <= wantedTowel.Length))
            {
                if (wantedTowel.StartsWith(towel))
                {
                    possibleTowels += IsTowelPossible(wantedTowel.Substring(towel.Length), towels, towelCounter, returnFirst);
                    if (possibleTowels > 0 && returnFirst)
                        return possibleTowels;
                }
            }
            towelCounter.Add(wantedTowel, possibleTowels);
            return possibleTowels;
        }
    }    
}
