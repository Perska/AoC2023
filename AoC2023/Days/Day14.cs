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
		static void Day14(List<string> input)
		{
			var map = input.ToCharMap(out int mx, out int my);
			var cache = new Dictionary<string, int>();
			while (true)
			{
				if (!roll(0)) break;
			}
			WriteLine($"Part 1: {load()}");
			for (int i = 0; i < 1000000000; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					while (roll(j)) ;
				}
				char[] state = new char[mx * my];
				for (int y = 0; y < my; y++)
				{
					for (int x = 0; x < mx; x++)
					{
						state[x + y * mx] = map[y, x];
					}
				}
				string key = new string(state);
				if (cache.ContainsKey(key))
				{
					int start = cache[key];
					int end = i;
					int length = end - start;
					int remain = 1000000000 - 1 - start;
					key = cache.First(x => x.Value == start + remain % length).Key;
					for (int y = 0; y < my; y++)
					{
						for (int x = 0; x < mx; x++)
						{
							map[y, x] = key[x + y * mx];
						}
					}
					break;
				}
				cache[key] = i;
			}
			WriteLine($"Part 2: {load()}");

			int load()
			{
				int l = 0;
				for (int y = 0; y < my; y++)
				{
					for (int x = 0; x < mx; x++)
					{
						if (map[y, x] == 'O')
						{
							l += my - y;
						}
					}
				}
				return l;
			}

			bool roll(int direction)
			{
				bool rolled = false;
				switch (direction % 4)
				{
					case 0: // North
						for (int y = 1; y < my; y++)
						{
							for (int x = 0; x < mx; x++)
							{
								if (map[y, x] == 'O' && map[y - 1, x] == '.')
								{
									map[y, x] = '.';
									map[y - 1, x] = 'O';
									rolled = true;
								}
							}
						}
						break;
					case 1: // West
						for (int x = 1; x < mx; x++)
						{
							for (int y = 0; y < my; y++)
							{
								if (map[y, x] == 'O' && map[y, x - 1] == '.')
								{
									map[y, x] = '.';
									map[y, x - 1] = 'O';
									rolled = true;
								}
							}
						}
						break;
					case 2: // South
						for (int y = my - 2; y >= 0; y--)
						{
							for (int x = 0; x < mx; x++)
							{
								if (map[y, x] == 'O' && map[y + 1, x] == '.')
								{
									map[y, x] = '.';
									map[y + 1, x] = 'O';
									rolled = true;
								}
							}
						}
						break;
					case 3: // East
						for (int x = mx - 2; x >= 0; x--)
						{
							for (int y = 0; y < my; y++)
							{
								if (map[y, x] == 'O' && map[y, x + 1] == '.')
								{
									map[y, x] = '.';
									map[y, x + 1] = 'O';
									rolled = true;
								}
							}
						}
						break;
				}
				return rolled;
			}
		}
	}
}
