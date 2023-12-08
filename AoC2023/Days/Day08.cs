using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static System.Console;
using static AoC2023.Useful;

namespace AoC2023
{
	partial class Program
	{
		// [UseSRL] // Uncomment if you wanna use SuperReadLine
		[NoTrailingNewLine] // Uncomment to not include an extra blank line in the input at the end
		static void Day08(List<string> input)
		{
			string path = input[0];
			var nodes = new Dictionary<string, (string l, string r)>();

			for (int i = 2; i < input.Count; i++)
			{
				var split = input[i].SplitToStringArray(new char[] { ' ', '=', '(', ',', ')' }, true);
				nodes[split[0]] = (split[1], split[2]);
			}

			string current = "AAA";
			int steps = 0;
			while (current != "ZZZ")
			{
				current = path[steps % path.Length] == 'L' ? nodes[current].l : nodes[current].r;
				steps++;
			}
			WriteLine($"Part 1: {steps}");

			var searchers = new Queue<(string, int)>();
			var times = new List<long>();
			foreach (var node in nodes)
			{
				if (node.Key.EndsWith("A")) searchers.Enqueue((node.Key, 0));
			}
			while (searchers.Count > 0)
			{
				(string current, int steps) spot = searchers.Dequeue();

				spot.current = path[spot.steps % path.Length] == 'L' ? nodes[spot.current].l : nodes[spot.current].r;
				spot.steps++;
				if (spot.current.EndsWith("Z"))
				{
					times.Add(spot.steps);
				}
				else
				{
					searchers.Enqueue(spot);
				}
			}
			
			WriteLine($"Part 2: {LCM(times)}");
		}
	}
}
