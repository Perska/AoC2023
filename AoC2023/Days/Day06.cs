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
		static void Day06(List<string> input)
		{
			var times = input[0].SplitToStringArray(":", true)[1].SplitToIntArray(" ");
			var distances = input[1].SplitToStringArray(":", true)[1].SplitToIntArray(" ");
			long truetime = long.Parse(input[0].SplitToStringArray(":", true)[1].Replace(" ", ""));
			long truedistance = long.Parse(input[1].SplitToStringArray(":", true)[1].Replace(" ", ""));

			long margin = 1;
			for (int i = 0; i < times.Length; i++)
			{
				margin = margin * race(times[i], distances[i]);
			}

			int race(long time, long distance)
			{
				int winners = 0;
				for (int j = 0; j <= time; j++)
				{
					if (distance < (time - j) * j) winners++;
				}
				return winners;
			}

			WriteLine($"Part 1: {margin}");
			WriteLine($"Part 2: {race(truetime, truedistance)}");
		}
	}
}
