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
		static void Day11(List<string> input)
		{
			var map = input.ToCharMap(out int mx, out int my);
			var emptyX = new List<int>();
			var emptyY = new List<int>();
			var galaxies = new List<(int x, int y)>();
			var distances = new Dictionary<(int a, int b), long>();
			var bigDistances = new Dictionary<(int a, int b), long>();

			for (int y = 0; y < my; y++)
			{
				for (int x = 0; x < mx; x++)
				{
					if (map[y, x] == '#') galaxies.Add((x, y));
				}
			}
			for (int y = 0; y < my; y++)
			{
				if (!galaxies.Any(g => g.y == y)) emptyY.Add(y);
			}
			for (int x = 0; x < mx; x++)
			{
				if (!galaxies.Any(g => g.x == x)) emptyX.Add(x);
			}

			for (int a = 0; a < galaxies.Count; a++)
			{
				for (int b = 0; b < galaxies.Count; b++)
				{
					if (a == b) continue;
					if (distances.ContainsKey((Math.Min(a, b), Math.Max(a, b)))) continue;

					(int sx, int sy) = galaxies[a];
					(int ex, int ey) = galaxies[b];
					long steps = 0, bigSteps = 0;
					while (sx != ex || sy != ey)
					{
						if (sx < ex) sx++;
						else if (sx > ex) sx--;
						else if (sy < ey) sy++;
						else if (sy > ey) sy--;
						if (emptyX.Contains(sx) || emptyY.Contains(sy))
						{
							steps += 2;
							bigSteps += 1000000;
						}
						else
						{
							steps++;
							bigSteps++;
						}
					}
					distances[(Math.Min(a, b), Math.Max(a, b))] = steps;
					bigDistances[(Math.Min(a, b), Math.Max(a, b))] = bigSteps;
				}
			}

			WriteLine($"Part 1: {distances.Values.Sum()}");
			WriteLine($"Part 2: {bigDistances.Values.Sum()}");
		}
	}
}
