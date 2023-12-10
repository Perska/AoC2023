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
		static void Day10(List<string> input)
		{
			var map = input.ToCharMap(out int maxX, out int maxY);
			var bigmap = new char[maxY * 3, maxX * 3];
			var visited = new Dictionary<(int x, int y), int>();
			var explore = new Queue<(int x, int y, int steps)>();
			var exploreNext = new Queue<(int x, int y, int steps)>();
			(int x, int y)[] vec = new (int x, int y)[4] { (1, 0), (0, 1), (-1, 0), (0, -1) };
			string[] permit = new string[4] { "-7J", "|LJ", "-LF", "|7F" };
			string[] canmovefrom = new string[4] { "S-LF", "S|7F", "S-7J", "S|LJ" };

			char[] shapeID = new[] { '-',          '|',         'L',         'J',         '7',         'F',         'S'};
			string[] shapes = new[] { "   ###   ", " #  #  # ", " #  ##   ", " # ##    ", "   ##  # ", "    ## # ", "#########" };

			int startX = -1, startY = -1;
			for (int j = 0; j < maxY; j++)
			{
				for (int i = 0; i < maxX; i++)
				{
					if (map[j, i] == 'S') (startX, startY) = (i, j);
					int id;
					if ((id = Array.FindIndex(shapeID, c => c == map[j, i])) != -1)
					{
						for (int k = 0; k < 9; k++)
						{
							bigmap[j * 3 + k / 3, i * 3 + k % 3] = shapes[id][k];
						}
					}
				}
			}
			explore.Enqueue((startX, startY, 0));
			visited[(startX, startY)] = 0;
			int farthest = 0;
			while (farthest == 0)
			{
				while (explore.Count > 0)
				{
					(int x, int y, int s) = explore.Dequeue();
					for (int i = 0; i < 4; i++)
					{
						(int nx, int ny) = (x + vec[i].x, y + vec[i].y);
						if (nx < 0 || ny < 0 || maxX <= nx || maxY <= ny) continue;
						if (permit[i].Contains(map[ny, nx]) && canmovefrom[i].Contains(map[y, x]))
						{
							if (visited.Read((nx, ny)) == s + 1)
							{
								farthest = s + 1;
							}
							else if (!visited.ContainsKey((nx, ny)))
							{
								visited[(nx, ny)] = s + 1;
								exploreNext.Enqueue((nx, ny, s + 1));
							}
						}
					}
				}

				/*
				for (int o = 0; o < maxY * 3; o++)
				{
					for (int p = 0; p < maxX * 3; p++)
					{
						int t = visited.Read((p / 3, o / 3));
						if (t != 0) Write($"#"); else
							Write($"{bigmap[o, p]}");
					}
					WriteLine();
				}
				WriteLine();*/

				(explore, exploreNext) = (exploreNext, explore);
			}
			WriteLine($"Part 1: {farthest}");

			for (int j = 0; j < maxY; j++)
			{
				for (int i = 0; i < maxX; i++)
				{
					if (!visited.ContainsKey((i, j)))
					{
						for (int k = 0; k < 9; k++)
						{
							bigmap[j * 3 + k / 3, i * 3 + k % 3] = ' ';
						}
					}
				}
			}

			visited.Clear();
			explore.Clear();
			explore.Enqueue((-1, 0, 0));
			explore.Enqueue((maxX * 3, 0, 0));
			explore.Enqueue((-1, maxY * 3 - 1, 0));
			explore.Enqueue((maxX * 3, maxY * 3 - 1, 0));

			//Clear();
			//for (int o = 0; o < maxY * 3; o++)
			//{
			//	for (int p = 0; p < maxX * 3; p++)
			//	{
			//		/*int t = visited.Read((p / 3, o / 3));
			//		if (t != 0) Write($"#");
			//		else*/
			//			Write($"{bigmap[o, p]}");
			//	}
			//	WriteLine();
			//}
			//WriteLine();
			//ReadLine();

			while (explore.Count > 0)
			{
				while (explore.Count > 0)
				{
					(int x, int y, int s) = explore.Dequeue();
					for (int i = 0; i < 4; i++)
					{
						(int nx, int ny) = (x + vec[i].x, y + vec[i].y);
						if (nx < 0 || ny < 0 || maxX * 3 <= nx || maxY * 3 <= ny) continue;
						if (bigmap[ny, nx] != '#')
						{
							if (!visited.ContainsKey((nx, ny)))
							{
								visited[(nx, ny)] = s + 1;
								exploreNext.Enqueue((nx, ny, s + 1));
							}
						}
					}
				}

				/*Clear();
				for (int o = 0; o < maxY * 3; o++)
				{
					for (int p = 0; p < maxX * 3; p++)
					{
						int t = visited.Read((p, o));
						if (t != 0) Write($"O");
						else Write($"{bigmap[o, p]}");
					}
					WriteLine();
				}
				WriteLine();
				ReadLine();*/

				(explore, exploreNext) = (exploreNext, explore);
			}
			/*for (int o = 0; o < maxY * 3; o++)
			{
				for (int p = 0; p < maxX * 3; p++)
				{
					int t = visited.Read((p, o));
					if (t != 0) Write($"O");
					else Write($"{bigmap[o, p]}");
				}
				WriteLine();
			}
			WriteLine();
			ReadLine();
			Clear();*/
			int nest = 0;
			for (int j = 0; j < maxY; j++)
			{
				for (int i = 0; i < maxX; i++)
				{
					if (!visited.ContainsKey((i * 3, j * 3)) && bigmap[j * 3 + 1, i * 3 + 1] != '#')
					{
						nest++;
						//Write("I");
						/*for (int k = 0; k < 9; k++)
						{
							bigmap[j * 3 + k / 3, i * 3 + k % 3] = reset[k];
						}*/
					}
					else
					{
						//Write(map[j, i]);
					}
				}
				//WriteLine();
			}

			WriteLine($"Part 2: {nest}");
		}
	}
}
