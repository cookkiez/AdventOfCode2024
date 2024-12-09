namespace AdventOfCode2024.Tasks
{
    public class Task9 : AdventTask
    {
        public Task9()
        {
            Filename += "9.txt";
        }

        public override void Solve1(string input)
        {
            long result = 0;
            var nums = input.Trim().ToCharArray().ToList().Select(char.GetNumericValue).ToList();
            var memory = new List<int>();
            var freeSpaceIndexes = new List<int>();
            var numIndexes = new List<int>();
            var numIndex = 0;
            for(var i = 0; i < nums.Count; i += 2)
            {
                var size = (int)nums[i];
                
                numIndexes.Add(memory.Count + size - 1);
                while (size > 0)
                {
                    memory.Add(numIndex);
                    size--;
                }
                if (i + 1 < nums.Count)
                {
                    var freeSpace = nums[i + 1];
                    while (freeSpace > 0)
                    {
                        memory.Add(-1);
                        freeSpaceIndexes.Add(memory.Count - 1);
                        freeSpace--;
                    }
                }
                
                numIndex++;
            }
            var freeSpaceStack = new Stack<int>(freeSpaceIndexes.OrderDescending());
            numIndexes = numIndexes.OrderDescending().ToList();
            foreach (var numI in numIndexes) 
            {
                var i = numI;
                while (i >= 0 && memory[i] != -1 && freeSpaceStack.Any())
                {
                    var num = memory[i];
                    var fi = freeSpaceStack.Pop();
                    if (fi > i)
                        break;
                    memory[fi] = num;
                    memory[i] = -1;
                    i--;
                }
                if (!freeSpaceStack.Any())
                    break;
            }
            for(var fileIndex = 0; fileIndex < memory.Count; fileIndex++)
            { 
                var file = memory[fileIndex];
                if (file < 0)
                    break;
                result += fileIndex * file;
            }
            //Console.WriteLine(string.Join("", memory));
            Console.WriteLine(result);
        }

        public override void Solve2(string input)
        {
            long result = 0;
            var nums = input.Trim().ToCharArray().ToList().Select(char.GetNumericValue).ToList();
            var memory = new List<int>();
            var freeSpaceIndexes = new List<(int, int)>();
            var numIndexes = new List<(int, int)>();
            var numIndex = 0;
            for (var i = 0; i < nums.Count; i += 2)
            {
                var size = (int)nums[i];

                numIndexes.Add((memory.Count + size - 1, size));
                while (size > 0)
                {
                    memory.Add(numIndex);
                    size--;
                }
                if (i + 1 < nums.Count)
                {
                    var freeSpace = (int)nums[i + 1];
                    freeSpaceIndexes.Add((memory.Count, freeSpace));
                    while (freeSpace > 0)
                    {
                        memory.Add(-1);
                        freeSpace--;
                    }
                }

                numIndex++;
            }
            var freeSpaceStack = new Stack<(int, int)>(freeSpaceIndexes.OrderByDescending(fi => fi.Item1));
            numIndexes = numIndexes.OrderDescending().ToList();
            foreach (var (numI, size) in numIndexes)
            {
                var i = numI;

                var num = memory[i];
                var fi = freeSpaceStack.Pop();
                var tempStack = new Stack<(int, int)>();
                while (freeSpaceStack.Any() && fi.Item2 < size)
                {
                    tempStack.Push(fi);
                    fi = freeSpaceStack.Pop();
                    if (fi.Item1 > i)
                    {
                        tempStack.Push(fi);
                        break;
                    }
                }
                while (tempStack.Any())
                    freeSpaceStack.Push(tempStack.Pop());
                if (fi.Item1 > i)
                {
                    continue;
                }

                var tempSize = size;
                while (tempSize > 0)
                {
                    memory[fi.Item1 + tempSize - 1] = num;
                    memory[i - tempSize + 1] = -1;
                    tempSize--;
                }
                    
                var newFreeSize = fi.Item2 - size;
                if (newFreeSize > 0)
                {
                    freeSpaceStack.Push((fi.Item1 + size, newFreeSize));
                }
                freeSpaceStack = new Stack<(int, int)>(freeSpaceStack.OrderByDescending(fi => fi.Item1));
                    i--;
                if (!freeSpaceStack.Any())
                    break;
            }
            for (var fileIndex = 0; fileIndex < memory.Count; fileIndex++)
            {
                var file = memory[fileIndex];
                if (file < 0)
                    continue;
                result += fileIndex * file;
            }
            Console.WriteLine(result);
        }
    }
}
