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
		static void Day20(List<string> input)
		{
			var modules = new Dictionary<string, (bool on, bool type, Dictionary<string, bool> memo, string[] outputs)>();
			var pulses = new Queue<(string name, bool on, string from)>();

			foreach (var line in input)
			{
				var split = line.SplitToStringArray(new char[] { ' ', '-', '>', ',' }, true);
				string name = split[0];
				bool type = false;
				if (name[0] == '&') type = true;
				if (name != "broadcaster") name = name.Substring(1);
				var outputs = split.Skip(1).ToArray();
				modules.Add(name, (false, type, type ? new Dictionary<string, bool>() : null, outputs));
			}

			foreach (var module in modules)
			{
				foreach (var output in module.Value.outputs)
				{
					if (modules.ContainsKey(output) && modules[output].type) modules[output].memo[module.Key] = false;
				}
			}

			long lo = 0, hi = 0;
			long rx = long.MaxValue;
			var rxm = modules.First(x => x.Value.outputs.Any(y => y == "rx")).Value.memo.ToDictionary(x => x.Key, x => 0);
			long part1 = 0;
			int i = 0;
			while (i < 1000 || rx == int.MaxValue)
			{
				lo++;
				sendPulse("broadcaster", false);

				while (pulses.Count > 0)
				{
					var (name, high, from) = pulses.Dequeue();
					var (on, type, memo, outputs) = modules[name];
					//WriteLine($"{from} -{(high ? "hi" : "lo")}-> {name}");
					if (type) // Conjuction
					{
						memo[from] = high;
						on = !memo.All(x => x.Value);
						sendPulse(name, on);
					}
					else if (!high) // Flip-flop
					{
						on = !on;
						sendPulse(name, on);
					}
					modules[name] = (on, type, memo, outputs);
					//ReadKey(true);
				}
				if (rxm.Values.All(x => x != 0)) rx = LCM(rxm.Select(x => (long)x.Value));
				if (i == 999) part1 = hi * lo;
				i++;
			}
			WriteLine($"Part 1: {part1}");
			WriteLine($"Part 2: {rx}");

			void sendPulse(string module, bool high)
			{
				foreach (var output in modules[module].outputs)
				{
					if (!high && rxm.ContainsKey(output))
					{
						rxm[output] = i + 1;
					}
					if (!high && output == "rx")
					{
						rx = Math.Min(rx, i + 1);
					}

					if (high) hi++; else lo++;
					if (modules.ContainsKey(output)) pulses.Enqueue((output, high, module));
				}
			}
		}
	}
}
