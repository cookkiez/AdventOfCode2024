using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Tasks
{
    public class Task14 : AdventTask
    {
        public Task14()
        {
            Filename += "14.txt";
        }

        public override void Solve1(string input)
        {
            long result = 1;
            var maxBlock = new Block(7, 11);
            var pRegex = new Regex("p=(\\d+),(\\d+)");
            var vRegex = new Regex("v=([-]*\\d+),([-]*\\d+)");
            var lines = GetLinesList(input);
            var robots = new List<Robot>();
            foreach (var line in lines)
            {
                var pos = pRegex.Match(line).Groups.Values.Skip(1).ToList().Select(g => int.Parse(g.Value)).ToList();
                var vel = vRegex.Match(line).Groups.Values.Skip(1).ToList().Select(g => int.Parse(g.Value)).ToList();
                robots.Add(new Robot
                {
                    Position = new Block(pos[1], pos[0]),  
                    Velocity = new Block(vel[1], vel[0])
                });
            }

            foreach (var robot in robots)
            {
                robot.Position.Plus(robot.Velocity.Multiply(100)).Modulus(maxBlock);

                robot.Position.Abs();
            }

            var Q1 = (0, maxBlock.Col / 2 - 1, 0, maxBlock.Row / 2 - 1);
            var Q2 = (maxBlock.Col / 2 + 1, (maxBlock.Col - 1), 0, maxBlock.Row / 2 - 1);
            var Q3 = (0, maxBlock.Col / 2 - 1, maxBlock.Row / 2 + 1, (maxBlock.Row - 1));
            var Q4 = (maxBlock.Col / 2 + 1, (maxBlock.Col - 1), maxBlock.Row / 2 + 1, (maxBlock.Row - 1));
            var quadrants = new List<(int, int, int, int)> { Q1, Q2, Q3, Q4 };
            foreach(var quad in quadrants)
            {
                var robotsInQuadrant = 0;
                foreach(var robot in robots)
                {
                    if (robot.CheckQuadrant(quad))
                        robotsInQuadrant++;
                }
                result *= robotsInQuadrant;
            } 
            Console.WriteLine(result);
        }

        public override void Solve2(string input)
        {
            long result = 0;
            Console.WriteLine(result);
        }
    }

    class Robot
    {
        public Block Position { get; set; }
        public Block Velocity { get; set; }

        public bool CheckQuadrant((int a, int b, int c, int d) quadrant) =>
            (quadrant.a <= Position.Col && Position.Col <= quadrant.b) && (quadrant.c <= Position.Row && Position.Row < quadrant.d);
    }

    class Block
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public Block(int row, int col) => (Row, Col) = (row, col);
        public Block Plus(Block y)
        {
            this.Row += y.Row;
            this.Col += y.Col;
            return this;
        }

        public Block Multiply(int mul) => new Block(this.Row * mul, this.Col * mul);
        public void Abs() => (this.Row, this.Col) = (Math.Abs(this.Row), Math.Abs(this.Col));
        public void Modulus(Block mod) => (this.Row, this.Col) = (this.Row % mod.Row, this.Col % mod.Col);
    }
}
