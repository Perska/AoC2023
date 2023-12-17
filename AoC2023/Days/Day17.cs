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
		static void Day17(List<string> input)
		{
			var map = input.ToCharMap(out int mx, out int my);
			var explore = new Queue<(int x, int y, int direction, int limit, int heat)>();
			var exploreNext = new Queue<(int x, int y, int direction, int limit, int heat)>();
			var vec = new (int x, int y)[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
			var explored = new Dictionary<(int x, int y), int>();
			var visited = new Dictionary<(int x, int y, int direction, int limit), int>();

			explore.Enqueue((1, 0, 0, 1, map[0, 1] - '0'));
			explore.Enqueue((0, 1, 1, 1, map[1, 0] - '0'));

			int leeway = 10;

			while (explore.Count > 0)
			{
				while (explore.Count > 0)
				{
					(int x, int y, int direction, int limit, int heat) = explore.Dequeue();
					int best = explored.Read((x, y), int.MaxValue - leeway);
					int otherbest = visited.Read((x, y, direction, limit), int.MaxValue);
					if (best > heat) explored[(x, y)] = heat;
					if (otherbest > heat) visited[(x, y, direction, limit)] = heat;
					if (best + leeway <= heat) continue;
					if (otherbest <= heat) continue;
					(int nx, int ny) = (x + vec[direction].x, y + vec[direction].y);
					int nextheat;
					if (limit != 3 && !(nx < 0 || ny < 0 || mx <= nx || my <= ny))
					{
						nextheat = heat + map[ny, nx] - '0';
						if (explored.Read((x, y), int.MaxValue - 10) + leeway > nextheat) exploreNext.Enqueue((nx, ny, direction, limit + 1, nextheat));
					}

					direction = (direction + 1) % 4;
					(nx, ny) = (x + vec[direction].x, y + vec[direction].y);
					if (!(nx < 0 || ny < 0 || mx <= nx || my <= ny) && explored.Read((nx, ny), int.MaxValue - leeway) + leeway > (nextheat = heat + map[ny, nx] - '0') && visited.Read((nx, ny, direction, limit), int.MaxValue) > nextheat) exploreNext.Enqueue((nx, ny, direction, 1, heat + map[ny, nx] - '0'));

					direction = (direction + 2) % 4;
					(nx, ny) = (x + vec[direction].x, y + vec[direction].y);
					if (!(nx < 0 || ny < 0 || mx <= nx || my <= ny) && explored.Read((nx, ny), int.MaxValue - leeway) + leeway > (nextheat = heat + map[ny, nx] - '0') && visited.Read((nx, ny, direction, limit), int.MaxValue) > nextheat) exploreNext.Enqueue((nx, ny, direction, 1, heat + map[ny, nx] - '0'));
				}
				(explore, exploreNext) = (exploreNext, explore);
			}
			WriteLine($"Part 1: {explored.Read((mx - 1, my - 1))}");

			explored.Clear();
			visited.Clear();
			explore.Enqueue((1, 0, 0, 1, map[0, 1] - '0'));
			explore.Enqueue((0, 1, 1, 1, map[1, 0] - '0'));

			leeway = 500;

			while (explore.Count > 0)
			{
				while (explore.Count > 0)
				{
					(int x, int y, int direction, int limit, int heat) = explore.Dequeue();
					int best = explored.Read((x, y), int.MaxValue - leeway);
					int otherbest = visited.Read((x, y, direction, limit), int.MaxValue);
					if (best > heat && limit > 3)
					{
						explored[(x, y)] = heat;
					}

					if (otherbest > heat) visited[(x, y, direction, limit)] = heat;
					if (best + leeway <= heat) continue;
					if (otherbest <= heat) continue;
					(int nx, int ny) = (x + vec[direction].x, y + vec[direction].y);
					int nextheat;
					if (limit != 10 && !(nx < 0 || ny < 0 || mx <= nx || my <= ny))
					{
						nextheat = heat + map[ny, nx] - '0';
						if (explored.Read((nx, ny), int.MaxValue - leeway) + leeway > nextheat) exploreNext.Enqueue((nx, ny, direction, limit + 1, nextheat));
					}

					if (limit > 3)
					{
						direction = (direction + 1) % 4;
						(nx, ny) = (x + vec[direction].x, y + vec[direction].y);
						if (!(nx < 0 || ny < 0 || mx <= nx || my <= ny) && explored.Read((nx, ny), int.MaxValue - leeway) + leeway > (nextheat = heat + map[ny, nx] - '0') && visited.Read((nx, ny, direction, 1), int.MaxValue) > nextheat) exploreNext.Enqueue((nx, ny, direction, 1, nextheat));

						direction = (direction + 2) % 4;
						(nx, ny) = (x + vec[direction].x, y + vec[direction].y);
						if (!(nx < 0 || ny < 0 || mx <= nx || my <= ny) && explored.Read((nx, ny), int.MaxValue - leeway) + leeway > (nextheat = heat + map[ny, nx] - '0') && visited.Read((nx, ny, direction, 1), int.MaxValue) > nextheat) exploreNext.Enqueue((nx, ny, direction, 1, nextheat));
					}
				}
				(explore, exploreNext) = (exploreNext, explore);
			}
			WriteLine($"Part 2: {explored.Read((mx - 1, my - 1))}");
		}
	}
}
