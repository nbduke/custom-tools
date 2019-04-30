using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tools.DataStructures {

	/*
	 * Represents an enumerator of permutations or combinations (a.k.a.
	 * arrangements) of a collection. Current points to an arrangement of
	 * items, and each call to MoveNext advances Current to the next
	 * arrangement in lexicographic order. The original collection is the
	 * first (or "sorted") element in the total ordering, and all items are
	 * considered to be distinct.
	*/
	class ArrangementEnumerator<T> : IEnumerator<List<T>>
	{
		private readonly IReadOnlyList<T> DataSource;
		private readonly uint MinSize;
		private readonly uint MaxSize;
		private Stack<int> IndexStack;
		private Stack<IEnumerator<int>> IndexEnumeratorStack;
		private BitArray IsInPermutation;
		private List<T> _Current;

		public ArrangementEnumerator(
			IReadOnlyList<T> dataSource,
			uint minimumSize,
			uint maximumSize,
			bool isPermutation)
		{
			Validate.IsNotNull(dataSource, "dataSource");
			Validate.IsTrue(minimumSize <= maximumSize, "minimumSize cannot exceed maximumSize");
			Validate.IsTrue(maximumSize <= (uint)dataSource.Count,
				"maximumSize cannot exceed the collection size");

			DataSource = dataSource;
			MinSize = minimumSize;
			MaxSize = maximumSize;
			IndexStack = new Stack<int>();
			IndexEnumeratorStack = new Stack<IEnumerator<int>>();

			if (isPermutation)
				IsInPermutation = new BitArray(dataSource.Count);

			Reset();
		}

		public List<T> Current
		{
			get
			{
				if (_Current == null)
					_Current = new List<T>(IndexStack.Reverse().Select(i => DataSource[i]));

				return _Current;
			}
		}

		object IEnumerator.Current
		{
			get { return Current; }
		}

		public bool MoveNext()
		{
			_Current = null;
			return FindNextArrangement();
		}

		public void Reset()
		{
			if (IsInPermutation != null)
				IsInPermutation.SetAll(false);

			IndexStack.Clear();
			IndexEnumeratorStack.Clear();
			IndexEnumeratorStack.Push(GetNextIndices(-1).GetEnumerator());
		}

		public void Dispose()
		{
		}

		private uint CurrentSize
		{
			get { return (uint)IndexStack.Count; }
		}

		/*
		 * The following algorithm is essentially a backtracking search over the
		 * graph of indices of the collection, but it has been modified to be
		 * iterative rather than recursive. The recursive behavior is mimicked by
		 * a stack of indices (representing the current arrangement under
		 * construction) and a stack of enumerators of indices (the next indices to
		 * be generated at each "level" of the algorithm). When an arrangement between
		 * MinSize and MaxSize has been found, the algorithm returns; the next time it
		 * runs, it uses the two stacks to pick up where it left off.
		 */
		private bool FindNextArrangement()
		{
			if (MaxSize == 0)
			{
				return false;
			}
			else if (CurrentSize >= MaxSize)
			{
				IndexStack.Pop();
				IndexEnumeratorStack.Pop();
			}

			while (IndexEnumeratorStack.Count > 0)
			{
				var currentEnumerator = IndexEnumeratorStack.Peek();
				bool increaseDepth = false;

				while (currentEnumerator.MoveNext())
				{
					int currentIndex = currentEnumerator.Current;
					IndexStack.Push(currentIndex);
					IndexEnumeratorStack.Push(GetNextIndices(currentIndex).GetEnumerator());

					if (CurrentSize >= MinSize)
					{
						// We've found the next arrangement
						return true;
					}
					else
					{
						// We need to move on to the next index and continue building from there
						increaseDepth = true;
						break;
					}
				}

				if (!increaseDepth)
				{
					IndexEnumeratorStack.Pop();
					if (IndexStack.Count > 0)
						IndexStack.Pop();
				}
			}

			// There are no more indices to build arrangements with
			return false;
		}

		/*
		 * Returns an enumerable of indices that can follow the current index in
		 * an arrangement.
		 */
		private IEnumerable<int> GetNextIndices(int currentIndex)
		{
			if (IsInPermutation != null)
				return GetNextIndicesForPermutations();
			else
				return GetNextIndicesForCombinations(currentIndex);
		}

		private IEnumerable<int> GetNextIndicesForPermutations()
		{
			for (int i = 0; i < DataSource.Count; ++i)
			{
				if (!IsInPermutation[i])
				{
					IsInPermutation[i] = true;
					yield return i;
					IsInPermutation[i] = false;
				}
			}
		}

		private IEnumerable<int> GetNextIndicesForCombinations(int currentIndex)
		{
			for (int i = currentIndex + 1; i < DataSource.Count; ++i)
			{
				yield return i;
			}
		}
	}

}
