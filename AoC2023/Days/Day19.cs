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
		//class Workflow
		//{
		//	struct Rule
		//	{
		//		char Cheks
		//	}
		//}

		// [UseSRL] // Uncomment if you wanna use SuperReadLine
		[NoTrailingNewLine] // Uncomment to not include an extra blank line in the input at the end
		static void Day19(List<string> input)
		{
			var workflows = new Dictionary<string, List<(string checks, bool condition, int value, string target)>>();
			var parts = new List<Dictionary<string, int>>();

			int i;
			for (i = 0; i < input.Count; i++)
			{
				if (input[i] == "") break;
				var split = input[i].SplitToStringArray(new char[] { '{', ',', '}' }, true);
				var key = split[0];
				var list = new List<(string, bool, int, string)>();
				for (int j = 1; j < split.Length; j++)
				{
					var pt = split[j].SplitToStringArray(new char[] { '<', '>', ':' }, true);
					if (pt.Length == 1)
					{
						list.Add((null, false, 0, pt[0]));
					}
					else
					{
						list.Add((pt[0], split[j].Contains(">"), int.Parse(pt[1]), pt[2]));
					}
				}
				workflows[key] = list;
			}
			for (i++; i < input.Count; i++)
			{
				var split = input[i].SplitToStringArray(new char[] { '{', ',', '=', '}' }, true);
				var part = new Dictionary<string, int>();
				for (int j = 0; j < split.Length; j += 2)
				{
					part[split[j]] = int.Parse(split[j + 1]);
				}
				parts.Add(part);
			}

			WriteLine(parts.Where(x => validate("in", x)).Sum(x => x.Values.Sum()));
			WriteLine(truncate("in", new Dictionary<string, (long min, long max)>() { { "x", (1, 4000) }, { "m", (1, 4000) }, { "a", (1, 4000) }, { "s", (1, 4000) } }));

			bool validate(string workflow, Dictionary<string, int> part)
			{
				if (workflow == "A") return true;
				if (workflow == "R") return false;
				var flow = workflows[workflow];
				for (int j = 0; j < flow.Count; j++)
				{
					var (checks, condition, value, target) = flow[j];
					if (checks == null) return validate(target, part);
					if (condition ? part[checks] > value : part[checks] < value) return validate(target, part);
				}
				return false;
			}
			
			long truncate(string workflow, Dictionary<string, (long min, long max)> range)
			{
				if (workflow == "A") return (range["x"].max - range["x"].min + 1) * (range["m"].max - range["m"].min + 1) * (range["a"].max - range["a"].min + 1) * (range["s"].max - range["s"].min + 1);
				if (workflow == "R") return 0;
				var flow = workflows[workflow];
				long valid = 0;
				for (int j = 0; j < flow.Count; j++)
				{
					var (checks, condition, value, target) = flow[j];
					var remainder = range.ToDictionary(x => x.Key, x => x.Value);
					if (checks == null) valid += truncate(target, range);
					else if (condition)
					{
						if (range[checks].max > value)
						{
							var newRange = range.ToDictionary(x => x.Key, x => x.Value);
							newRange[checks] = (value + 1, newRange[checks].max);
							valid += truncate(target, newRange);
						}
						remainder[checks] = (remainder[checks].min, Math.Min(remainder[checks].max, value));
					}
					else
					{
						if (range[checks].min < value)
						{
							var newRange = range.ToDictionary(x => x.Key, x => x.Value);
							newRange[checks] = (newRange[checks].min, value - 1);
							valid += truncate(target, newRange);
						}
						remainder[checks] = (Math.Max(remainder[checks].min, value), remainder[checks].max);
					}
					range = remainder;
				}
				return valid;
			}
		}
	}
}
