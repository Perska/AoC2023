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
		static void Day02(List<string> input)
		{
			Dictionary<string, int> limit = new Dictionary<string, int>() { { "red", 12 }, { "green", 13 }, { "blue", 14 } };
			Dictionary<string, int> minimum = new Dictionary<string, int>();
			int sum = 0;
			int powerSum = 0;
			int ID = 1;
			Regex cubes = new Regex(@"(\d+) (\w+)");
			foreach (string game in input)
			{
				minimum.Clear();
				bool OK = true;
				string[] turns = game.SplitToStringArray(";", false);
				foreach (string turn in turns)
				{
					MatchCollection matches = cubes.Matches(turn);
					foreach (Match match in matches)
					{
						if (int.Parse(match.Groups[1].Value) > limit[match.Groups[2].Value]) OK = false;
						minimum[match.Groups[2].Value] = Math.Max(minimum.Read(match.Groups[2].Value), int.Parse(match.Groups[1].Value));
					}
				}
				if (OK) sum += ID;
				powerSum += minimum["red"] * minimum["green"] * minimum["blue"];
				ID++;
			}
			WriteLine($"Part 1: Sum of game IDs is {sum}");
			WriteLine($"Part 2: Sum of powers is {powerSum}");
		}
	}
}
