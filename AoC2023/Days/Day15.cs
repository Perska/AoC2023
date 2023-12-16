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
		static void Day15(List<string> input)
		{
			string str = string.Join("", input);
			var values = str.SplitToStringArray(",", true);
			var regex = new Regex(@"(\w+)(?:(-)|=(\d))");

			int sum = 0;
			foreach (var value in values)
			{
				sum += hash(value);
			}
			WriteLine($"Part 1: {sum}");

			var boxes = new List<(string label, int lens)>[256];
			for (int i = 0; i < 256; i++)
			{
				boxes[i] = new List<(string, int)>();
			}

			foreach (var value in values)
			{
				var match = regex.Match(value);
				string label = match.Groups[1].Value;
				string type = match.Groups[2].Value;
				int.TryParse(match.Groups[3].Value, out int lens);
				int box = hash(label);
				if (type == "-")
				{
					int index;
					if ((index = boxes[box].FindIndex(x => x.label == label)) != -1)
					{
						boxes[box].RemoveAt(index);
					}
				}
				else
				{
					int index;
					if ((index = boxes[box].FindIndex(x => x.label == label)) != -1)
					{
						boxes[box][index] = (label, lens);
					}
					else
					{
						boxes[box].Add((label, lens));
					}
				}
			}

			long power = 0;
			for (int i = 0; i < 256; i++)
			{
				for (int j = 0; j < boxes[i].Count; j++)
				{
					power += (i + 1) * (j + 1) * boxes[i][j].lens;
				}
			}
			WriteLine($"Part 2: {power}");

			int hash(string s)
			{
				int ha = 0;
				for (int i = 0; i < s.Length; i++)
				{
					ha += s[i];
					ha *= 17;
					ha %= 256;
				}
				return ha;
			}
		}
	}
}
