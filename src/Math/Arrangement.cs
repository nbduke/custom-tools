using System;
using System.Collections.Generic;

using Tools.Algorithms.Search;

namespace Tools.Algorithms {

	/*
	 * Arrangement wraps a collection of items and provides methods
	 * for computing combinations and permutations on them.
	 */
	public class Arrangement<T>
	{
		private readonly List<T> Items;

		public Arrangement(IEnumerable<T> items)
		{
			if (items == null)
				throw new ArgumentNullException("items");

			Items = new List<T>(items);
		}

		public int Count
		{
			get { return Items.Count; }
		}

		public IEnumerable<Tuple<T, T>> GetPairs()
		{
			for (int i = 0; i < Items.Count - 1; ++i)
			{
				for (int j = i + 1; j < Items.Count; ++j)
				{
					yield return new Tuple<T, T>(Items[i], Items[j]);
				}
			}
		}

		public IEnumerable<Tuple<T, T>> GetOrderedPairs()
		{
			for (int i = 0; i < Items.Count; ++i)
			{
				for (int j = 0; j < Items.Count; ++j)
				{
					if (i != j)
						yield return new Tuple<T, T>(Items[i], Items[j]);
				}
			}
		}

		public List<List<T>> GetCombinations(uint size)
		{
			return GetCombinations(size, size);
		}

		public List<List<T>> GetCombinations(uint minimumSize, uint maximumSize)
		{
			var result = new List<List<T>>();
			ForEachCombination(minimumSize, maximumSize, combo => result.Add(new List<T>(combo)));
			return result;
		}

		public void ForEachCombination(uint size, Action<IEnumerable<T>> action)
		{
			ForEachCombination(size, size, action);
		}

		public void ForEachCombination(uint minimumSize, uint maximumSize, Action<IEnumerable<T>> action)
		{
			CoreAlgorithm(minimumSize, maximumSize, action, false /*doPermutations*/);
		}

		public List<List<T>> GetPermutations()
		{
			return GetPermutations((uint)Items.Count);
		}

		public List<List<T>> GetPermutations(uint size)
		{
			return GetPermutations(size, size);
		}

		public List<List<T>> GetPermutations(uint minimumSize, uint maximumSize)
		{
			var result = new List<List<T>>();
			ForEachPermutation(minimumSize, maximumSize, combo => result.Add(new List<T>(combo)));
			return result;
		}

		public void ForEachPermutation(uint size, Action<IEnumerable<T>> action)
		{
			ForEachPermutation(size, size, action);
		}

		public void ForEachPermutation(uint minimumSize, uint maximumSize, Action<IEnumerable<T>> action)
		{
			CoreAlgorithm(minimumSize, maximumSize, action, true /*doPermutations*/);
		}

		/*
		 * Uses the FlexibleBacktrackingSearch algorithm to construct combinations or permutations
		 * of a collection. Each combination/permutation is passed to action for processing.
		 */
		private void CoreAlgorithm(
			uint minimumSize,
			uint maximumSize,
			Action<IEnumerable<T>> action,
			bool doPermutations)
		{
			if (maximumSize > Items.Count)
				throw new ArgumentException("The maximum size cannot exceed the number of items in the arrangement.");
			else if (minimumSize > maximumSize)
				throw new ArgumentException("The minimum size cannot exceed the maximum size.");

			// Create functors used by FlexibleBacktrackingSearch
			GoalTest<SourceIndex> isValidCombination =
				(node) =>
				{
					return node.CumulativePathLength >= minimumSize;
				};

			ChildEnumerator<SourceIndex> getNextIndices =
				(node) =>
				{
					return new IndexEnumerator(Items.Count - 1, node);
				};

			GoalAction<SourceIndex> processCombination =
				(node) =>
				{
					action(node.GetOriginals(Items));
					return GoalOption.Continue;
				};

			var fbs = new FlexibleBacktrackingSearch<SourceIndex>(isValidCombination, getNextIndices, processCombination);

			// Run FlexibleBacktrackingSearch starting from every position in Items
			for (int i = 0; i < Items.Count; ++i)
			{
				SourceIndex startOfCombination = new SourceIndex(i, doPermutations);
				fbs.Search(startOfCombination, maximumSize);
			}
		}
	}

}
