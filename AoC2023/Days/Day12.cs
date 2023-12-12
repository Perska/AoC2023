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
		static void Day12(List<string> input)
		{
			int arrangements = 0;
			int bigArrangements = 0;
			int a = 0;
			foreach (var line in input)
			{
				var data = line.SplitToStringArray(" ", true);
				var pattern = data[0];
				var hint = data[1].SplitToIntArray(',');
				var bigPattern = pattern + "?" + pattern + "?" + pattern + "?" + pattern + "?" + pattern;
				var bigHint = hint.ToList();
				bigHint.AddRange(hint);
				bigHint.AddRange(hint);
				bigHint.AddRange(hint);
				bigHint.AddRange(hint);
				arrangements += arrange(pattern, hint);
				bigArrangements += arrange(bigPattern, bigHint.ToArray());
				WriteLine($"{a + 1}/{input.Count}");
				a++;
			}

			WriteLine(arrangements);
			WriteLine(bigArrangements);


			int arrange(string pattern, int[] hint)
			{
				var finds = new Queue<StringBuilder>();
				var newFinds = new Queue<StringBuilder>();
				finds.Enqueue(new StringBuilder());
				for (int i = 0; i < pattern.Length; i++)
				{
					if (pattern[i] == '?')
					{
						while (finds.Count > 0)
						{
							var x = finds.Dequeue();
							var ok = new StringBuilder(x.ToString()).Append('#');
							var bad = new StringBuilder(x.ToString()).Append('.');
							if (evaluate(ok)) newFinds.Enqueue(ok);
							if (evaluate(bad)) newFinds.Enqueue(bad);
						}
					}
					else
					{
						while (finds.Count > 0)
						{
							var x = finds.Dequeue();
							x.Append(pattern[i]); if (evaluate(x)) newFinds.Enqueue(x);
						}
						//find.Append(pattern[i]);
					}
					//finds.AddRange(newFinds);
					//newFinds.Clear();
					//foreach (var find in newFinds)
					//{
					//	var str = find.ToString();
					//	var match = str.SplitToStringArray(".", true);
					//	/*if (match.Length <= hint.Length)
					//	{
					//		bool ok = true;
					//		for (int j = 0; j < match.Length; j++)
					//		{
					//			if (match[j].Length > hint[j]) ok = false;
					//		}
					//		if (ok) newFinds.Add(find);
					//	}*/
					//	//Write($"{str} ");
					//	//foreach (var num in match)
					//	//{
					//	//	Write($"{num.Length} ");
					//	//}
					//	//WriteLine();
					//}
					//WriteLine();
					(finds, newFinds) = (newFinds, finds);
					newFinds.Clear();
				}
				return finds.Where(x => x.ToString().SplitToStringArray(".", true).Select(s => s.Length).SequenceEqual(hint)).Count();
				
				bool evaluate(StringBuilder test)
				{
					var str = test.ToString();
					var match = str.SplitToStringArray(".", true);

					//Write($"{str} ");
					//foreach (var num in match)
					//{
					//	Write($"{num.Length} ");
					//}
					//WriteLine();
					if (match.Length <= hint.Length)
					{
						bool ok = true;
						for (int j = 0; j < match.Length; j++)
						{
							if (match[j].Length > hint[j]) ok = false;
						}
						if (ok) return true;
					}
					return false;
				}
			}
		}
	}
}
