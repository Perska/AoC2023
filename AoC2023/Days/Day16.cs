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
		static void Day16(List<string> input)
		{
			var map = input.ToCharMap(out int mx, out int my);
			var vec = new (int x, int y)[] { (1, 0), (0, 1), (-1, 0), (0, -1) };

			int best = simulate(-1, 0, 0);
			WriteLine(best);
			for (int y = 0; y < my; y++)
			{
				if (y != 0) best = Math.Max(best, simulate(-1, y, 0));
				best = Math.Max(best, simulate(mx, y, 2));
			}
			for (int x = 0; x < mx; x++)
			{
				best = Math.Max(best, simulate(x, -1, 1));
				best = Math.Max(best, simulate(x, my, 3));
			}
			WriteLine(best);

			int simulate(int startX, int startY, int startDirection)
			{
				var energized = new Dictionary<(int x, int y), bool>();
				var visited = new Dictionary<(int x, int y, int direction), bool>();
				var beams = new List<(int x, int y, int direction)>();
				var newBeams = new List<(int x, int y, int direction)>();
				beams.Add((startX, startY, startDirection));

				int timeout = 100;
				int lastCount = 0;
				while (timeout > 0 && beams.Count > 0)
				{
					for (int i = 0; i < beams.Count; i++)
					{
						var (x, y, direction) = beams[i];
						(x, y) = (x + vec[direction].x, y + vec[direction].y);
						if (!(x < 0 || y < 0 || x >= mx || y >= my))
						{
							switch (map[y, x])
							{
								case '\\':
									switch (direction)
									{
										case 0:
											direction = 1;
											break;
										case 1:
											direction = 0;
											break;
										case 2:
											direction = 3;
											break;
										case 3:
											direction = 2;
											break;
									}
									break;
								case '/':
									switch (direction)
									{
										case 0:
											direction = 3;
											break;
										case 1:
											direction = 2;
											break;
										case 2:
											direction = 1;
											break;
										case 3:
											direction = 0;
											break;
									}
									break;
								case '-':
									if (direction % 2 == 1)
									{
										direction = 0;
										newBeams.Add((x, y, 2));
									}
									break;
								case '|':
									if (direction % 2 == 0)
									{
										direction = 1;
										newBeams.Add((x, y, 3));
									}
									break;
							}
						}

						beams[i] = (x, y, direction);
					}
					beams.RemoveAll(beam => beam.x < 0 || beam.y < 0 || beam.x >= mx || beam.y >= my);
					beams.RemoveAll(beam => visited.ContainsKey(beam));
					beams.AddRange(newBeams);
					newBeams.Clear();
					foreach (var (x, y, direction) in beams)
					{
						energized[(x, y)] = true;
						visited[(x, y, direction)] = true;
					}
					if (energized.Count > lastCount)
					{
						lastCount = energized.Count;
						timeout = 100;
					}
					timeout--;
				}
				return energized.Count;
			}
		}
	}
}
