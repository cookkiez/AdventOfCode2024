using System.Runtime.CompilerServices;

namespace AdventOfCode2024.Tasks
{
    public class Task2 : AdventTask
    {
        public Task2()
        {
            Filename += "2.txt";
        }
        public override void Solve1(string input)
        {
            var lines = GetLinesList(input);
            var safe = 0;
            foreach (var line in lines)
            {
                var splittedLine = line.Split(" ").Select(n => int.Parse(n)).ToArray();
                var increasing = splittedLine[1] > splittedLine[0];
                safe += CheckIfArraySafe(1, splittedLine, increasing);
            }
            Console.WriteLine(safe);
        }
        
        private int CheckIfArraySafe(int numIndex, int[] numArray, bool increasing, bool dampener = false, bool hasDampenedAlready = false)
        {
            if (numIndex == numArray.Count())
                return 1;
            var currentNum = numArray[numIndex];
            var previousNum = numArray[numIndex - 1];
            if ((increasing && CheckIfLevelsSafe(currentNum, previousNum)) ||
                (!increasing && CheckIfLevelsSafe(previousNum, currentNum)))
                return CheckIfArraySafe(numIndex + 1, numArray, increasing, dampener, hasDampenedAlready);
            if (dampener && !hasDampenedAlready)
                return CheckIfArraySafe(numIndex + 1, numArray, dampener, true);
            return 0;
        }

        private bool CheckIfLevelsSafe(int largerNum, int smallerNum)
        {
            return largerNum >= smallerNum && largerNum - smallerNum >= 1 && largerNum - smallerNum <= 3;
        }

        public override void Solve2(string input)
        {
            var lines = GetLinesList(input);
            var safe = 0;
            foreach (var line in lines)
            {
                var splittedLine = line.Split(" ").Select(n => int.Parse(n)).ToList();
                var increasing = splittedLine[1] > splittedLine[0];

                var currentSafe = CheckIfArraySafe(1, splittedLine.ToArray(), increasing);
                var removeAt = 0;
                while (currentSafe == 0 && removeAt < splittedLine.Count)
                {
                    var tempList = splittedLine.ToList();
                    tempList.RemoveAt(removeAt);
                    increasing = tempList[1] > tempList[0];
                    currentSafe = CheckIfArraySafe(1, tempList.ToArray(), increasing);
                    removeAt++;
                }
                safe += currentSafe;
            }
            Console.WriteLine(safe);
        }
    }
}
