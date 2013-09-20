using System;
using System.Collections.Generic;

namespace SumUniques
{
	/* Part 1 of Boomtown! employment test. Started at 2:15pm. */
	class MainClass
	{
		/* 
		 * To sum the unique entries in the list, we first compute the frequencies of each list
		 * entry and store them in a hash. This will let us know whether to add the entry
		 * to the final sum in the second pass. This way, we don't have to scan the entire array
		 * for each term in the sum.
		 */
		public static int SumUniques(List<int> l)  {
			if (l == null) {
				return -1;
			}

			int sum = 0;
			Dictionary<int, int> frequencies = new Dictionary<int, int> ();

			foreach (int item in l) {
				int freq;
				if (frequencies.TryGetValue (item, out freq)) {
					frequencies [item] = freq + 1;
				} else {
					frequencies [item] = 1;
				}
			}

			/* Now we can iterate the dictionary and compute our final sum. */
			foreach (KeyValuePair<int, int> kv in frequencies) {
				if (kv.Value == 1) {
					sum += kv.Key;
				}
			}

			return sum;
		}

		public static void Main (string[] args)
		{
			List<int> l = new List<int> { 1,5,2,5,7,3 };
			Console.WriteLine (SumUniques(l));
		}
	}
}
