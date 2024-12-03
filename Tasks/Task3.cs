using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Tasks
{
    public class Task3 : AdventTask
    {
        public Task3()
        {
            Filename += "3.txt";
        }
        public override void Solve1(string input)
        {
            var multiplications = GetMatches(input);
            var result = MatchNumbersAndMultiply(multiplications);
            Console.WriteLine(result);
        }

        private List<Match> GetMatches(string input)
        {
            var regex = new Regex("mul[(]\\d+,\\d+[)]");
            return regex.Matches(input).ToList();
        }

        private static long MatchNumbersAndMultiply(List<Match> matches)
        {
            var numRegex = new Regex("\\d+");
            return matches.Select(mul => numRegex
                    .Matches(mul.Groups[0].Value)
                    .Select(num => num.Value)
                    .Select(int.Parse)
                    .Aggregate((n1, n2) => n1 * n2)
                )
                .Sum();
        }
        
        private bool CheckIfMulBetweenDontsAndDos(List<(int, int)> rangeList, int mulIndex)
        {
            foreach (var (dont, doi) in rangeList)
            {
                if (mulIndex < doi && mulIndex > dont)
                    return false;
            }
            return true;
        }

        public override void Solve2(string input)
        {
            input = input.Replace("\n", "");
            var rangeRegex = new Regex("don't\\(\\).*?(do\\(\\)|$)");
            var ranges = rangeRegex.Matches(input).Select(range => (range.Index, range.Index + range.Length)).ToList();
            var multiplications = GetMatches(input);
            var filtered = multiplications
                .Where(mul => CheckIfMulBetweenDontsAndDos(ranges, mul.Index))
                .ToList();
            var result = MatchNumbersAndMultiply(filtered);
            Console.WriteLine(result);
        }
    }
}
