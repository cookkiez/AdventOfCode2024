using System.Data;

namespace AdventOfCode2024.Tasks
{
    public class Task22: AdventTask
    {
        public Task22()
        {
            Filename += "22.txt";
        }

        public override void Solve1(string input)
        {
            long result = 0;
            var buyers = GetLinesList(input).Select(long.Parse);
            foreach(var buyer in buyers)
                result += GenerateBuyerPrices(buyer, false).Last();
            Console.WriteLine(result);
        }

        public override void Solve2(string input)
        {
            long result = 0;
            var buyers = GetLinesList(input).Select(long.Parse);
            var sequenceChanges = new Dictionary<(long, long, long, long), long>();
            foreach (var buyer in buyers)
            {
                var buyerPrices = GenerateBuyerPrices(buyer, true);

                // Calculate differences between price changes
                var priceChanges = new List<long>();
                for (var i = 1; i < buyerPrices.Count; i++)
                    priceChanges.Add(buyerPrices[i] - buyerPrices[i - 1]);

                buyerPrices = buyerPrices.Skip(1).ToList();

                // Generate sequences of 4 and get the price for them. Add this to global dict that stores values for all buyers
                var currentBuyerChanges = new HashSet<(long, long, long, long)>();
                for(var i = 3; i < priceChanges.Count; i++)
                {
                    var currentSequence = (priceChanges[i - 3],  priceChanges[i - 2], priceChanges[i - 1], priceChanges[i]);
                    if (!currentBuyerChanges.Contains(currentSequence))
                    {
                        currentBuyerChanges.Add(currentSequence);
                        CheckAndAddToDictionary(sequenceChanges, currentSequence, buyerPrices[i]);
                    }
                }
            }
            // Take the price where we get the maximum number of bananas
            result = sequenceChanges.Max(kvp => kvp.Value);
            Console.WriteLine(result);
        }     
        
        private List<long> GenerateBuyerPrices(long secretNum, bool getDigit = false)
        {
            // Generate buyer prices based on rules from the task
            var buyerPrices = new List<long> { secretNum.Digit() };
            for (var i = 0; i < 2000; i++)
            {
                secretNum = secretNum.Mix(secretNum.Multiply(64)).Prune();
                secretNum = secretNum.Mix(secretNum.Divide(32));
                secretNum = secretNum.Mix(secretNum.Multiply(2048)).Prune();
                buyerPrices.Add(getDigit ? secretNum.Digit() : secretNum);
            }
            return buyerPrices;
        }
    }    

    static class LongExtensions
    {
        public static long Mix(this long num, long givenNum) => num ^ givenNum;
        public static long Prune(this long num) => num % 16777216;
        public static long Multiply(this long num, long multiplier) => num * multiplier;
        public static long Divide(this long num, long divisor) => num / divisor;
        public static long Digit(this long num) => num % 10;
    }
}
