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
		private readonly int? MaxCapacity;

		/// <summary>
		/// Constructs a heap with the default comparer for T.
		/// </summary>
		/// <param name="maxCapacity">(optional) specifies the capacity at which
		/// pushing new items will cause the least item to be evicted</param>
		public Heap(int? maxCapacity = null)
			: this(Comparer<T>.Default, maxCapacity)
		{
		}

		/// <summary>
		/// Constructs a heap whose items are ordered by a comparison function.
		/// </summary>
		/// <param name="comparison">the comparison function</param>
		/// <param name="maxCapacity">(optional) specifies the capacity at which
		/// pushing new items will cause the least item to be evicted</param>
		public Heap(Comparison<T> comparison, int? maxCapacity = null)
		{
			Validate.IsNotNull(comparison, "comparison");
			Compare = comparison;
			MaxCapacity = maxCapacity;
			Data = new List<T>();
		}

		/// <summary>
		/// Constructs a heap whose items are ordered by a comparer object.
		/// </summary>
		/// <param name="comparer">the comparer object</param>
		/// <param name="maxCapacity">(optional) specifies the capacity at which
		/// pushing new items will cause the least item to be evicted</param>
		public Heap(IComparer<T> comparer, int? maxCapacity = null)
		{
			Validate.IsNotNull(comparer, "comparer");
			Compare = comparer.Compare;
			MaxCapacity = maxCapacity;
			Data = new List<T>();
		}

		/// <summary>
		/// Pushes a new item onto the heap. If the heap is at its maximum capacity,
		/// the least item after pushing will be evicted.
		/// </summary>
		/// <param name="item">the item</param>
		public void Push(T item)
		{
			Data.Add(item);
			HeapifyBottomUp(Count - 1);

			if (MaxCapacity.HasValue && Count > MaxCapacity)
				Pop();
		}

		public T Pop()
		{
			T result = Top;
			Swap(0, Count - 1);
			Data.RemoveAt(Count - 1);

			HeapifyTopDown(0);

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
		 * Enforces the heap property starting from the given index and
		 * propagating toward the root.
		 */
		private void HeapifyBottomUp(int index)
		{
			if (index <= 0)
				return;

			int parentIndex = (int)System.Math.Floor((index - 1) / 2.0);
			if (!InOrder(parentIndex, index))
			{
				Swap(parentIndex, index);
				HeapifyBottomUp(parentIndex);
			}
		}

		/*
		 * Enforces the heap property starting from the given index and
		 * propagating toward the leaves.
		 */
		private void HeapifyTopDown(int index)
		{
			int leftChildIndex = index * 2 + 1;
			int rightChildIndex = leftChildIndex + 1;

			if (leftChildIndex >= Count)
				return;

			int inOrderChildIndex;
			if (rightChildIndex >= Count)
				inOrderChildIndex = leftChildIndex;
			else
				inOrderChildIndex = InOrder(leftChildIndex, rightChildIndex) ? leftChildIndex : rightChildIndex;

			if (!InOrder(index, inOrderChildIndex))
			{
				Swap(index, inOrderChildIndex);
				HeapifyTopDown(inOrderChildIndex);
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
			var temp = Data[p];
			Data[p] = Data[q];
			Data[q] = temp;
		}
	}

}
