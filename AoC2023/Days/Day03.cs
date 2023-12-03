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
		static void Day03(List<string> input)
		{
			var symbols = new List<(int x, int y, char type)>();
			char[,] map = input.ToCharMap(out int maxX, out int maxY);
			for (int y = 0; y < maxY; y++)
			{
				for (int x = 0; x < maxX; x++)
				{
					if (map[y, x] != '.' && !char.IsDigit(map[y, x])) symbols.Add((x, y, map[y, x]));
				}
			}

			long partsSum = 0;
			long gearsSum = 0;
			foreach (var symbol in symbols)
			{
				long ratio = 1;
				int adjacent = 0;
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						if (bounds(symbol.y + i, symbol.x + j))
						{
							if (char.IsDigit(map[symbol.y + i, symbol.x + j]))
							{
								StringBuilder num = new StringBuilder();
								num.Append(map[symbol.y + i, symbol.x + j]);
								map[symbol.y + i, symbol.x + j] = ' ';
								for (int k = symbol.x + j - 1; 0 <= k; k--)
								{
									if (char.IsDigit(map[symbol.y + i, k])) num.Insert(0, map[symbol.y + i, k]); else break;
									map[symbol.y + i, k] = ' ';
								}
								for (int k = symbol.x + j + 1; k < maxX; k++)
								{
									if (char.IsDigit(map[symbol.y + i, k])) num.Append(map[symbol.y + i, k]); else break;
									map[symbol.y + i, k] = ' ';
								}
								long value = long.Parse(num.ToString());
								partsSum += value;
								ratio *= value;
								adjacent++;
							}
						}

						bool bounds(int y, int x)
						{
							if (x < 0 || y < 0 || x >= maxX || y >= maxY) return false;
							return true;
						}
					}
				}
				if (symbol.type == '*' && adjacent == 2) gearsSum += ratio;
			}
			WriteLine(partsSum);
			WriteLine(gearsSum);
		}
	}
}
