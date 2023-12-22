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
		struct Cuboid
		{
			public int X1;
			public int X2;
			public int Y1;
			public int Y2;
			public int Z1;
			public int Z2;

			public Cuboid(int x1, int x2, int y1, int y2, int z1, int z2)
			{
				X1 = x1;
				X2 = x2;
				Y1 = y1;
				Y2 = y2;
				Z1 = z1;
				Z2 = z2;
			}

			public bool Valid
			{
				get
				{
					return (X1 <= X2 && Y1 <= Y2 && Z1 <= Z2);
				}
			}

			public long Width
			{
				get
				{
					return X2 - X1 + 1;
				}
			}

			public long Height
			{
				get
				{
					return Y2 - Y1 + 1;
				}
			}

			public long Depth
			{
				get
				{
					return Z2 - Z1 + 1;
				}
			}

			public long Volume
			{
				get
				{
					return Width * Height * Depth;
				}
			}

			public override string ToString()
			{
				return $"{X1}..{X2}, {Y1}..{Y2}, {Z1}..{Z2}";
			}

			public static Cuboid Intersection(Cuboid a, Cuboid b)
			{
				return new Cuboid(Math.Max(a.X1, b.X1), Math.Min(a.X2, b.X2), Math.Max(a.Y1, b.Y1), Math.Min(a.Y2, b.Y2), Math.Max(a.Z1, b.Z1), Math.Min(a.Z2, b.Z2));
			}

			public static Cuboid[] Abjunction(Cuboid a, Cuboid b)
			{
				Cuboid intersection = Intersection(a, b);
				if (intersection.Valid)
				{
					List<Cuboid> cuboids = new List<Cuboid>();
					if (intersection.X1 > a.X1)
					{
						cuboids.Add(new Cuboid(a.X1, intersection.X1 - 1, a.Y1, a.Y2, a.Z1, a.Z2));
					}
					if (intersection.X2 < a.X2)
					{
						cuboids.Add(new Cuboid(intersection.X2 + 1, a.X2, a.Y1, a.Y2, a.Z1, a.Z2));
					}
					if (intersection.Y1 > a.Y1)
					{
						cuboids.Add(new Cuboid(intersection.X1, intersection.X2, a.Y1, intersection.Y1 - 1, a.Z1, a.Z2));
					}
					if (intersection.Y2 < a.Y2)
					{
						cuboids.Add(new Cuboid(intersection.X1, intersection.X2, intersection.Y2 + 1, a.Y2, a.Z1, a.Z2));
					}
					if (intersection.Z1 > a.Z1)
					{
						cuboids.Add(new Cuboid(intersection.X1, intersection.X2, intersection.Y1, intersection.Y2, a.Z1, intersection.Z1 - 1));
					}
					if (intersection.Z2 < a.Z2)
					{
						cuboids.Add(new Cuboid(intersection.X1, intersection.X2, intersection.Y1, intersection.Y2, intersection.Z2 + 1, a.Z2));
					}
					return cuboids.ToArray();
				}
				else
				{
					return new Cuboid[] { a };
				}
			}
		}

		// [UseSRL] // Uncomment if you wanna use SuperReadLine
		[NoTrailingNewLine] // Uncomment to not include an extra blank line in the input at the end
		static void Day22(List<string> input)
		{
			List<Cuboid> cubes = new List<Cuboid>();
			var floor = new Cuboid(0, 10, 0, 10, 0, 0);

			foreach (var line in input)
			{
				var s = line.SplitToIntArray(',', '~');
				cubes.Add(new Cuboid(s[0], s[3], s[1], s[4], s[2], s[5]));
			}
			cubes.Sort((a, b) => (a.Z1 > b.Z1 ? 1 : a.Z1 < b.Z1 ? -1 : 0));

			fall(cubes);

			int removables = 0;
			long sum = 0;
			for (int i = 0; i < cubes.Count; i++)
			{
				var removed = cubes.ToList();
				removed.RemoveAt(i);
				var drops = fall(removed);
				if (drops == 0) removables++;
				sum += drops;
				WriteLine($"Trying to break brick {i} (Resulted in {drops} bricks falling)");
			}

			WriteLine("Part 1: {removables}");
			WriteLine("Part 2: {sum}");

			int fall(List<Cuboid> cuboids)
			{
				var drops = new HashSet<int>();
				bool any = false;
				while (true)
				{
					bool fell = false;
					for (int i = 0; i < cuboids.Count; i++)
					{
						var next = cuboids[i];
						next.Z1--;
						next.Z2--;
						if (Cuboid.Intersection(floor, next).Valid) continue;
						bool ok = true;
						for (int j = 0; j < cuboids.Count; j++)
						{
							if (i == j) continue;
							if (Cuboid.Intersection(cuboids[j], next).Valid)
							{
								ok = false;
								break;
							}
						}
						if (ok)
						{
							any = true;
							drops.Add(i);
							fell = true;
							cuboids[i] = next;
						}
					}
					if (!fell) break;
				}
				return drops.Count;
			}
		}
	}
}
