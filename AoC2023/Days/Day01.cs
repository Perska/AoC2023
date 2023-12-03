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
		static void Day01(List<string> input)
		{
			Dictionary<string, int> words = new Dictionary<string, int>() { { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 }, { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 } };
			string pattern1 = "[0-9]";
			string pattern2 = "[0-9]|one|two|three|four|five|six|seven|eight|nine";
			Regex regex1 = new Regex($"^.*?(?=({pattern1})).*({pattern1}).*$");
			Regex regex2 = new Regex($"^.*?(?=({pattern2})).*({pattern2}).*$");
			long sum1 = 0, sum2 = 0;
			foreach (string line in input)
			{
				var match = regex1.Match(line);
				sum1 += int.Parse(match.Groups[1].Value) * 10 + int.Parse(match.Groups[2].Value);
				match = regex2.Match(line);
				string a = match.Groups[1].Value, b = match.Groups[2].Value;
				sum2 += (char.IsDigit(a[0]) ? int.Parse(a) : words[a]) * 10 + (char.IsDigit(b[0]) ? int.Parse(b) : words[b]);
			}
			WriteLine($"Part 1: The sum is {sum1}");
			WriteLine($"Part 2: The sum is {sum2}");
		}
	}
}
