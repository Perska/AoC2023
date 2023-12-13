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
		// [NoTrailingNewLine] // Uncomment to not include an extra blank line in the input at the end
		static void Day13(List<string> input)
		{
			var one = new List<string>();
			var maps = new List<(char[,] map, int mx, int my, int hor, int ver)>();
			int hori = 0, vert = 0;
			foreach (var line in input)
			{
				if (line == "")
				{
					var map = one.ToCharMap(out int mx, out int my);
					var (hor, ver) = find(map, mx, my);
					hori += hor;
					vert += ver;
					maps.Add((map, mx, my, hor, ver));
					one.Clear();
				}
				else
				{
					one.Add(line);
				}
			}
			WriteLine($"Part 1: {vert + hori * 100}");
			vert = hori = 0;
			foreach ((char[,] map, int mx, int my, int horz, int veri) in maps)
			{
				for (int y = 0; y < my; y++)
				{
					for (int x = 0; x < mx; x++)
					{
						map[y, x] = map[y, x] == '#' ? '.' : '#';
						var (hor, ver) = find(map, mx, my, veri, horz);
						if (horz != hor && hor != 0)
						{
							hori += hor;
							goto cont;
						}
						else if (veri != ver && ver != 0)
						{
							vert += ver;
							goto cont;
						}
						map[y, x] = map[y, x] == '#' ? '.' : '#';
					}
				}
			cont:;
			}
			WriteLine($"Part 2: {vert + hori * 100}");

			(int hori, int vert) find(char[,] map, int mx, int my, int ex = -1, int ey = -1)
			{
				int hor = 0, ver = 0;
				for (int y = 0; y < my - 1; y++)
				{
					bool ok = true;
					for (int y2 = 0; y2 < my / 2; y2++)
					{
						if (y - y2 < 0 || y + 1 + y2 >= my) continue;
						for (int x = 0; x < mx; x++)
						{
							if (map[y - y2, x] != map[y + 1 + y2, x]) ok = false;
						}
					}
					if (ok && y + 1 != ey)
					{
						hor = y + 1;
						break;
					}
				}
				for (int x = 0; x < mx - 1; x++)
				{
					bool ok = true;
					for (int x2 = 0; x2 < mx / 2; x2++)
					{
						if (x - x2 < 0 || x + 1 + x2 >= mx) continue;
						for (int y = 0; y < my; y++)
						{
							if (map[y, x - x2] != map[y, x + 1 + x2]) ok = false;
						}
					}
					if (ok && x + 1 != ex)
					{
						ver = x + 1;
						break;
					}
				}
				return (hor, ver);
			}
		}
	}
}
