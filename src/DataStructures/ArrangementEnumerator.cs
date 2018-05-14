using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tools.DataStructures {

	/// <summary>
	/// Represents an enumerator of permutations or combinations (a.k.a.
	/// arrangements) of a collection. Current points to an arrangement of
	/// items, and each call to MoveNext advances Current to the next
	/// arrangement in lexicographic order. The original collection is the
	/// first (or "sorted") element in the total ordering, and all items are
	/// considered to be distinct.
	/// </summary>
	class ArrangementEnumerator<T> : IEnumerator<List<T>>
	{
		private readonly List<T> Collection;
		private readonly uint MinSize;
		private readonly uint MaxSize;
		private readonly bool IsPermutation;
		private Stack<int> IndexStack;
		private Stack<IEnumerator<int>> IndexEnumeratorStack;
		private List<T> _Current;

		/// <summary>
		/// Creates an ArrangementEnumerator.
		/// </summary>
		/// <param name="collection">the original collection</param>
		/// <param name="minimumSize">the minimum size of an arrangement</param>
		/// <param name="maximumSize">the maximum size of an arrangement</param>
		/// <param name="isPermutation">if true, enumerate permutations. Otherwise,
		/// enumerate combinations</param>
		public ArrangementEnumerator(
			List<T> collection,
			uint minimumSize,
			uint maximumSize,
			bool isPermutation)
		{
			Validate.IsNotNull(collection, "collection");

			if (minimumSize > maximumSize)
				throw new ArgumentException("minimumSize cannot exceed maximumSize");
			if (maximumSize > (uint)collection.Count)
				throw new ArgumentException("maximumSize cannot exceed the collection size");

			Collection = collection;
			MinSize = minimumSize;
			MaxSize = maximumSize;
			IsPermutation = isPermutation;
			IndexStack = new Stack<int>();
			IndexEnumeratorStack = new Stack<IEnumerator<int>>();

			Reset();
		}

		public List<T> Current
		{
			get
			{
				if (_Current == null)
					_Current = new List<T>(IndexStack.Reverse().Select(i => Collection[i]));

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
				bool moveUpStack = false;

				while (currentEnumerator.MoveNext())
				{
					int currentIndex = currentEnumerator.Current;

					// For permutations, make sure we haven't tried this index yet (combinations
					// never generate redundant indices)
					if (!(IsPermutation && IndexStack.Contains(currentIndex)))
					{
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
							moveUpStack = true;
							break;
						}
					}
				}

				if (!moveUpStack)
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
			// Every index leads to every other index in a permutation, but in a
			// combination, only indices after the current one are reachable.
			int nextIndex = IsPermutation ? 0 : currentIndex + 1;
			for (int i = nextIndex; i < Collection.Count; ++i)
			{
				yield return i;
			}
		}
	}

}
