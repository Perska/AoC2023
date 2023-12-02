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
			string[] words = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
			long sum = 0;
			foreach (string line in input)
			{
				int a = 0, b = 0;
				for (int i = 0; i < line.Length; i++)
				{
					if ('0' <= line[i] && line[i] <= '9')
					{
						a = line[i] - '0';
						break;
					}
				}
				for (int i = line.Length - 1; i >= 0; i--)
				{
					if ('0' <= line[i] && line[i] <= '9')
					{
						b = line[i] - '0';
						break;
					}
				}
				sum += a * 10 + b;
			}
			WriteLine($"Part 1: The sum is {sum}");
			sum = 0;
			foreach (string line in input)
			{
				int a = 0, b = 0;
				for (int i = 0; i < line.Length; i++)
				{
					if ('0' <= line[i] && line[i] <= '9')
					{
						a = line[i] - '0';
						break;
					}
					for (int j = 0; j < words.Length; j++)
					{
						if (i + words[j].Length < line.Length && line.Substring(i, words[j].Length) == words[j])
						{
							a = j + 1;
							goto aOK;
						}
					}
				}
			aOK:;
				for (int i = line.Length - 1; i >= 0; i--)
				{
					if ('0' <= line[i] && line[i] <= '9')
					{
						b = line[i] - '0';
						break;
					}
					for (int j = 0; j < words.Length; j++)
					{
						if (i - words[j].Length + 1 >= 0 && line.Substring(i - words[j].Length + 1, words[j].Length) == words[j])
						{
							b = j + 1;
							goto bOK;
						}
					}
				}
			bOK:;
				WriteLine(a * 10 + b);
				sum += a * 10 + b;
			}
			WriteLine($"Part 2: The sum is {sum}");
		}
	}
}
