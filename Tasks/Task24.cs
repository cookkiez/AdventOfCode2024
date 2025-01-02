using System.Text.RegularExpressions;

namespace AdventOfCode2024.Tasks
{
    public class Task24: AdventTask
    {
        public Task24()
        {
            Filename += "24.txt";
        }

        public override void Solve1(string input)
        {
            long result = 0;
            var wires = GetLinesList(input.Split("\r\n\r\n")[0]);
            var outputs = new Dictionary<string, bool>();
            foreach (var wire in wires)
            {
                var (wireName, wireValue) = (wire.Split(": ")[0], wire.Split(": ")[1].Equals("1"));
                outputs.Add(wireName, wireValue);
            }

            var operations = new Queue<string>(GetLinesList(input.Split("\r\n\r\n")[1]));

            while(operations.TryDequeue(out var operation))
            {
                var (operationIn, operationOut) = (operation.Split(" -> ")[0].Split(" "), operation.Split(" -> ")[1]);
                var (operand1, op, operand2) = (operationIn[0], operationIn[1], operationIn[2]);
                if (!outputs.ContainsKey(operand1) || !outputs.ContainsKey(operand2))
                {
                    operations.Enqueue(operation);
                    continue;
                }
                outputs[operationOut] = GetValueOfOperation(op, outputs[operand1], outputs[operand2]);
            }

            var outValues = outputs.Where(kvp => kvp.Key.StartsWith("z")).OrderByDescending(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            result = outValues.Aggregate<bool, long>(0, (sum, val) => (sum * 2) + (val ? 1 : 0));
            Console.WriteLine(result);
        }

        public override void Solve2(string input)
        {
            long result = 0;
            var wires = GetLinesList(input.Split("\r\n\r\n")[0]);
            var outputs = new Dictionary<string, bool>();
            foreach (var wire in wires)
            {
                var (wireName, wireValue) = (wire.Split(": ")[0], wire.Split(": ")[1].Equals("1"));
                outputs.Add(wireName, wireValue);
            }
            var operations = new Queue<string>(GetLinesList(input.Split("\r\n\r\n")[1]));
            var orderedOperations = new List<(string operand1, string operation, string operand2, string output)>();
            while (operations.TryDequeue(out var operation))
            {
                var (operationIn, operationOut) = (operation.Split(" -> ")[0].Split(" "), operation.Split(" -> ")[1]);
                var (operand1, op, operand2) = (operationIn[0], operationIn[1], operationIn[2]);
                if (!outputs.ContainsKey(operand1) || !outputs.ContainsKey(operand2))
                {
                    operations.Enqueue(operation);
                    continue;
                }
                outputs[operationOut] = GetValueOfOperation(op, outputs[operand1], outputs[operand2]);
                orderedOperations.Add((operand1, op, operand2, operationOut));
            }

            var zRegex = new Regex("z\\d+");
            var maxZ = zRegex.Matches(input).Select(m => m.Value).Max();
            var swaps = new List<string>();
            foreach (var (operand1, operation, operand2, output) in orderedOperations)
            {
                if (operation != "XOR" && output.StartsWith("z") && output != maxZ)
                    swaps.Add(output);
                else if (operation == "XOR" && !output.StartsWith("z") && !(OperatorStartsWithXY(operand1) && OperatorStartsWithXY(operand2)))
                    swaps.Add(output);
                else if (operation == "AND" && operand1 != "x00" && operand2 != "x00" && CheckIfOutputExistsAsInputInGate(orderedOperations, output, (o => o != "OR")))
                    swaps.Add(output);
                else if (operation == "XOR" && CheckIfOutputExistsAsInputInGate(orderedOperations, output, (o => o == "OR")))
                    swaps.Add(output);
            }
           
            //var outValues = outputs.Where(kvp => kvp.Key.StartsWith("z")).OrderByDescending(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            //var xs = outputs.Where(kvp => kvp.Key.StartsWith("x")).OrderByDescending(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            //var ys = outputs.Where(kvp => kvp.Key.StartsWith("y")).OrderByDescending(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            //Console.WriteLine(" " + string.Join("", xs.Select(x => x ? 1 : 0)));
            //Console.WriteLine(" " + string.Join("", ys.Select(x => x ? 1 : 0)));
            //Console.WriteLine(string.Join("", outValues.Select(x => x ? 1 : 0)));
            //result = outValues.Aggregate<bool, long>(0, (sum, val) => (sum * 2) + (val ? 1 : 0));
            Console.WriteLine(string.Join(",", swaps.Order()));
        }

        private bool CheckIfOutputExistsAsInputInGate(List<(string operand1, string operation, string operand2, string output)> operations, 
            string output, Func<string, bool> gateCondition)
        {
            foreach (var (operand1, operation, operand2, _) in operations)
            {
                if (gateCondition(operation) && (operand1 == output || operand2 == output))
                    return true;
            }
            return false;
        }

        private bool OperatorStartsWithXY(string op) => op.StartsWith("x") || op.StartsWith("y");

        private bool GetValueOfOperation(string operation, bool operand1, bool operand2) => operation switch
        {
            "AND" => operand1 && operand2,
            "OR" => operand1 || operand2,
            "XOR" => operand1 ^ operand2,
            _ => throw new NotImplementedException()
        }; 
    }
}
