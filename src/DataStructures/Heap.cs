using System;
using System.Collections;
using System.Collections.Generic;

namespace Tools.DataStructures {

	/// <summary>
	/// A heap where the top item is the least in the partial ordering
	/// specified by the heap's comparator.
	/// </summary>
	/// <remarks>
	/// The heap is represented as a binary tree embedded in an array.
	/// </remarks>
	public class Heap<T> : IEnumerable<T>
	{
		public int Count
		{
			get { return Data.Count; }
		}
		public T Top
		{
			get { return Data[0]; }
		}

		private readonly List<T> Data;
		private readonly Comparison<T> Compare;
		private readonly int MaxCapacity;

		/// <summary>
		/// Constructs a heap with the default comparer for T.
		/// </summary>
		/// <param name="maxCapacity">(optional) specifies the capacity at which
		/// pushing new items will cause the least item to be evicted</param>
		public Heap(int? maxCapacity = null)
		{
			MaxCapacity = maxCapacity.HasValue ? maxCapacity.Value : int.MaxValue;
			Compare = Comparer<T>.Default.Compare;
			Data = new List<T>();
		}

		/// <summary>
		/// Constructs a heap whose items are ordered by the default comparer for
		/// their keys.
		/// </summary>
		/// <param name="keySelector">a function that returns a comparable key for an item</param>
		/// <param name="maxCapacity">(optional) specifies the capacity at which
		/// pushing new items will cause the least item to be evicted</param>
		public Heap(Func<T, IComparable> keySelector, int? maxCapacity = null)
			: this(maxCapacity)
		{
			Validate.IsNotNull(keySelector, "keySelector");
			Compare = (a, b) => keySelector(a).CompareTo(keySelector(b));
		}

		/// <summary>
		/// Constructs a heap whose items are ordered by a comparison function.
		/// </summary>
		/// <param name="comparison">the comparison function</param>
		/// <param name="maxCapacity">(optional) specifies the capacity at which
		/// pushing new items will cause the least item to be evicted</param>
		public Heap(Comparison<T> comparison, int? maxCapacity = null)
			: this(maxCapacity)
		{
			Validate.IsNotNull(comparison, "comparison");
			Compare = comparison;
		}

		/// <summary>
		/// Pushes a new item onto the heap. If the heap is at its maximum capacity,
		/// the least item after pushing will be evicted.
		/// </summary>
		/// <param name="item">the item</param>
		public void Push(T item)
		{
			Data.Add(item);
			HeapifyBottomUp();

			if (Count > MaxCapacity)
				Pop();
		}

		public T Pop()
		{
			T result = Top;
			Swap(0, Count - 1);
			Data.RemoveAt(Count - 1);
			HeapifyTopDown();
			return result;
		}

		public void Clear()
		{
			Data.Clear();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return Data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/*
		 * Enforces the heap property starting at the bottom of the heap and
		 * propagating upward.
		 */
		private void HeapifyBottomUp()
		{
			int current = Count - 1;
			int parent = (current - 1) / 2;

			while (current > 0 && !InOrder(parent, current))
			{
				Swap(parent, current);
				current = parent;
				parent = (current - 1) / 2;
			}
		}

		/*
		 * Enforces the heap property starting from the top of the heap and
		 * propagating downward.
		 */
		private void HeapifyTopDown()
		{
			int current = 0;
			int leftChild = current * 2 + 1;
			int rightChild = leftChild + 1;

			while (leftChild < Count)
			{
				int childToCompare = leftChild;
				if (rightChild < Count && InOrder(rightChild, leftChild))
					childToCompare = rightChild;

				if (!InOrder(current, childToCompare))
				{
					Swap(current, childToCompare);
					current = childToCompare;
					leftChild = current * 2 + 1;
					rightChild = leftChild + 1;
				}
				else
				{
					break;
				}
			}
		}

		/*
		 * Determines if the item at index p comes at or before the item at index q,
		 * according to the heap's partial ordering.
		 */
		private bool InOrder(int p, int q)
		{
			return Compare(Data[p], Data[q]) <= 0;
		}

		private void Swap(int p, int q)
		{
			T temp = Data[p];
			Data[p] = Data[q];
			Data[q] = temp;
		}
	}

}
