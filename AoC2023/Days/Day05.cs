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
		// [NoTrailingNewLine] // Uncomment to not include an extra blank line in the input at the end
		static void Day05(List<string> input)
		{
			var maps = new List<List<long[]>>();
			var seeds = input[0].SplitToStringArray(": ", true)[1].SplitToLongArray(" ");
			var map = new List<long[]>();
			for (int i = 2; i < input.Count; i++)
			{
				if (input[i] == "")
				{
					maps.Add(map);
					map = new List<long[]>();
				}
				else if (char.IsDigit(input[i][0]))
				{
					map.Add(input[i].SplitToLongArray(" "));
				}
			}

			long lowest = long.MaxValue;

			foreach (var seed in seeds)
			{
				long location = seed;
				for (int i = 0; i < maps.Count; i++)
				{
					map = maps[i];
					for (int j = 0; j < map.Count; j++)
					{
						if (map[j][1] <= location && location < map[j][1] + map[j][2])
						{
							location = location - map[j][1] + map[j][0];
							break;
						}
					}
				}
				lowest = Math.Min(lowest, location);
			}

			long maximum = long.MinValue;
			for (int i = 0; i < seeds.Length; i += 2)
			{
				maximum = Math.Max(maximum, seeds[i] + seeds[i + 1]);
			}

			WriteLine($"Part 1: {lowest}");
			long bruteforce = 0;
			while (true)
			{
				long location = bruteforce;
				for (int i = maps.Count - 1; i >= 0; i--)
				{
					map = maps[i];
					for (int j = 0; j < map.Count; j++)
					{
						if (map[j][0] <= location && location < map[j][0] + map[j][2])
						{
							location = location - map[j][0] + map[j][1];
							break;
						}
					}
				}
				for (int i = 0; i < seeds.Length; i += 2)
				{
					if (seeds[i] <= location && location < seeds[i] + seeds[i + 1])
					{
						goto found;
					}
				}
				bruteforce++;
			}
		found:;
			WriteLine($"Part 2: {bruteforce}");
		}
	}
}
