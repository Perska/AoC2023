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
		static void Day23(List<string> input)
		{
			var map = input.ToCharMap(out int mx, out int my);
			var vec = new (int x, int y)[4] { (1, 0), (0, 1), (-1, 0), (0, -1) };
			var vec2 = new Dictionary<char, (int x, int y)>() { { '>', (1, 0) }, { 'v', (0, 1) }, { '<', (-1, 0) }, { '^', (0, -1) } };

			//WriteLine(findPath(1, 0, 0, new HashSet<(int, int)>(), false));
			WriteLine(findPath(1, 0, 0, new HashSet<(int, int)>(), true));

			int findPath(int x, int y, int steps, HashSet<(int x, int y)> path, bool part2)
			{
				if (path.Contains((x, y))) return 0;
				path.Add((x, y));
				if (y == my - 1) return steps;
				//WriteLine($"Explored {steps} on {x}, {y}");
				int best = 0;
				char tile = map[y, x];
				if (!part2 && vec2.ContainsKey(tile))
				{
					var (nx, ny) = (x + vec2[tile].x, y + vec2[tile].y);
					if (nx < 0 || ny < 0 || mx <= nx || my <= ny) return 0;
					if (map[ny, nx] != '#') best = Math.Max(best, findPath(nx, ny, steps + 1, new HashSet<(int x, int y)>(path), part2));
				}
				else
				{
					for (int i = 0; i < 4; i++)
					{
						var (px, py) = (x, y);
						var (nx, ny) = (x + vec[i].x, y + vec[i].y);
						if (nx < 0 || ny < 0 || mx <= nx || my <= ny) continue;
						if (map[ny, nx] != '#')
						{
							int turnSteps = steps + 1;
							var turnPath = new HashSet<(int x, int y)> { (nx, ny) };
							//var turnPath = new List<(int, int)> { (nx, ny) };
							while (part2)
							{
								int branching = 0;
								int branch = 0;
								for (int j = 0; j < 4; j++)
								{
									var (bx, by) = (nx + vec[j].x, ny + vec[j].y);
									if (bx < 0 || by < 0 || mx <= bx || my <= by) continue;
									//if (!part2 && vec2.ContainsKey(map[by, bx]))
									//	continue;
									//WriteLine($"{part2} {map[by, bx]}");
									if (map[by, bx] != '#' && !path.Contains((bx, by)) && !turnPath.Contains((bx, by)))
									{
										branch = j;
										branching++;// && !turnPath.Contains((bx, by))) branching++;
									}
								}
								if (branching != 1)
								{
									//turnPath.Add((nx, ny));
									break;
								}

								{
									int j = branch;
									var (bx, by) = (nx + vec[j].x, ny + vec[j].y);
									//if (bx < 0 || by < 0 || mx <= bx || my <= by) continue;
									//if (map[by, bx] != '#' && !turnPath.Contains((bx, by)))// && !turnPath.Contains((bx, by)))
									//{
									turnSteps++;
									turnPath.Add((bx, by));
									//turnPath.Add((bx, by));
									//turnPath.Add((bx, by));
									(px, py) = (nx, ny);
									(nx, ny) = (bx, by);
									//}
								}
							}
							var newPath = turnPath.ToList();
							int add = 0;
							//if (turnPath.Count == 1)
							//{
							//	add = 1;
							//	newPath = path.Append(turnPath[0]);
							//}
							if (newPath.Count == 2)
							{
								//add = 2;
								//newPath = path.Concat(new[] { newPath[0] });//, turnPath[turnPath.Count - 1] });
								best = Math.Max(best, findPath(nx, ny, turnSteps, new HashSet<(int x, int y)>(path.Concat(new[] { newPath[0] })), part2));
							}
							else if (newPath.Count > 2)
							{
								//add = 3;
								//newPath = path.Concat(new[] { newPath[0], newPath[newPath.Count - 2] });//, turnPath[turnPath.Count - 1] } );
								best = Math.Max(best, findPath(nx, ny, turnSteps, new HashSet<(int x, int y)>(path.Concat(new[] { newPath[0], newPath[newPath.Count - 2] })), part2));
							}
							else
							{
								best = Math.Max(best, findPath(nx, ny, turnSteps, new HashSet<(int x, int y)>(newPath), part2));
							}
						}
					}
				}
				//if (best != 0) WriteLine(best);
				return best;
			}
		}
	}
}
