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
		static void Day21(List<string> input)
		{
			var map = input.ToCharMap(out int mx, out int my);
			var visited = new Dictionary<(int x, int y), int>();
			var visitedGrid = new Dictionary<(int x, int y), long>();
			var prevVisitedGrid = new Dictionary<(int x, int y), long>();
			var prevPrevVisitedGrid = new Dictionary<(int x, int y), long>();
			var repeated = new Dictionary<(int x, int y), int>();
			var finalGrid = new Dictionary<(int x, int y), long>();
			var frozen = new Dictionary<(int x, int y), bool>();
			var explore = new Queue<(int x, int y, int steps)>();
			var exploreNext = new Queue<(int x, int y, int steps)>();
			(int x, int y)[] vec = new (int x, int y)[4] { (1, 0), (0, 1), (-1, 0), (0, -1) };
			int sx = 0, sy = 0;
			for (int y = 0; y < my; y++)
			{
				for (int x = 0; x < mx; x++)
				{
					if (map[y, x] == 'S') (sx, sy) = (x, y);
				}
			}
			int quota = 64;
			long part1 = 0;
		startover:;
			if (part1 != 0)
			{
				quota = 327;
				frozen.Clear();
				prevPrevVisitedGrid.Clear();
				prevVisitedGrid.Clear();
			}

			explore.Enqueue((sx, sy, quota));
			visited[(sx, sy)] = quota;
			int cycle = 1;
			var last = new Queue<long>(11);
			long repeatsA = 0, repeatsB = 0;
			while (cycle <= quota)
			{
				visited.Clear();
				visitedGrid.Clear();
				while (explore.Count > 0)
				{
					(int x, int y, int s) = explore.Dequeue();
					int gx = (int)Math.Floor(x / (float)mx);
					int gy = (int)Math.Floor(y / (float)my);
					if (frozen.ContainsKey((gx, gy))) continue;
					for (int i = 0; i < 4; i++)
					{
						(int nx, int ny) = (x + vec[i].x, y + vec[i].y);
						if (part1 == 0 && (nx < 0 || ny < 0 || mx <= nx || my <= ny)) continue;
						int cx = nx + mx * 300000;
						int cy = ny + my * 300000;
						cx = cx % mx;
						cy = cy % my;
						if (map[cy, cx] != '#' && s > 0)
						{
							if (visited.Read((nx, ny), -1) != s - 1)
							{
								visited[(nx, ny)] = s - 1;
								gx = (int)Math.Floor(nx / (float)mx);
								gy = (int)Math.Floor(ny / (float)my);
								visitedGrid[(gx, gy)] = visitedGrid.Read((gx, gy)) + 1;
								exploreNext.Enqueue((nx, ny, s - 1));
							}
						}
					}
				}
				if (repeatsA == 0 && part1 != 0)
				{
					last.Enqueue(visitedGrid[(0, 0)]);
					if (last.Count > 10)
					{
						last.Dequeue();
						if (last.Distinct().Count() == 2)
						{
							if (cycle % 2 == quota % 2) last.Dequeue();
							repeatsA = finalGrid[(0, 0)] = last.Dequeue();
							repeatsB = last.Dequeue();
							frozen[(0, 0)] = true;
						}
					}
				}
				if (part1 != 0)
				{
					foreach (var grid in visitedGrid)
					{
						if (!frozen.Read(grid.Key) && grid.Value == repeatsB && grid.Value == prevVisitedGrid[grid.Key])
						{
							if ((repeated[grid.Key] = repeated.Read(grid.Key) + 1) == 2)
							{
								frozen[grid.Key] = true;
								int g = ((grid.Key.x + grid.Key.y) % 2 + 2) % 2;
								finalGrid[grid.Key] = (g == 0) ? repeatsA : repeatsB;
							}
						}
					}
				}
				(explore, exploreNext) = (exploreNext, explore);
				(visitedGrid, prevVisitedGrid, prevPrevVisitedGrid) = (prevVisitedGrid, prevPrevVisitedGrid, visitedGrid);
				cycle++;
			}
			if (part1 == 0)
			{
				part1 = visited.Where(x => x.Value == 0).LongCount();
				WriteLine($"Part 1: {part1}");
				goto startover;
			}
			var gr = prevPrevVisitedGrid;
			foreach (var froze in finalGrid)
			{
				gr[froze.Key] = froze.Value;
			}
			long part2 = 0;
			long m = gr[(0, 0)];
			long m2 = gr[(0, 1)];
			long e = gr[(2, 0)] + gr[(-2, 0)] + gr[(0, 2)] + gr[(0, -2)];
			long h = gr[(1, 1)] + gr[(-1, 1)] + gr[(-1, -1)] + gr[(1, -1)];
			long d = gr[(2, 1)] + gr[(-2, 1)] + gr[(-2, -1)] + gr[(2, -1)];
			part2 = e + d * 202300 + h * 202299 + m2 * 202300 * 202300 + m * 202299 * 202299;
			WriteLine($"Part 2: {part2}");
		}
	}
}
