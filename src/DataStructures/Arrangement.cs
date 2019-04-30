using System;
using System.Collections.Generic;

namespace Tools.DataStructures {

	/// <summary>
	/// Wraps a collection of items and exposes methods for iterating over
	/// permutations or combinations of them.
	/// </summary>
	public class Arrangement<T>
	{
		private readonly IReadOnlyList<T> DataSource;

		public Arrangement(IEnumerable<T> dataSource)
		{
			Validate.IsNotNull(dataSource, "dataSource");
			DataSource = new List<T>(dataSource);
		}

		public Arrangement(IReadOnlyList<T> dataSource)
		{
			Validate.IsNotNull(dataSource, "dataSource");
			DataSource = dataSource;
		}

		public int Count
		{
			get { return DataSource.Count; }
		}

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

		public IEnumerable<List<T>> GetCombinations(uint size)
		{
			return GetCombinations(size, size);
		}

		public IEnumerable<List<T>> GetCombinations(uint minimumSize, uint maximumSize)
		{
			var enumerator = new ArrangementEnumerator<T>(
				DataSource,
				minimumSize,
				maximumSize,
				false /*isPermutation*/);
			return enumerator.Iterate();
		}

		public IEnumerable<List<T>> GetPermutations()
		{
			return GetPermutations((uint)DataSource.Count);
		}

		public IEnumerable<List<T>> GetPermutations(uint size)
		{
			return GetPermutations(size, size);
		}

		public IEnumerable<List<T>> GetPermutations(uint minimumSize, uint maximumSize)
		{
			var enumerator = new ArrangementEnumerator<T>(
				DataSource,
				minimumSize,
				maximumSize,
				true /*isPermutation*/);
			return enumerator.Iterate();
		}
	}
}
