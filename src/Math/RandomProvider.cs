using System;
using System.Collections.Generic;

namespace Tools.Math {

	/// <summary>
	/// Contains functions for performing thread-safe randomization algorithms.
	/// </summary>
	public static class RandomProvider
	{
		private static readonly Random Global = new Random();

		[ThreadStatic]
		private static Random Local;

		/// <summary>
		/// Returns a random int in the interval [0, |maxValue| - 1].
		/// If maxValue is 0, this function returns 0.
		/// </summary>
		public static int GetInt(int maxValue = int.MaxValue)
		{
			CheckThreadLocalMember();
			return Local.Next(System.Math.Abs(maxValue));
		}

		/// <summary>
		/// Returns a random int in the interval [minValue, maxValue - 1].
		/// If minValue equals maxValue, minValue is returned. Either or both
		/// parameters may be negative.
		/// </summary>
		public static int GetInt(int minValue, int maxValue)
		{
			CheckThreadLocalMember();
			return Local.Next(minValue, maxValue);
		}

		/// <summary>
		/// Returns a random double in the interval [0.0, 1.0).
		/// </summary>
		public static double GetUnitDouble()
		{
			CheckThreadLocalMember();
			return Local.NextDouble();
		}

		/// <summary>
		/// Returns a random double in the interval [0.0, |maxValue|).
		/// </summary>
		public static double GetDouble(double maxValue)
		{
			return GetUnitDouble() * System.Math.Abs(maxValue);
		}

		/// <summary>
		/// Returns a random double in the interval [minValue, maxValue).
		/// Either or both parameters may be negative.
		/// </summary>
		public static double GetDouble(double minValue, double maxValue)
		{
			if (minValue > maxValue)
				throw new ArgumentException("minValue may not exceed maxValue.");

			double diff = maxValue - minValue;
			return GetUnitDouble() * diff + minValue;
		}

		/// <summary>
		/// Flips a weighted coin.
		/// </summary>
		/// <param name="weight">A probability in [0, 1]</param>
		/// <returns>True with probability equal to the given weight</returns>
		public static bool FlipCoin(double weight = 0.5)
		{
			return GetUnitDouble() < weight;
		}

		/// <summary>
		/// Returns an element selected uniformly at random from a collection.
		/// </summary>
		public static T Select<T>(IList<T> items)
		{
			if (items.Count == 0)
				throw new ArgumentException("The collection cannot be empty.");

			return items[GetInt(items.Count)];
		}

		/// <summary>
		/// Samples a collection uniformly at random.
		/// </summary>
		/// <param name="items">A nonempty collection</param>
		/// <param name="sampleSize">The number of items to be returned in the sample</param>
		/// <param name="withReplacement">Indicates whether the sample should allow duplicates</param>
		/// <returns>A list of items of length sampleSize</returns>
		public static List<T> Sample<T>(IList<T> items, int sampleSize, bool withReplacement)
		{
			if (sampleSize <= 0 || (!withReplacement && sampleSize > items.Count))
				throw new ArgumentException("Invalid sample size.");
			else if (items.Count == 0)
				throw new ArgumentException("The provided collection cannot be empty.");

			List<T> result = new List<T>();
			if (withReplacement)
			{
				CheckThreadLocalMember();

				for (int i = 0; i < sampleSize; ++i)
				{
					result.Add(items[Local.Next(items.Count)]);
				}
			}
			else
			{
				List<T> copy = new List<T>(items);
				Shuffle(copy);

				if (sampleSize == items.Count)
					return copy;

				for (int i = 0; i < sampleSize; ++i)
				{
					result.Add(copy[i]);
				}
			}

			return result;
		}

		/// <summary>
		/// Samples a discrete distribution at random.
		/// </summary>
		/// <param name="distribution">A distribution (elements and their corresponding probabilities)</param>
		/// <param name="sampleSize">The number of items to be returned in the sample</param>
		/// <param name="preSorted">Indicates whether the distribution is already sorted by ascending probability</param>
		/// <returns>A list of items of length sampleSize</returns>
		public static List<T> Sample<T>(IList<Tuple<T, double>> distribution, int sampleSize, bool preSorted)
		{
			if (sampleSize <= 0)
				throw new ArgumentException("Invalid sample size.");
			else if (distribution.Count == 0)
				throw new ArgumentException("The provided collection cannot be empty.");

			IList<Tuple<T, double>> orderedDistribution = null;

			if (preSorted)
			{
				orderedDistribution = distribution;
			}
			else
			{
				orderedDistribution = new List<Tuple<T, double>>(distribution);
				((List<Tuple<T, double>>)orderedDistribution).Sort((a, b) => a.Item2.CompareTo(b.Item2));
			}

			CheckThreadLocalMember();

			List<T> result = new List<T>();
			for (int i = 0; i < sampleSize; ++i)
			{
				double random = Local.NextDouble();
				double sum = 0;

				foreach (var entry in orderedDistribution)
				{
					sum += entry.Item2;
					if (sum > random)
					{
						result.Add(entry.Item1);
						break;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Randomly shuffles a collection in place using the Fisher-Yates algorithm.
		/// </summary>
		public static void Shuffle<T>(IList<T> items)
		{
			if (items.Count > 1)
			{
				CheckThreadLocalMember();

				for (int i = items.Count - 1; i >= 1; --i)
				{
					int j = Local.Next(i + 1);
					T temp = items[i];
					items[i] = items[j];
					items[j] = temp;
				}
			}
		}

#region Helper methods
		private static void CheckThreadLocalMember()
		{
			if (Local == null)
			{
				int seed;
				lock(Global)
				{
					seed = Global.Next();
				}

				Local = new Random(seed);
			}
		}
#endregion
	}

}
