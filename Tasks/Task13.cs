namespace AdventOfCode2024.Tasks
{
    public class Task13 : AdventTask
    {
        public Task13()
        {
            Filename += "13.txt";
        }

        public override void Solve1(string input) => SolveBoth(input);

        public override void Solve2(string input) => SolveBoth(input, 10000000000000);

        private long SolveBoth(string input, long offset = 0)
        {
            long result = 0;
            var lines = input.Split('\n').Select(l => l.Trim()).ToList();
            for (var i = 0; i < lines.Count; i += 4)
            {
                (long X, long Y) buttonA = (GetXMove(lines[i]), GetYMove(lines[i]));
                (long X, long Y) buttonB = (GetXMove(lines[i + 1]), GetYMove(lines[i + 1]));
                (long X, long Y) prize = (GetXMove(lines[i + 2], "=", 1) + offset, GetYMove(lines[i + 2], "=", 2) + offset);

                var bMoves = (prize.Y * buttonA.X - prize.X * buttonA.Y) / (buttonB.Y * buttonA.X - buttonA.Y * buttonB.X);
                var aMoves = (prize.X - bMoves * buttonB.X) / buttonA.X;
                if (buttonA.X * aMoves + buttonB.X * bMoves != prize.X || buttonA.Y * aMoves + buttonB.Y * bMoves != prize.Y)
                    continue;
                result += bMoves + aMoves * 3;
            }
            Console.WriteLine(result);
            return result;
        }

        private long GetXMove(string str, string sep = "+", int spaceIndex = 2) => 
            long.Parse(str.Split(" ")[spaceIndex].Split(sep)[1].Replace(",", ""));
        private long GetYMove(string str, string sep = "+", int spaceIndex = 3) => 
            long.Parse(str.Split(" ")[spaceIndex].Split(sep)[1].Trim());
    }
}
