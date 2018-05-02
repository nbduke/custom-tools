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

#region Helper methods
		/*
		 * If we view the items of a collection as nodes in a complete graph, then nPr can be
		 * expressed as building all possible paths of lengths n through r in the graph. nCr can
		 * be expressed similarly, but with fewer edges than a complete graph.
		 * 
		 * We can therefore use FlexibleBacktrackingSearch to generate and search the graph
		 * over the indices in Items in order to calculate combinations and permutations.
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
			ChildGenerator<int> getChildIndices = (currentIndex) =>
			{
				return GetChildIndices(currentIndex, doPermutations);
			};

			NodeAction<int> processNode = (node) =>
			{
				if (node.CumulativePathLength >= minimumSize)
				{
					// Record the valid combination/permutation
					action(GetItemsFromIndexPath(node.GetPath()));
				}

				return NodeOption.Continue;
			};

			// Due to the order of index generation for combinations, we can skip
			// checking whether each index is already in the path.
			var fbs = new FlexibleBacktrackingSearch<int>(
				getChildIndices,
				!doPermutations /*assumeChildrenNotInPath*/);

			// Run FlexibleBacktrackingSearch starting from every position in Items
			for (int i = 0; i < Items.Count; ++i)
			{
				fbs.Search(i, processNode, maximumSize);
			}
		}

		private IEnumerable<int> GetChildIndices(int currentIndex, bool doPermutations)
		{
			// Every index leads to every other index in a permutation (complete graph),
			// but in a combination, only indices after the current one are reachable.
			int nextIndex = doPermutations ? 0 : currentIndex + 1;
			for (int i = nextIndex; i < Items.Count; ++i)
			{
				yield return i;
			}
		}

		private IEnumerable<T> GetItemsFromIndexPath(IEnumerable<int> indexPath)
		{
			foreach (int index in indexPath)
			{
				yield return Items[index];
			}
		}
	}
#endregion

}
