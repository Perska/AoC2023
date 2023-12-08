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
		static void Day07(List<string> input)
		{
			string cards = "AKQJT98765432";
			var signatures = new Dictionary<string, int>
			{
				{ "5", 0 }, { "X4", 0 }, { "XX3", 0 }, { "XXX2", 0 }, { "XXXX1", 0 }, { "XXXXX", 0 },
				{ "14", 1 }, { "X13", 1 }, { "XX12", 1 }, { "XXX11", 1 },
				{ "23", 2 }, { "X22", 2 },
				{ "113", 3 }, { "X112", 3 }, { "XX111", 3 },
				{ "122", 4 },
				{ "1112", 5 }, { "X1111", 5 },
				{ "11111", 6 }
			};
			var evaluated = new Dictionary<string, int>();
			var list = new List<(string card, int bid)>();

			foreach (var line in input)
			{
				string[] pair = line.SplitToStringArray(" ", true);
				string drawn = pair[0];
				var groups = drawn.GroupBy(x => x).OrderBy(x => x.Count());
				StringBuilder signature = new StringBuilder();
				foreach (var group in groups)
				{
					signature.Append(group.Count());
				}
				int bid = int.Parse(pair[1]);
				evaluated[drawn] = signatures[signature.ToString()];
				list.Add((drawn, bid));
			}

			list.Sort((a, b) => compare(a, b));
			long totals = 0;
			for (int i = 0; i < list.Count; i++)
			{
				totals += list[i].bid * (i + 1);
			}
			WriteLine($"Part 1: {totals}");

			cards = "AKQT98765432J";
			foreach (var line in list)
			{
				string drawn = line.card;
				var groups = drawn.GroupBy(x => x).OrderBy(x => x.Count());
				StringBuilder signature = new StringBuilder();
				int wild = 0;
				foreach (var group in groups)
				{
					if (group.Key == 'J')
					{
						wild = group.Count();
						continue;
					}
					signature.Append(group.Count());
				}
				signature.Insert(0, "X", wild);
				evaluated[drawn] = signatures[signature.ToString()];
			}

			list.Sort((a, b) => compare(a, b)); totals = 0;
			for (int i = 0; i < list.Count; i++)
			{
				totals += list[i].bid * (i + 1);
			}
			WriteLine($"Part 2: {totals}");

			int compare((string card, int bid) a, (string card, int bid) b)
			{
				if (evaluated[a.card] < evaluated[b.card]) return 1;
				if (evaluated[a.card] > evaluated[b.card]) return -1;
				for (int i = 0; i < 5; i++)
				{
					if (cards.IndexOf(a.card[i]) < cards.IndexOf(b.card[i])) return 1;
					if (cards.IndexOf(a.card[i]) > cards.IndexOf(b.card[i])) return -1;
				}
				return 0;
			}
		}
	}
}
