namespace AdventOfCode2024.Tasks
{
    public class Task7 : AdventTask
    {
        public Task7()
        {
            Filename += "7.txt";
        }

        // Helper function for operations that need to be performed
        private long Mutiply(long a, long b) => a * b;

        private long Sum(long a, long b) => a + b;

        private long Concatenate(long a, long b) => long.Parse(a.ToString() + b.ToString());


        private long TryCombinations(long result, Stack<long> numbers, Func<long, long, long>[] operations)
        {
            // Check the stack if we have reached the end and gotten the result. 
            // if we have reached the end and the result is not correct, then return 0
            if (numbers.Count == 1 && numbers.Peek() == result)
                return result;
            if (numbers.Peek() > result || numbers.Count == 0 || numbers.Count == 1)
                return 0;

            // Get current result (at start first number, later partial result) and next number and perform operations on them
            var currentResult = numbers.Pop();
            var nextNumber = numbers.Pop();
            foreach (var operation in operations)
            {
                numbers.Push(operation(currentResult, nextNumber));
                var res = TryCombinations(result, numbers, operations);
                if (res > 0)
                    return res;
                // Pop the intermediate result that was from the done operation
                numbers.Pop();
            }
            // If the correct combination has not been found for these two numbers then push them back, 
            // we will need them to try another operation to get the intermediate result.
            numbers.Push(nextNumber);
            numbers.Push(currentResult); 
            return 0;
        }

        private long SolveBoth(string input, Func<long, long, long>[] operations)
        {
            // Go over each line and try combinations of operations to find the right combination
            // using a stack here so I can move the numbers on and off it dynamicaly.
            long result = 0;
            var lines = GetLinesList(input);
            foreach (var line in lines)
            {
                var splitByColon = line.Split(":");
                var testNumber = long.Parse(splitByColon[0]);
                var numbers = splitByColon[1].Trim().Split(" ").Select(long.Parse).Reverse().ToList();
                result += TryCombinations(testNumber, new Stack<long>(numbers), operations);
            }
            return result;
        }
        
        public override void Solve1(string input)
        {
            Console.WriteLine(SolveBoth(input, [Mutiply, Sum]));
        }
        public override void Solve2(string input)
        {
            Console.WriteLine(SolveBoth(input, [Mutiply, Sum, Concatenate]));
        }
    }
}
