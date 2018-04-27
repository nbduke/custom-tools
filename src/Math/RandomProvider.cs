/*
 * RandomProvider.cs
 * 
 * Nathan Duke
 * 1/31/15
 * 
 * Contains a set of static methods for performing randomization algorithms
 * in a thread-safe manner.
 */

using System;
using System.Collections.Generic;

namespace CommonTools { namespace Algorithms {

	public static class RandomProvider
	{
		private static readonly Random Global = new Random();

		[ThreadStatic]
		private static Random Local;

		/// <summary>
		/// Returns a random int in the interval [0, |maxValue| - 1].
		/// </summary>
		/// <remarks>
		/// If maxValue is 0, this function returns 0.
		/// </remarks>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static int GetInt(int maxValue = int.MaxValue)
		{
			CheckThreadLocalMember();
			return Local.Next(System.Math.Abs(maxValue));
		}

		/// <summary>
		/// Returns a random int in the interval [minValue, maxValue - 1].
		/// </summary>
		/// <remarks>
		/// minValue must not be greater than maxValue. If they are equal, this function
		/// returns minValue. Either or both parameters may be negative.
		/// </remarks>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static int GetInt(int minValue, int maxValue)
		{
			CheckThreadLocalMember();
			return Local.Next(minValue, maxValue);
		}

		/// <summary>
		/// Returns a random double in the interval [0.0, 1.0).
		/// </summary>
		/// <returns></returns>
		public static double GetUnitDouble()
		{
			CheckThreadLocalMember();
			return Local.NextDouble();
		}

		/// <summary>
		/// Returns a random double in the interval [0.0, |maxValue|).
		/// </summary>
		/// <remarks>
		/// If maxValue is 0, this function returns 0.
		/// </remarks>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static double GetDouble(double maxValue)
		{
			return GetUnitDouble() * System.Math.Abs(maxValue);
		}

		/// <summary>
		/// Returns a random double in the interval [minValue, maxValue).
		/// </summary>
		/// <remarks>
		/// minValue must not be greater than maxValue. If they are equal, this function
		/// returns minValue. Either or both parameters may be negative.
		/// </remarks>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static double GetDouble(double minValue, double maxValue)
		{
			if (minValue > maxValue)
				throw new ArgumentException("minValue may not exceed maxValue.");

			double diff = maxValue - minValue;
			return GetUnitDouble() * diff + minValue;
		}

		/// <summary>
		/// Flips a weighted coin. Returns true with probability equal to the given weight.
		/// </summary>
		/// <param name="weight"></param>
		/// <returns></returns>
		public static bool FlipCoin(double weight = 0.5)
		{
			return GetUnitDouble() < weight;
		}

		/// <summary>
		/// Returns an element selected uniformly at random from the given collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
		public static T Select<T>(IList<T> items)
		{
			if (items.Count == 0)
				throw new ArgumentException("The provided collection cannot be empty.");

			return items[GetInt(items.Count)];
		}

		/// <summary>
		/// Samples the given collection uniformly at random.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items">A collection of type T</param>
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
		/// Samples the given discrete distribution at random.
		/// </summary>
		/// <typeparam name="T"></typeparam>
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
		/// Randomly shuffles the given collection in place. This function
		/// implements the Fisher-Yates random shuffle algorithm.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
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

}}
