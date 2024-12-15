using System.Text.RegularExpressions;

namespace AdventOfCode2024.Tasks
{
    public class Task15 : AdventTask
    {
        public Task15()
        {
            Filename += "15.txt";
        }

        public override void Solve1(string input)
        {
            var map = GetMatrixList(input.Split("\r\n\r\n")[0]);
            var moves = string.Join("", GetLinesList(input.Split("\r\n\r\n")[1]));

            Position robot = new Position(0, 0);
            var walls = new HashSet<Position>(); 
            var boxes = new HashSet<Position>();
            for (var row = 0; row < map.Count; row++)
            {
                for (var col = 0; col < map[0].Count; col++)
                {
                    if (map[row][col] == '#') 
                        walls.Add(new Position(row, col));
                    if (map[row][col] == 'O')
                        boxes.Add(new Position(row, col));
                    if (map[row][col] == '@')
                        robot = new Position(row, col);
                }
            }

            foreach(var move in moves)
                robot.Move(GetMovingDirection(move), walls, boxes);

            //for (var row = 0; row < map.Count; row++)
            //{
            //    for (var col = 0; col < map[0].Count; col++)
            //    {
            //        var newPosition = new Position(row, col);
            //        if (boxes.Where(p => p.Row == newPosition.Row && p.Col == newPosition.Col).Any())
            //            Console.Write("O");
            //        else if (walls.Where(p => p.Row == newPosition.Row && p.Col == newPosition.Col).Any())
            //            Console.Write("#");
            //        else if (robot.Col == col && robot.Row == row)
            //            Console.Write("@");
            //        else
            //            Console.Write(".");
            //    }
            //    Console.WriteLine();
            //}
            Console.WriteLine(GetResult(boxes));
        }

        public override void Solve2(string input)
        {
            long result = 0;
            input = input.Replace("#", "##").Replace(".", "..").Replace("@", "@.").Replace("O", "[]");
            var map = GetMatrixList(input.Split("\r\n\r\n")[0]);
            var moves = string.Join("", GetLinesList(input.Split("\r\n\r\n")[1]));

            Position robot = new Position(0, 0);
            var walls = new HashSet<Position>();
            var boxes = new HashSet<Position>();
            for (var row = 0; row < map.Count; row++)
            {
                for (var col = 0; col < map[0].Count; col++)
                {
                    if (map[row][col] == '#')
                        walls.Add(new Position(row, col));
                    if (map[row][col] == '@')
                        robot = new Position(row, col);
                }
                var inputRow = string.Join("", map[row]);
                var boxRegex = new Regex(@"\[\]");
                foreach(Match box in boxRegex.Matches(inputRow))
                    boxes.Add(new Position(row, box.Index, true, row, box.Index + 1));
            }

            foreach (var move in moves)
            {
                robot.Move(GetMovingDirection(move), walls, boxes);
            }

            //for (var row = 0; row < map.Count; row++)
            //{
            //    for (var col = 0; col < map[0].Count; col++)
            //    {
            //        var newPosition = new Position(row, col);
            //        if (boxes.Where(p => p.Row == newPosition.Row && p.Col == newPosition.Col).Any())
            //            Console.Write("[]");
            //        else if (walls.Where(p => p.Row == newPosition.Row && p.Col == newPosition.Col).Any())
            //            Console.Write("#");
            //        else if (robot.Col == col && robot.Row == row)
            //            Console.Write("@");
            //        else
            //            Console.Write(".");
            //    }
            //    Console.WriteLine();
            //}

            Console.WriteLine(GetResult(boxes));
        }

        private int GetResult(HashSet<Position> boxes) => boxes.Sum(box => box.Row * 100 + box.Col);

        private Direction GetMovingDirection(char move) => move switch
        {
            '>' => Direction.East,
            '<' => Direction.West,
            '^' => Direction.North,
            'v' => Direction.South,
            _ => throw new NotImplementedException()
        };

        class Position
        {
            public int Row;
            public int Col;
            public int? Row2;
            public int? Col2;
            public bool IsBox;

            public Position(int row, int col, bool isBox = false, int? row2 = null, int? col2 = null)
            {
                Row = row;
                Col = col;
                Row2 = row2;
                Col2 = col2;
                IsBox = isBox;
            }

            private bool CanMove(Direction direction, HashSet<Position> walls, HashSet<Position> boxes, (int, int) pos)
            {
                if (IsWallHit(direction, walls, (Row, Col)) || 
                    (IsBox && IsWallHit(direction, walls, (Row2!.Value, Col2!.Value))))
                    return false;

                var (newRow, newCol) = MakeMove(pos, direction);
                var newPosition = new Position(newRow, newCol);
                var filteredBoxes = boxes
                        .Where(p => (p.Row == newPosition.Row && p.Col == newPosition.Col) ||
                    (p.IsBox && p.Row2!.Value == newPosition.Row && p.Col2!.Value == newPosition.Col));
                if (filteredBoxes.Any() && filteredBoxes.First() != this)
                    return filteredBoxes
                        .First()
                        .CanMove(direction, walls, boxes);
                return true;
            }

            public bool CanMove(Direction direction, HashSet<Position> walls, HashSet<Position> boxes)
            {
                var canMove = CanMove(direction, walls, boxes, (Row, Col));
                return IsBox ? canMove && CanMove(direction, walls, boxes, (Row2!.Value, Col2!.Value)) : canMove;
            }

            private bool IsWallHit(Direction direction, HashSet<Position> walls, (int, int) pos)
            {
                var (newRow, newCol) = MakeMove(pos, direction);
                return walls.Contains(new Position(newRow, newCol));
            }

            private (int, int) Move(Direction direction, HashSet<Position> walls, HashSet<Position> boxes, (int, int) pos)
            {
                var (newRow, newCol) = MakeMove(pos, direction);
                var newPosition = new Position(newRow, newCol);
                var filteredBoxes = boxes
                    .Where(p => (p.Row == newPosition.Row && p.Col == newPosition.Col) ||
                    (p.IsBox && p.Row2!.Value == newPosition.Row && p.Col2!.Value == newPosition.Col));
                if (filteredBoxes.Any() && filteredBoxes.First() != this)
                    filteredBoxes.First()
                        .Move(direction, walls, boxes);
                return (newRow, newCol);
            }

            public void Move(Direction direction, HashSet<Position> walls, HashSet<Position> boxes)
            {
                if (CanMove(direction, walls, boxes))
                {
                    (Row, Col) = Move(direction, walls, boxes, (Row, Col));
                    if (IsBox)
                    {
                        (Row2, Col2) = Move(direction, walls, boxes, (Row2!.Value, Col2!.Value));
                    }
                }
            }

            public override bool Equals(object? obj)
            {
                return obj.GetHashCode() == this.GetHashCode();
            }

            private int GetUniqueInt(int n1, int n2) => (n1 + n2) * (n1 + n2 + 1) / 2 + n2;

            public override int GetHashCode() => IsBox ? GetUniqueInt(GetUniqueInt(Row, Col), GetUniqueInt(Row2!.Value, Col2!.Value)) : GetUniqueInt(Row, Col);
        }
    }    
}
