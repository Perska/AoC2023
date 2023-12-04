using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static System.Console;

namespace AoC2023
{
	partial class Program
	{
		// [UseSRL] // Uncomment if you wanna use SuperReadLine
		[NoTrailingNewLine] // Uncomment to not include an extra blank line in the input at the end
		static void Day04(List<string> input)
		{
			long value = 0;
			long cards = 0;
			Queue<int> dupes = new Queue<int>();
			Dictionary<int, int> nets = new Dictionary<int, int>();
			for (int i = 0; i < input.Count; i++)
			{
				dupes.Enqueue(i);
			}
			while (dupes.Count > 0)
			{
				int i = dupes.Dequeue();
				int point = 0;
				if (!nets.ContainsKey(i))
				{
					var card = input[i].SplitToStringArray(new char[] { ':', '|' }, true);
					int[] winners = card[1].SplitToIntArray(' ');
					int[] scratched = card[2].SplitToIntArray(' ');
					point = winners.Intersect(scratched).Count();
					nets[i] = point;
				}
				for (int j = 0; j < nets[i]; j++)
				{
					dupes.Enqueue(i + 1 + j);
				}
				value += point != 0 ? (long)Math.Pow(2, point - 1) : 0;
				cards++;
			}
			WriteLine($"Part 1: {value}");
			WriteLine($"Part 2: {cards}");
		}
	}
}
