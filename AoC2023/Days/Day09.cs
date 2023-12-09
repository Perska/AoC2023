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
		static void Day09(List<string> input)
		{
			long forward = 0;
			long backward = 0;
			foreach (var line in input)
			{
				var nums = line.SplitToIntArray(' ');
				var (f, b) = extrapolate(nums);
				forward += nums.Last() + f;
				backward += nums.First() - b;
			}
			WriteLine($"Part 1: {forward}");
			WriteLine($"Part 2: {backward}");

			(int f, int b) extrapolate(int[] numbers)
			{
				var nextNums = new int[numbers.Length - 1];
				for (int i = 1; i < numbers.Length; i++)
				{
					nextNums[i - 1] = numbers[i] - numbers[i - 1];
				}
				if (nextNums.Count(x => x == 0) == nextNums.Length)
				{
					return (0, 0);
				}
				var (f, b) = extrapolate(nextNums);
				return (nextNums.Last() + f, nextNums.First() - b);
			}
		}
	}
}
