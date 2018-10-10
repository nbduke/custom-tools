using System;
using System.Collections;
using System.Collections.Generic;

namespace Tools.DataStructures {

	/// <summary>
	/// A heap where the top element is the least in the partial ordering
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

		public Heap()
			: this(Comparer<T>.Default)
		{
		}

		public Heap(Comparison<T> comparison)
		{
			Validate.IsNotNull(comparison, "comparison");
			Compare = comparison;
			Data = new List<T>();
		}

		public Heap(IComparer<T> comparer)
		{
			Validate.IsNotNull(comparer, "comparer");
			Compare = comparer.Compare;
			Data = new List<T>();
		}

		public void Push(T item)
		{
			Data.Add(item);
			HeapifyBottomUp(Count - 1);
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
