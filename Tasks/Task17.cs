namespace AdventOfCode2024.Tasks
{
    public class Task17 : AdventTask
    {
        public Task17()
        {
            Filename += "17.txt";
        }

        public override void Solve1(string input)
        {
            long result = 0;
            var lines = GetLinesList(input);
            var registers = new List<long>
            {
                long.Parse(lines[0].Split(" ")[2]),
                long.Parse(lines[1].Split(" ")[2]),
                long.Parse(lines[2].Split(" ")[2])
            };
            var program = lines[4].Split(" ")[1].Split(",").Select(long.Parse).ToList();
            var toPrint = DoProgram(program, registers);
            Console.WriteLine(string.Join(",", toPrint));
            Console.WriteLine(result);
        }

        public override async void Solve2(string input)
        {
            long result = 0;
            var lines = GetLinesList(input);
            var registers = new List<long>
            {
                long.Parse(lines[0].Split(" ")[2]),
                long.Parse(lines[1].Split(" ")[2]),
                long.Parse(lines[2].Split(" ")[2])
            };
            var program = lines[4].Split(" ")[1].Split(",").Select(long.Parse).ToList();
            var tasks = new List<Task<(List<long>, long)>>();
            long A = 281474893418241;
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            while (A < long.MaxValue && !token.IsCancellationRequested)
            {
                registers[0] = A;
                var t = DoProgram(program.ToList(), registers.ToList());
                if (t.Take(8).SequenceEqual(program.Take(8)))
                    Console.WriteLine($"{string.Join(",", t)} ----- {A}, {A % 8}, {Convert.ToString(A, 2)}");
                if (program.SequenceEqual(t))
                {
                    result = A;
                    break;
                }
                if (t.Count - 1 > program.Count)
                {
                    Console.WriteLine(A);
                    break;
                }

                //tasks.Add(Task<(List<long>, long)>.Run(() => DoProgram2(program.ToList(), registers.ToList(), A), token)
                //    .ContinueWith(taskResult =>
                //    {
                //        if (taskResult.IsCanceled)
                //        {
                //            return taskResult.Result;
                //        }
                //        if (program.SequenceEqual(taskResult.Result.Item1))
                //        {
                //            result = taskResult.Result.Item2;
                //            tokenSource.Cancel();
                //        }
                //       return (taskResult.Result, 0);
                //    }));
                A-= 3841;
            }
            await Task.WhenAll(tasks);
            //Console.WriteLine(string.Join(",", toPrint));
            Console.WriteLine(result);
        }

        public List<long> DoProgram(List<long> program, List<long> registers)
        {
            var toPrint = new List<long>();
            int programPointer = 0;

            while (programPointer < program.Count && programPointer >= 0)
            {
                var (opcode, operand) = (program[programPointer], program[programPointer + 1]);
                var val = DoInstruction(opcode, operand, registers);
                if (opcode == 5)
                    toPrint.Add(val);

                if (opcode == 3)
                    programPointer = (int)val;
                else
                    programPointer += 2;
            }
            return toPrint;
        }

        public async Task<(List<long>, long)> DoProgram2(List<long> program, List<long> registers, long a)
        {
            return (DoProgram(program, registers), a);
        }

        public long DoInstruction(long opcode, long operand, List<long> registers) => opcode switch
        {
            0 => Adv(opcode, operand, registers),
            1 => Bxl(opcode, operand, registers),
            2 => Bst(opcode, GetComboOperand(operand, registers), registers),
            3 => Jnz(opcode, operand, registers),
            4 => Bxc(opcode, operand, registers),
            5 => Out(opcode, operand, registers),
            6 => Bdv(opcode, operand, registers),
            7 => Cdv(opcode, operand, registers),
            _ => throw new NotImplementedException()
        };

        public long Adv(long opcode, long operand, List<long> registers)
        {
            registers[0] = (long)(registers[0] / Math.Pow(2, GetComboOperand(operand, registers)));
            return registers[0];
        }

        public long Bxl(long opcode, long operand, List<long> registers)
        {
            registers[1] = registers[1] ^ operand;
            return registers[1];
        }

        public long Bst(long opcode, long operand, List<long> registers)
        {
            registers[1] = operand % 8;
            return registers[1];
        }

        public long Jnz(long opcode, long operand, List<long> registers)
        {
            if (registers[0] == 0)
                return long.MaxValue;
            return operand;
        }

        public long Bxc(long opcode, long operand, List<long> registers)
        {
            registers[1] = registers[1] ^ registers[2];
            return registers[1];
        }

        public long Out(long opcode, long operand, List<long> registers)
        {
            return GetComboOperand(operand, registers) % 8;
        }

        public long Bdv(long opcode, long operand, List<long> registers)
        {
            registers[1] = (long)(registers[0] / Math.Pow(2, GetComboOperand(operand, registers)));
            return registers[1];
        }

        public long Cdv(long opcode, long operand, List<long> registers)
        {
            registers[2] = (long) (registers[0] / Math.Pow(2, GetComboOperand(operand, registers)));
            return registers[2];
        }

        public long GetComboOperand(long operand, List<long> registers) => operand switch
        {
            var x when (x >= 0 && x <= 3) => operand,
            4 => registers[0],
            5 => registers[1],
            6 => registers[2],
            _ => throw new NotImplementedException()
        };
    }    
}
