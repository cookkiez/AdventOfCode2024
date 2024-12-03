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
                var splittedLine = line.Split(" ").Select(int.Parse).ToList();
                var increasing = splittedLine[1] > splittedLine[0];
                safe += CheckIfArraySafe(1, splittedLine, increasing);
            }
            Console.WriteLine(safe);
        }
        
        private int CheckIfArraySafe(int numIndex, List<int> numArray, bool increasing)
        {
            if (numIndex == numArray.Count)
                return 1;
            var currentNum = numArray[numIndex];
            var previousNum = numArray[numIndex - 1];
            if ((increasing && CheckIfLevelsSafe(currentNum, previousNum)) ||
                (!increasing && CheckIfLevelsSafe(previousNum, currentNum)))
                return CheckIfArraySafe(numIndex + 1, numArray, increasing);
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
                var splittedLine = line.Split(" ").Select(int.Parse).ToList();
                var increasing = splittedLine[1] > splittedLine[0];
                var currentSafe = CheckIfArraySafe(1, splittedLine, increasing);
                var removeAt = 0;
                while (currentSafe == 0 && removeAt < splittedLine.Count)
                {
                    var tempList = splittedLine.ToList();
                    tempList.RemoveAt(removeAt);
                    increasing = tempList[1] > tempList[0];
                    currentSafe = CheckIfArraySafe(1, tempList, increasing);
                    removeAt++;
                }
                safe += currentSafe;
            }
            Console.WriteLine(safe);
        }
    }
}
