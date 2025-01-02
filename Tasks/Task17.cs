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

        /*
            PseudoCode
        while (A > 0)
            B = A % 8;
            B = B xor 5;
            C = A / (2^B);
            B = B xor C;
            A = A / (2^3);
            B = B xor 6;
            out += B % 8;               
         */

        public override void Solve2(string input)
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
            var toPrint = new List<long>();
            //while (registers[0] > 0)
            //{
            //    Bst(2, GetComboOperand(4, registers), registers);
            //    Bxl(1, 5, registers);
            //    Cdv(7, 5, registers);
            //    Bxc(4, 5, registers);
            //    Adv(0, 3, registers);
            //    Bxl(1, 6, registers);
            //    toPrint.Add(Out(5, 5, registers));
            //    Jnz(3, 0, registers);
            //}


            Console.Write(Solve2(program, program, 0));
            Console.WriteLine(string.Join(",", toPrint));
        }

        private long Solve2(List<long> program, List<long> output, long A)
        {
            if (output.Count == 0)
                return A;
            for(long a = 0; a < 1024; a++)
            {
                if (a >> 3 == (A & 127))
                {
                    var registers = new List<long> { a, 0, 0 };
                    var tempOut = DoProgram(program, registers);
                    if (tempOut.First() == output.Last())
                    {
                        var nextOutput = output.ToList();
                        nextOutput.RemoveAt(nextOutput.Count - 1);
                        var result = Solve2(program, nextOutput, (A << 3) | (a % 8));
                        if (result != long.MaxValue)
                            return result;
                    }
                }
            }
            return long.MaxValue;
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
                //Console.WriteLine(string.Join(",", registers));
            }
            return toPrint;
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
