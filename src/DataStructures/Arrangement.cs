using System;
using System.Collections.Generic;

using Tools.Math;

namespace Tools.DataStructures {

	/// <summary>
	/// Wraps a collection of items and exposes methods for iterating over
	/// permutations or combinations of them.
	/// </summary>
	public class Arrangement<T>
	{
		private readonly IReadOnlyList<T> DataSource;

		/// <summary>
		/// Creates an Arrangement from data copied from an enumerable source.
		/// </summary>
		/// <param name="dataSource">the data source</param>
		public Arrangement(IEnumerable<T> dataSource)
		{
			Validate.IsNotNull(dataSource, "dataSource");
			DataSource = new List<T>(dataSource);
		}

		/// <summary>
		/// Creates an Arrangement in-place from a read-only data source.
		/// </summary>
		/// <param name="dataSource">the data source</param>
		public Arrangement(IReadOnlyList<T> dataSource)
		{
			Validate.IsNotNull(dataSource, "dataSource");
			DataSource = dataSource;
		}

		public int Count
		{
			get { return DataSource.Count; }
		}

		/// <summary>
		/// Returns an enumerable of unordered pairs of items.
		/// </summary>
		public IEnumerable<Tuple<T, T>> GetPairs()
		{
			for (int i = 0; i < DataSource.Count - 1; ++i)
			{
				for (int j = i + 1; j < DataSource.Count; ++j)
				{
					yield return Tuple.Create(DataSource[i], DataSource[j]);
				}
			}
		}

		/// <summary>
		/// Returns an enumerable of ordered pairs of items.
		/// </summary>
		public IEnumerable<Tuple<T, T>> GetOrderedPairs()
		{
			for (int i = 0; i < DataSource.Count; ++i)
			{
				for (int j = 0; j < DataSource.Count; ++j)
				{
					if (i != j)
						yield return Tuple.Create(DataSource[i], DataSource[j]);
				}
			}
		}

		/// <summary>
		/// Returns an enumerable of combinations of a fixed size.
		/// </summary>
		/// <param name="size">the size of combinations to return</param>
		public IEnumerable<List<T>> GetCombinations(uint size)
		{
			return GetCombinations(size, size);
		}

		/// <summary>
		/// Returns an enumerable of combinations whose sizes fall within a given range.
		/// </summary>
		/// <param name="minimumSize">the minimum combination size</param>
		/// <param name="maximumSize">the maximum combination size</param>
		public IEnumerable<List<T>> GetCombinations(uint minimumSize, uint maximumSize)
		{
			var enumerator = new ArrangementEnumerator<T>(
				DataSource,
				minimumSize,
				maximumSize,
				false /*isPermutation*/
			);
			return enumerator.Iterate();
		}

		/// <summary>
		/// Returns an enumerable of permutations of the entire data source.
		/// </summary>
		public IEnumerable<List<T>> GetPermutations()
		{
			return GetPermutations((uint)DataSource.Count);
		}

		/// <summary>
		/// Returns an enumerable of permutations of a fixed size.
		/// </summary>
		/// <param name="size">the size of permutations to return</param>
		public IEnumerable<List<T>> GetPermutations(uint size)
		{
			return GetPermutations(size, size);
		}

		/// <summary>
		/// Returns an enumerable of permutations whose sizes fall within a given range.
		/// </summary>
		/// <param name="minimumSize">the minimum permutation size</param>
		/// <param name="maximumSize">the maximum permutation size</param>
		public IEnumerable<List<T>> GetPermutations(uint minimumSize, uint maximumSize)
		{
			var enumerator = new ArrangementEnumerator<T>(
				DataSource,
				minimumSize,
				maximumSize,
				true /*isPermutation*/
			);
			return enumerator.Iterate();
		}

		/// <summary>
		/// Returns the Cartesian product of this and another Arrangement.
		/// </summary>
		public IEnumerable<Tuple<T, U>> Product<U>(Arrangement<U> other)
		{
			Validate.IsNotNull(other, "other");
			return Combinatorics.CartesianProduct(DataSource, other.DataSource);
		}
	}
}
