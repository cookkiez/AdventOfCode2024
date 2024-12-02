using System.Runtime.CompilerServices;

namespace AdventOfCode2024.Tasks
{
    public class Task1 : AdventTask
    {
        public Task1()
        {
            Filename += "1.txt";
        }
        public override void Solve1(string input)
        {
            //var splitted = input.Split("\n").ToList();
            var nums1 = new List<int>();
            var nums2 = new List<int>();
            var lines = GetLinesList(input);
            foreach (var line in lines)
            {
                var splittedLine = line.Split("  ").Select(n => int.Parse(n)).ToArray();
                nums1.Add(splittedLine[0]);
                nums2.Add(splittedLine[1]);
            }
            nums1 = nums1.Order().ToList();
            nums2 = nums2.Order().ToList();
            var zipped = nums1.Zip(nums2);
            var distance = 0;
            foreach(var line in zipped)
            {
                distance += Math.Abs(Math.Abs(line.First) - Math.Abs(line.Second));
            }
            Console.WriteLine(distance);
        }

        private void CheckAndAddToDictionary(Dictionary<int, int> dict, int num)
        {
            if (!dict.ContainsKey(num))
                dict.Add(num, 0);
            dict[num]++;
        }

        public override void Solve2(string input)
        {
            var nums1 = new List<int>();
            var nums2 = new Dictionary<int, int>();
            var lines = GetLinesList(input);
            foreach (var line in lines)
            {
                var splittedLine = line.Split("  ").Select(n => int.Parse(n)).ToArray();

                nums1.Add(splittedLine[0]);
                CheckAndAddToDictionary(nums2, splittedLine[1]);
            }

            var distance = 0;
            foreach(var key in nums1)
            {
                if (nums2.ContainsKey(key))
                    distance += (key * nums2[key]);
            }
            Console.WriteLine(distance);
        }
    }
}
