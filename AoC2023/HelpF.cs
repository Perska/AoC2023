using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023
{
	/// <summary>
	/// This class contains code that usually are helpful for multiple days, or aren't written by me.
	/// If any are by someone else, they are marked as such.
	/// </summary>
	static class HelpF
	{
		/// <summary>Split a string into separate strings, as specified by the delimiter.</summary>
		public static string[] SplitToStringArray(this string str, string split, bool removeEmpty)
		{
			return str.Split(new string[] { split }, removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
		}

		/// <summary>Split a string into separate strings, as specified by the delimiter.</summary>
		public static string[] SplitToStringArray(this string str, char[] split, bool removeEmpty)
		{
			return str.Split(split, removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
		}

		/// <summary>Split a string into an int array.</summary>
		public static int[] SplitToIntArray(this string str, string split)
		{
			return Array.ConvertAll(str.SplitToStringArray(split, true), s => int.Parse(s));
		}

		/// <summary>Split a string into an int array.</summary>
		public static int[] SplitToIntArray(this string str, params char[] split)
		{
			return Array.ConvertAll(str.SplitToStringArray(split, true), s => int.Parse(s));
		}

		/// <summary>Split a string into a long array.</summary>
		public static long[] SplitToLongArray(this string str, string split)
		{
			return Array.ConvertAll(str.SplitToStringArray(split, true), s => long.Parse(s));
		}

		/// <summary>Split a string into a long array.</summary>
		public static long[] SplitToLongArray(this string str, params char[] split)
		{
			return Array.ConvertAll(str.SplitToStringArray(split, true), s => long.Parse(s));
		}

		public static V Read<K,V>(this Dictionary<K,V> dict, K key)
		{
			if (dict.ContainsKey(key)) return dict[key];
			return default(V);
		}

		public static V Read<K, V>(this Dictionary<K, V> dict, K key, V def)
		{
			if (dict.ContainsKey(key)) return dict[key];
			return def;
		}
		public static char[,] ToCharMap(this List<string> input, out int maxX, out int maxY)
		{
			maxX = input[0].Length;
			maxY = input.Count;
			char[,] map = new char[maxY, maxX];
			for (int y = 0; y < maxY; y++)
			{
				for (int x = 0; x < maxX; x++)
				{
					map[y, x] = input[y][x];
				}
			}
			return map;
		}
	}

	static class Useful
	{
		/// <summary>Get the Least Common Multiplier of a IEnumerable</summary>
		public static long LCM(IEnumerable<long> list)
		{
			long total = 1;
			foreach (var num in list)
			{
				total = total * num / GCD(total, num);
			}
			return total;
		}

		/// <summary>Get the Greatest Common Divider of two longs</summary>
		public static long GCD(long a, long b)
		{
			if (b == 0) return a;
			return GCD(b, a % b);
		}
	}
}