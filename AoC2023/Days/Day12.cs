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
		internal class Sequence
		{
			public List<int> Numbers;

			public int Count => Numbers.Count;

			public Sequence()
			{
				Numbers = new List<int>();
				Numbers.Add(0);
			}

			public Sequence(Sequence from)
			{
				Numbers = from.Numbers.ToList();
			}

			public override string ToString()
			{
				StringBuilder b = new StringBuilder();
				foreach (var num in Numbers)
				{
					b.Append($"{num} ");
				}
				return b.ToString();
			}

			public override int GetHashCode()
			{
				return ToString().GetHashCode();
			}
		}
		
		// [UseSRL] // Uncomment if you wanna use SuperReadLine
		[NoTrailingNewLine] // Uncomment to not include an extra blank line in the input at the end
		static void Day12(List<string> input)
		{
			long arrangements = 0;
			long bigArrangements = 0;
			int a = 0;
			//var compare = new ListCompare();
			foreach (var line in input)
			{
				var data = line.SplitToStringArray(" ", true);
				var pattern = data[0];
				var hint = data[1].SplitToIntArray(',').ToList();
				var bigPattern = pattern + "?" + pattern + "?" + pattern + "?" + pattern + "?" + pattern;
				var bigHint = hint.ToList();
				bigHint.AddRange(hint);
				bigHint.AddRange(hint);
				bigHint.AddRange(hint);
				bigHint.AddRange(hint);
				bigHint.Add(0);
				hint.Add(0);
				arrangements += arrange(pattern, hint);
				bigArrangements += arrange(bigPattern, bigHint);
				WriteLine($"{a + 1}/{input.Count}");
				a++;
			}

			WriteLine(arrangements);
			WriteLine(bigArrangements);


			long arrange(string pattern, List<int> hint)
			{
				//var finds = new List<List<int>>();
				//var newFinds = new List<List<int>>();
				var finds = new Dictionary<Sequence, int>();
				var newFinds = new Dictionary<Sequence, int>();
				finds.Add(new Sequence(), 1);
				for (int i = 0; i < pattern.Length; i++)
				{
					WriteLine($" {i + 1}/{pattern.Length}");
					//WriteLine(finds.Count);
					if (i == 13)
						;
					if (pattern[i] == '?')
					{
						foreach (var x in finds)
						{
							int count = x.Value;
							var ok = x.Key;
							var bad = new Sequence(x.Key);
							append(ok, true);
							append(bad, false);
							if (evaluate(ok)) add(newFinds, new KeyValuePair<Sequence, int>(ok, count));
							if (evaluate(bad)) add(newFinds, new KeyValuePair<Sequence, int>(bad, count));
						}
					}
					else
					{
						foreach (var x in finds)
						{
							append(x.Key, pattern[i] == '#');
							//x.Append(pattern[i]); 
							if (evaluate(x.Key)) add(newFinds, new KeyValuePair<Sequence, int>(x.Key, x.Value));
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
					//finds = newFinds.Distinct(compare).ToList();
					//finds.Clear();
					//foreach (var item in newFinds)
					//{
					//	
					//}
					(finds, newFinds) = (newFinds, finds);
					newFinds.Clear();
				}

				long total = 0;
				foreach (var find in finds)
				{
					if (find.Key.Numbers[find.Key.Count - 1] != 0) find.Key.Numbers.Add(0);
					if (find.Key.Numbers.SequenceEqual(hint)) total += find.Value;
				}
				return total;
				//return finds.Where(x => x.Key.Numbers.SequenceEqual(hint)).Count();


				void add(Dictionary<Sequence, int> thing, KeyValuePair<Sequence, int> item)
				{
					if (thing.ContainsKey(item.Key))
					{
						thing[item.Key] += item.Value;
					}
					else
					{
						thing[item.Key] = item.Value;
					}
				}

				void append(Sequence x, bool damaged)
				{
					if (damaged)
					{
						x.Numbers[x.Count - 1]++;
					}
					else
					{
						if (x.Numbers[x.Count - 1] != 0) x.Numbers.Add(0);
					}
				}

				bool evaluate(Sequence test)
				{
					//Write($"{str} ");
					//foreach (var num in match)
					//{
					//	Write($"{num.Length} ");
					//}
					//WriteLine();
					if (test.Count <= hint.Count)
					{
						bool ok = true;
						for (int j = 0; j < test.Count; j++)
						{
							if (j != test.Count - 1 && test.Numbers[j] != hint[j]) ok = false;
							if (test.Numbers[j] > hint[j]) ok = false;
						}
						if (ok) return true;
					}
					return false;
				}
			}
		}

		/*private class ListCompare : IEqualityComparer<List<int>>
		{
			public bool Equals(List<int> a, List<int> b)
			{
				return a.SequenceEqual(b);
			}

			public int GetHashCode(List<int> obj)
			{
				return base.GetHashCode();
			}
		}*/
	}
}
