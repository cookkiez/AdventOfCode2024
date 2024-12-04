namespace AdventOfCode2024.Tasks
{
    public class Task4 : AdventTask
    {
        public Task4()
        {
            Filename += "4.txt";
        }
        public override void Solve1(string input)
        {
            var result = SolveBoth(input, 'X', (row, col, matrix) =>
            {
                var wordToFind = "XMAS";
                var words = new List<string>
                    {
                        GetWord(row, col, matrix, 1, 0),
                        GetWord(row, col, matrix, -1, 0),
                        GetWord(row, col, matrix, 0, 1),
                        GetWord(row, col, matrix, 0, -1),
                        GetWord(row, col, matrix, 1, 1),
                        GetWord(row, col, matrix, 1, -1),
                        GetWord(row, col, matrix, -1, 1),
                        GetWord(row, col, matrix, -1, -1),
                    };
                return words.Where(word => word == wordToFind).Count();
            });
            Console.WriteLine(result);
        }

        private int SolveBoth(string input, char charToFind, Func<int, int, char[][], int> checkForResult)
        {
            var result = 0;
            var matrix = GetMatrixArray(input);
            for (var row = 0; row < matrix.Length; row++)
            {
                for (var col = 0; col < matrix[row].Length; col++)
                {
                    if (matrix[row][col] == charToFind)
                    {
                        result += checkForResult(row, col, matrix);
                    }
                }
            }
            return result;
        }

        private string GetWord(int row, int col, char[][] matrix, int colPlus, int rowPlus, int wordLength = 3)
        {
            var maxRow = row + wordLength * rowPlus;
            var maxCol = col + wordLength * colPlus;
            if (CheckIfIndexOutsideMatrix<char>(matrix, maxRow, maxCol))
                return "";
            var charList = new List<char>();
            for (var i = 0; i <= wordLength; i++)
            {
                charList.Add(matrix[row + i * rowPlus][col + i * colPlus]);
            }
            return string.Join("", charList);
        }
        
        private bool CheckIfMas(string wordAm, string wordAS)
        {
            return wordAm == "AM" && wordAS == "AS";
        }

        public override void Solve2(string input)
        {
            var result = SolveBoth(input, 'A', (row, col, matrix) =>
            {
                var diagUpLeft = GetWord(row, col, matrix, -1, -1, 1);
                var diagDownRight = GetWord(row, col, matrix, 1, 1, 1);
                var diagDownLeft = GetWord(row, col, matrix, 1, -1, 1);
                var diagUpRight = GetWord(row, col, matrix, -1, 1, 1);
                if ((CheckIfMas(diagDownRight, diagUpLeft) || CheckIfMas(diagUpLeft, diagDownRight)) &&
                    (CheckIfMas(diagDownLeft, diagUpRight) || CheckIfMas(diagUpRight, diagDownLeft)))
                    return 1;
                return 0;
            });
            Console.WriteLine(result);
        }
    }
}
