using System.Linq;
using System.Net;
using System.Runtime.InteropServices;

namespace AdventOfCode2024.Tasks
{
    public class Task23: AdventTask
    {
        public Task23()
        {
            Filename += "23.txt";
        }

        public override void Solve1(string input)
        {
            var nodes = GetNodesOfNetwork(input);
            var tNodes = nodes.Where(n => n.Key.StartsWith("t"));
            var uniqueParties = new HashSet<string>();
            foreach (var tNode in tNodes)
            {
                foreach(var neighbor in tNode.Value)
                {
                    var commonNeighbors = tNode.Value.Intersect(nodes[neighbor]).ToList();
                    foreach (var common in commonNeighbors)
                    {
                        var party = new HashSet<string> { tNode.Key, neighbor, common };
                        uniqueParties.Add(string.Join("", party.Order()));
                    }
                }
            }

            Console.WriteLine(uniqueParties.Count);
        }

        public override void Solve2(string input)
        {
            var nodes = GetNodesOfNetwork(input);
            //var visitedNodes = new HashSet<string>();
            //var parties = nodes.Keys.ToHashSet();
            //while (parties.Count > 1)
            //{
            //    var largerParties = new HashSet<string>();
            //    foreach (var party in parties)
            //    {
            //        var splittedParty = party.Split(",").ToList();
            //        foreach(var node in nodes.Keys)
            //        {
            //            if (!splittedParty.Contains(node) &&
            //                nodes[node].Intersect(splittedParty).Count() == splittedParty.Count)
            //            {
            //                var tempParty = splittedParty.ToList();
            //                tempParty.Add(node);
            //                largerParties.Add(string.Join(",", tempParty.Order()));
            //            }
            //        }
            //    }
            //    parties = largerParties.ToHashSet();
            //}
            //Console.WriteLine(parties.First());
            long result = 0;
            var lanParty = "";
            foreach(var (node, neighbors) in nodes)
            {
                var party = new HashSet<string> { node };
                if (neighbors.Count < result)
                    continue;

                foreach(var neighbor in neighbors)
                {
                    if (party.All(partyNode => nodes[neighbor].Contains(partyNode)))
                        party.Add(neighbor);
                }

                if (party.Count > result)
                {
                    result = party.Count;
                    lanParty = string.Join(",", party.Order());
                }
            }

            Console.WriteLine(result);
            Console.WriteLine(lanParty);
        }

        private Dictionary<string, HashSet<string>> GetNodesOfNetwork(string input)
        {
            var lines = GetLinesList(input).Select(l => l.Split("-")).ToList();
            var nodes = new Dictionary<string, HashSet<string>>();
            foreach (var line in lines)
            {
                var (n1, n2) = (line[0], line[1]);
                if (!nodes.ContainsKey(n1))
                    nodes.Add(n1, new HashSet<string>());
                if (!nodes.ContainsKey(n2))
                    nodes.Add(n2, new HashSet<string>());
                nodes[n1].Add(n2);
                nodes[n2].Add(n1);
            }
            return nodes;
        }
    }
}
