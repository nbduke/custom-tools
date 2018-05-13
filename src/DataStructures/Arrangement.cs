using System;
using System.Collections.Generic;

namespace Tools.DataStructures {

	/// <summary>
	/// Wraps a collection of items and exposes methods for iterating over
	/// permutations or combinations of them.
	/// </summary>
	public class Arrangement<T>
	{
		private readonly List<T> Collection;

		public Arrangement(IEnumerable<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			Collection = new List<T>(collection);
		}

		public int Count
		{
			get { return Collection.Count; }
		}

		public IEnumerable<Tuple<T, T>> GetPairs()
		{
			for (int i = 0; i < Collection.Count - 1; ++i)
			{
				for (int j = i + 1; j < Collection.Count; ++j)
				{
					yield return Tuple.Create(Collection[i], Collection[j]);
				}
			}
		}

		public IEnumerable<Tuple<T, T>> GetOrderedPairs()
		{
			for (int i = 0; i < Collection.Count; ++i)
			{
				for (int j = 0; j < Collection.Count; ++j)
				{
					if (i != j)
						yield return Tuple.Create(Collection[i], Collection[j]);
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
				Collection,
				minimumSize,
				maximumSize,
				false /*isPermutation*/);
			return enumerator.Iterate();
		}

		public IEnumerable<List<T>> GetPermutations()
		{
			return GetPermutations((uint)Collection.Count);
		}

		public IEnumerable<List<T>> GetPermutations(uint size)
		{
			return GetPermutations(size, size);
		}

		public IEnumerable<List<T>> GetPermutations(uint minimumSize, uint maximumSize)
		{
			var enumerator = new ArrangementEnumerator<T>(
				Collection,
				minimumSize,
				maximumSize,
				true /*isPermutation*/);
			return enumerator.Iterate();
		}
	}
}
