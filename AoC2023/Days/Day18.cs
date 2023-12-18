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
		static void Day18(List<string> input)
		{
			var points = new List<(long x, long y)>();
			var points2 = new List<(long x, long y)>();
			var vec = new Dictionary<string, (int x, int y)>() { { "R", (1, 0) }, { "D", (0, 1) }, { "L", (-1, 0) }, { "U", (0, -1) } };
			var vec2 = new (int x, int y)[4] { (1, 0), (0, 1), (-1, 0), (0, -1) };

			int px = 0, py = 0;
			int px2 = 0, py2 = 0;
			points.Add((px, py));
			points2.Add((px, py));
			long extra = 0, extra2 = 0;
			foreach (var line in input)
			{
				var c = line.SplitToStringArray(new char[] { ' ', '(', '#', ')' }, true);
				int times = int.Parse(c[1]);
				int times2 = Convert.ToInt32(c[2].Substring(0, 5), 16);
				int dir = c[2][5] - '0';
				extra += times;
				extra2 += times2;
				WriteLine(times2);
				(px, py) = (px + vec[c[0]].x * times, py + vec[c[0]].y * times);
				(px2, py2) = (px2 + vec2[dir].x * times2, py2 + vec2[dir].y * times2);
				points.Add((px, py));
				points2.Add((px2, py2));
			}

			WriteLine($"Part 1: {excavate(points, extra)}");
			WriteLine($"Part 2: {excavate(points2, extra2)}");

			long excavate(List<(long x, long y)> pts, long ext)
			{
				return Math.Abs(pts.Take(pts.Count - 1).Select((p, i) => (pts[i + 1].y * p.x) - (pts[i + 1].x * p.y)).Sum() / 2) + ext / 2 + 1;
			}
		}
	}
}
