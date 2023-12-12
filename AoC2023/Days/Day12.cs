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
					b.Append($" {num} ");
				}
				return b.ToString();
			}

			public override bool Equals(object obj)
			{
				if (!(obj is Sequence)) return false;
				var hint = obj as Sequence;
				if (Count == hint.Count)
				{
					bool ok = true;
					for (int j = 0; j < Count; j++)
					{
						if (Numbers[j] != hint.Numbers[j]) ok = false;
					}
					if (ok) return true;
				}
				return false;
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
			var cache = new Dictionary<(string pattern, Sequence finds, int index), long>();
			List<string> patterns = new List<string>();
			foreach (var line in input)
			{
				var data = line.SplitToStringArray(" ", true);
				var pattern = data[0];
				var hint = data[1].SplitToIntArray(',').ToList();
				var bigPattern = pattern + "?" + pattern + "?" + pattern + "?" + pattern + "?" + pattern;
				var bigHint = hint.Concat(hint).Concat(hint).Concat(hint).Concat(hint).ToList();
				bigHint.Add(0);
				hint.Add(0);
				arrangements += arrange(pattern, new Sequence(), 0, hint);
				bigArrangements += arrange(bigPattern, new Sequence(), 0, bigHint);
				cache.Clear(); // If I don't clear the cache between runs things get miscounted because some of the unfolded patterns may match the original patterns
			}
			WriteLine($"Part 1: {arrangements}");
			WriteLine($"Part 2: {bigArrangements}");

			long arrange(string pattern, Sequence sequence, int index, List<int> hint)
			{
				long hit = cache.Read((pattern, sequence, index), -1);
				if (hit != -1) return hit;
				if (index == pattern.Length)
				{
					var x = new Sequence(sequence);
					if (x.Numbers[sequence.Count - 1] != 0) x.Numbers.Add(0);
					return cache[(pattern, sequence, index)] = (match(x) ? 1 : 0);
				}
				long count = 0;
				if (pattern[index] == '?')
				{
					var ok = new Sequence(sequence);
					var bad = new Sequence(sequence);
					append(ok, true);
					append(bad, false);
					if (evaluate(ok)) count += arrange(pattern, ok, index + 1, hint);
					if (evaluate(bad)) count += arrange(pattern, bad, index + 1, hint);
				}
				else
				{
					var x = new Sequence(sequence);
					append(x, pattern[index] == '#');
					if (evaluate(x)) count += arrange(pattern, x, index + 1, hint);
				}

				cache[(pattern, sequence, index)] = count;
				return count;

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

				bool match(Sequence test)
				{
					if (test.Count == hint.Count)
					{
						bool ok = true;
						for (int j = 0; j < test.Count; j++)
						{
							if (test.Numbers[j] != hint[j]) ok = false;
						}
						if (ok) return true;
					}
					return false;
				}

				bool evaluate(Sequence test)
				{
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
	}
}
