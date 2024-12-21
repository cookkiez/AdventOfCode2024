using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;

namespace AdventOfCode2024.Tasks
{
    public class Task21: AdventTask
    {
        public Task21()
        {
            Filename += "21.txt";
        }

        public override void Solve1(string input)
        {
            long result = 0;
            //MakeMove((0, 0), (AdventTask.Direction) Direction.West);
            var pinRobot = new KeyPresser<char>
            { 
                Position = (3, 2),
                KeyPadDict = PinKeypad,
                Name = "PIN"
            };
            var moveRobot1 = new KeyPresser<Direction>
            {
                Position = (0, 2),
                KeyPadDict = DirectionKeypad,
                Name = "R1"
            };
            var moveRobot2 = new KeyPresser<Direction>
            {
                Position = (0, 2),
                KeyPadDict = DirectionKeypad,
                Name = "R2"
            };
            
            var pins = GetLinesList(input);
            foreach (var pin in pins)
            {
                var steps = pin.ToCharArray();
                Console.WriteLine(pin);
                var cost = 0;
                //result += steps.Length;
                foreach (var action in steps)
                {
                    var moves = pinRobot.GenerateMovesForActionAndMovePresser(action);
                    var moveRobotActions = GenerateActionsForMoves(moves);
                    Console.Write("R1:");
                    Console.WriteLine(string.Join(", ", moveRobotActions));
                    //result += moveRobotActions.Count; 
                    foreach (var moveAction in moveRobotActions)
                    {
                        var moves1 = moveRobot1.GenerateMovesForActionAndMovePresser(moveAction);
                        var moveRobotActions2 = GenerateActionsForMoves(moves1);
                        Console.Write("R2:");
                        Console.WriteLine(string.Join(", ", moveRobotActions2));
                        //result += moveRobotActions2.Count;
                        foreach (var moveAction2 in moveRobotActions2)
                        {
                            var myMoves = moveRobot2.GenerateMovesForActionAndMovePresser(moveAction2);
                            var myActions = GenerateActionsForMoves(myMoves);
                            //result += myActions.Count;
                            cost += myActions.Count;
                            Console.Write("ME:");
                            Console.WriteLine(string.Join(", ", myActions));
                            //Console.WriteLine(myActions.Count);
                            var a = 0;
                        }
                    }
                }
                result += cost * int.Parse(pin.Substring(0, 3));
                Console.WriteLine(cost);
                Console.WriteLine("#######");
            }
            Console.WriteLine(result);
        }

        public override void Solve2(string input)
        {
            long result = 0;

            Console.WriteLine(result);
        }

        private List<Direction> GenerateActionsForMoves((int dRow, int dCol) moves)
        {
            var list = new List<Direction>();
            AddToList(list, moves.dRow, true);
            AddToList(list, moves.dCol, false);
            list.Add(Direction.Press);
            return list.ToList();
        }

        private void AddToList(List<Direction> list, int d, bool row)
        {
            while (Math.Abs(d) > 0)
            {
                list.Add(row ? GetRowActions(d) : GetColActions(d));
                d = d > 0 ? d - 1 : d + 1;
            }
        }

        private Direction GetRowActions(int drow) => drow > 0 ? Direction.South : Direction.North;
        private Direction GetColActions(int dcol) => dcol > 0 ? Direction.West : Direction.East;

        class KeyPresser<T>
        {
            public (int Row, int Col) Position { get; set; }
            public Dictionary<T, (int, int)> KeyPadDict { get; set; }
            public string Name { get; set; }
            
            private (int Row, int Col) GetMovesForDesiredPosition((int Row, int Col) desiredPosition) 
                => (desiredPosition.Row - Position.Row, desiredPosition.Col - Position.Col);

            private (int Row, int Col) GetDesiredPosition(T action)
                => KeyPadDict[action];

            public (int dRow, int dCol) GenerateMovesForActionAndMovePresser(T action)
            {
                var desired = GetDesiredPosition(action);
                var moves = GetMovesForDesiredPosition(desired);
                Position = desired;
                return moves;
            }
        }

        Dictionary<char, (int Row, int Col)> PinKeypad = new Dictionary<char, (int Row, int Col)>
        {
            { '1', (2, 0) },
            { '2', (2, 1) },
            { '3', (2, 2) },
            { '4', (1, 0) },
            { '5', (1, 1) },
            { '6', (1, 2) },
            { '7', (0, 0) },
            { '8', (0, 1) },
            { '9', (0, 2) },
            { '0', (3, 1) },
            { 'A', (3, 2) },
        };

        Dictionary<Direction, (int Row, int Col)> DirectionKeypad = new Dictionary<Direction, (int Row, int Col)>
        {
            { Direction.North, (0, 1) },
            { Direction.West, (1, 2) },
            { Direction.South, (1, 1) },
            { Direction.East, (1, 0) },
            { Direction.Press, (0, 2) },
        };

        protected new enum Direction
        {
            North, West, South, East, Press
        }
    }    
}
