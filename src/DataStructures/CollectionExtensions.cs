﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.DataStructures {

	/// <summary>
	/// Contains useful extension methods for collection data types.
	/// </summary>
	public static class CollectionExtensions
	{
		#region Max
		public static T Max<T>(this IEnumerable<T> items, IComparer<T> comparer)
		{
			return Max(items, (a, b) => comparer.Compare(a, b) < 0);
		}

		public static T Max<T>(this IEnumerable<T> items, Func<T, T, bool> lessThan)
		{
			Validate.IsNotNull(lessThan, "lessThan");

			T max = items.First();
			foreach (T item in items.Skip(1))
			{
				if (lessThan(max, item))
					max = item;
			}

			return max;
		}

		/// <summary>
		/// Returns the index of the maximum-valued item in a collection.
		/// </summary>
		/// <param name="items">the collection</param>
		public static int ArgMax<T>(this IEnumerable<T> items) where T : IComparable, IComparable<T>
		{
			return ArgMax(items, (a, b) => a.CompareTo(b) < 0);
		}

		/// <summary>
		/// Returns the index of the maximum-valued item in a collection.
		/// </summary>
		/// <param name="items">the collection</param>
		public static int ArgMax<T>(this IEnumerable<T> items, IComparer<T> comparer)
		{
			return ArgMax(items, (a, b) => comparer.Compare(a, b) < 0);
		}

		/// <summary>
		/// Returns the index of the maximum-valued item in a collection.
		/// </summary>
		/// <param name="items">the collection</param>
		public static int ArgMax<T>(this IEnumerable<T> items, Func<T, T, bool> lessThan)
		{
			Validate.IsNotNull(lessThan, "lessThan");

			int index = 0;
			int argMax = index;
			T max = items.First();

			foreach (T item in items.Skip(1))
			{
				++index;
				if (lessThan(max, item))
				{
					max = item;
					argMax = index;
				}
			}

			return argMax;
		}
		#endregion

		#region Min
		public static T Min<T>(this IEnumerable<T> items, IComparer<T> comparer)
		{
			return Min(items, (a, b) => comparer.Compare(a, b) < 0);
		}

		public static T Min<T>(this IEnumerable<T> items, Func<T, T, bool> lessThan)
		{
			Validate.IsNotNull(lessThan, "lessThan");

			T min = items.First();
			foreach (T item in items.Skip(1))
			{
				if (lessThan(item, min))
					min = item;
			}

			return min;
		}

		/// <summary>
		/// Returns the index of the minimum-valued item in a collection.
		/// </summary>
		/// <param name="items">the collection</param>
		public static int ArgMin<T>(this IEnumerable<T> items) where T : IComparable, IComparable<T>
		{
			return ArgMin(items, (a, b) => a.CompareTo(b) < 0);
		}

		/// <summary>
		/// Returns the index of the minimum-valued item in a collection.
		/// </summary>
		/// <param name="items">the collection</param>
		public static int ArgMin<T>(this IEnumerable<T> items, IComparer<T> comparer)
		{
			return ArgMin(items, (a, b) => comparer.Compare(a, b) < 0);
		}

		/// <summary>
		/// Returns the index of the minimum-valued item in a collection.
		/// </summary>
		/// <param name="items">the collection</param>
		public static int ArgMin<T>(this IEnumerable<T> items, Func<T, T, bool> lessThan)
		{
			Validate.IsNotNull(lessThan, "lessThan");

			int index = 0;
			int argMin = index;
			T min = items.First();

			foreach (T item in items.Skip(1))
			{
				++index;
				if (lessThan(item, min))
				{
					min = item;
					argMin = index;
				}
			}

			return argMin;
		}
		#endregion

		#region Generic IEnumerable
		public static IEnumerable<T> Iterate<T>(this IEnumerator<T> enumerator)
		{
			while (enumerator.MoveNext())
			{
				yield return enumerator.Current;
			}
		}
		#endregion
	}

}
