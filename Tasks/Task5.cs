namespace AdventOfCode2024.Tasks
{
    public class Task5 : AdventTask
    {
        public Task5()
        {
            Filename += "5.txt";
        }

        public class Page(int PageIndex, HashSet<int> pagesBefore, HashSet<int> pagesAfter)
        {
            public int PageIndex { get; set; } = PageIndex;
            public HashSet<int> PagesBefore { get; set; } = pagesBefore;
        }

        public override void Solve1(string input)
        {
            var result = 0;
            var (i, pagesMap, lines) = GetPagesMap(input);
            i++;
            while (i < lines.Length)
            {
                var currLine = lines[i].Trim().Split(",").Select(int.Parse).ToList();
                if (CanPrint(pagesMap, currLine))
                    result += currLine[currLine.Count / 2];
                i++;
            }
            Console.WriteLine(result);
        }

        private (int, Dictionary<int, Page>, string[] lines) GetPagesMap(string input)
        {
            var lines = input.Split('\n');
            var pagesMap = new Dictionary<int, Page>();
            var i = 0;
            while (lines[i] != "" && lines[i] != "\r")
            {
                var currLine = lines[i].Split("|");
                var firstPage = int.Parse(currLine[0]);
                var secondPage = int.Parse(currLine[1]);
                if (pagesMap.ContainsKey(secondPage))
                    pagesMap[secondPage].PagesBefore.Add(firstPage);
                else
                    pagesMap.Add(secondPage, new Page(secondPage, new HashSet<int> { firstPage }, new HashSet<int>()));
                i++;
            }
            return (i, pagesMap, lines);
        }

        private bool CanPrint(Dictionary<int, Page> pagesMap, List<int> currLine)
        {
            var pagesPrinted = new HashSet<int>();
            foreach (var page in currLine)
            {
                if (pagesPrinted.Contains(page))
                    return false;
                pagesPrinted.Add(page);
                pagesPrinted = pagesPrinted.Union(pagesMap[page].PagesBefore).ToHashSet();
            }
            return true;
        }

        public override void Solve2(string input)
        {
            var result = 0;
            var (i, pagesMap, lines) = GetPagesMap(input);
            i++;
            while (i < lines.Length)
            {
                var currLine = lines[i].Trim().Split(",").Select(int.Parse).ToList();
                if (!CanPrint(pagesMap, currLine))
                { 
                    var j = 0;
                    while (j < currLine.Count)
                    {
                        var page = currLine[j];
                        var switched = false;
                        for (var k = j + 1; k < currLine.Count; k++)
                        {
                            if (pagesMap[page].PagesBefore.Contains(currLine[k]))
                            {
                                currLine[j] = currLine[k];
                                currLine[k] = page;
                                switched = true;
                                break;
                            }                                
                        }
                        if (!switched)
                            j++;
                    }
                    result += currLine[currLine.Count / 2];
                }
                i++;
            }
            Console.WriteLine(result);
        }
    }
}
