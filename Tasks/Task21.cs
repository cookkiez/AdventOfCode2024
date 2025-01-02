using System;
using System.Collections;
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
            var moveRobot1 = new KeyPresser<Direction>
            {
                Position = (0, 2),
                KeyPadDict = DirectionKeypad,
                Name = "R1",
                IsPin = false
            };
            var moveRobot2 = new KeyPresser<Direction>
            {
                Position = (0, 2),
                KeyPadDict = DirectionKeypad,
                Name = "R2",
                IsPin = false
            };

            var robots = new List<KeyPresser<Direction>> { moveRobot1, moveRobot2 };

            SolveBoth(input, robots);
        }

        public override void Solve2(string input)
        {
            var robots = new List<KeyPresser<Direction>>();
            for (var i = 0; i < 25; i++)
                robots.Add(new KeyPresser<Direction>
                {
                    Position = (0, 2),
                    KeyPadDict = DirectionKeypad,
                    Name = $"R{i}",
                    IsPin = false
                });

            SolveBoth(input, robots);
        }

        private void SolveBoth(string input, List<KeyPresser<Direction>> robots)
        {
            long result = 0;
            var pinRobot = new KeyPresser<char>
            {
                Position = (3, 2),
                KeyPadDict = PinKeypad,
                Name = "PIN",
                IsPin = true
            };
            var pins = GetLinesList(input);
            foreach (var pin in pins)
            {
                var steps = pin.ToCharArray();
                //Console.WriteLine(pin);
                long pinCost = 0;
                //result += steps.Length;
                foreach (var action in steps)
                {
                    long bestCost = long.MaxValue;
                    var (moves, pos) = pinRobot.GenerateMovesForActionAndMovePresser(action);
                    var moveRobotActions = GenerateActionsForMoves(moves, pos, true);
                    var validPermutations = Permutate(moveRobotActions, moveRobotActions.Count).Where(seq => CheckIfSequenceIsValid(pos, seq, pinRobot.IsPin) == -1).ToList();
                    foreach (var permutation in validPermutations)
                    {
                        permutation.Add(Direction.Press);
                        long cost = 0;
                        foreach (var a in permutation)
                            cost += GetBestPressSequence(robots, robots.Count - 1, a, new Dictionary<(string, Direction, (int, int)), long>());

                        if (cost < bestCost)
                            bestCost = cost;
                    }
                    pinCost += bestCost;
                }
                result += pinCost * int.Parse(pin.Substring(0, 3));
                //Console.WriteLine(pinCost);
                //Console.WriteLine("#######");
            }
            Console.WriteLine(result);
        }

        private long GetBestPressSequence(List<KeyPresser<Direction>> robots, int depth, Direction action, Dictionary<(string, Direction, (int, int)), long> visited)
        {
            var robot = robots[depth];
            var (moves, pos) = robot.GenerateMovesForActionAndMovePresser(action);
            var moveRobotActions = GenerateActionsForMoves(moves, pos, true);
            //Console.WriteLine(robot.Name + ", " + pos + ": " + string.Join(", ", moveRobotActions));
            if (depth == 0)
                return (long) moveRobotActions.Count + 1;
            var visitedKey = (robot.Name, action, pos);
            if (visited.ContainsKey(visitedKey))
                return visited[visitedKey];

            long bestSequence = long.MaxValue;
            foreach(var permutation in Permutate(moveRobotActions, moveRobotActions.Count).Where(seq => CheckIfSequenceIsValid(pos, seq, robot.IsPin) == -1))
            {
                permutation.Add(Direction.Press);
                long currSequence = 0;
                foreach (var moveAction in permutation)
                    currSequence += GetBestPressSequence(robots, depth - 1, moveAction, visited);

                if (currSequence < bestSequence)
                    bestSequence = currSequence;
            }
            if (moveRobotActions.Count == 0)
            {
                bestSequence = GetBestPressSequence(robots, depth - 1, Direction.Press, visited);
            }
            visited[visitedKey] = bestSequence;
            return bestSequence;
        }

        private void RotateRight(List<Direction> sequence, int count)
        {
            Direction tmp = sequence[count - 1];
            sequence.RemoveAt(count - 1);
            sequence.Insert(0, tmp);
        }

        private IEnumerable<List<Direction>> Permutate(List<Direction> sequence, int count)
        {
            if (count == 1) yield return sequence;
            else
            {
                for (int i = 0; i < count; i++)
                {
                    RotateRight(sequence, count);
                    yield return sequence.ToList();
                }
            }
        }

        private int CheckIfSequenceIsValid((int Row, int Col) position, List<Direction> sequence, bool isPin)
        {
            foreach (var move in sequence)
            {
                position = MakeMove(position, move);
                if (isPin && position == (3, 0))
                {
                    return sequence.IndexOf(move);
                }
                else if (!isPin && position == (0, 0))
                {
                    return sequence.IndexOf(move);
                }
            }
            return -1;
        }

        private List<Direction> GenerateActionsForMoves((int dRow, int dCol) moves, (int Row, int Col) startPosition, bool isPin)
        {
            var list = new List<Direction>();
            AddToList(list, moves.dRow, true);
            AddToList(list, moves.dCol, false);
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
            public bool IsPin { get; set; }
            
            private (int Row, int Col) GetMovesForDesiredPosition((int Row, int Col) desiredPosition) 
                => (desiredPosition.Row - Position.Row, desiredPosition.Col - Position.Col);

            private (int Row, int Col) GetDesiredPosition(T action)
                => KeyPadDict[action];

            public ((int dRow, int dCol), (int Row, int Col)) GenerateMovesForActionAndMovePresser(T action)
            {
                var desired = GetDesiredPosition(action);
                var moves = GetMovesForDesiredPosition(desired);
                var startPosition = Position;
                Position = desired;
                return (moves, startPosition);
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
            East,
            South, 
            North,
            West,
            Press
        }

        protected new (int Row, int Col) MakeMove((int Row, int Col) block, Direction movingDirection) =>
            movingDirection switch
            {
                Direction.West => (block.Row, block.Col + 1),
                Direction.East => (block.Row, block.Col - 1),
                Direction.South => (block.Row + 1, block.Col),
                Direction.North => (block.Row - 1, block.Col),
                _ => throw new Exception()
            };
    }    
}
