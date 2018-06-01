using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tools.DataStructures {

	/// <summary>
	/// Specifies the partial ordering of elements in a heap.
	/// </summary>
	public enum HeapOrder
	{
		MinFirst,
		MaxFirst
	}

	/// <summary>
	/// A priority-based heap in which each node has at most two children.
	/// </summary>
	public class BinaryHeap<T> : IEnumerable<T>
	{
		/// <summary>
		/// A tuple consisting of an element in the heap and its real-valued priority.
		/// </summary>
		public class Handle
		{
			public T Value { get; set; }
			public double Priority { get; set; }

			public Handle(T value, double priority)
			{
				Value = value;
				Priority = priority;
			}
		}

		public readonly HeapOrder Order;
		public int Count
		{
			get { return Data.Count; }
		}
		public Handle Top
		{
			get { return Data[0]; }
		}

		private readonly List<Handle> Data;

		public static BinaryHeap<T> CreateMinFirstHeap()
		{
			return new BinaryHeap<T>(HeapOrder.MinFirst);
		}

		public static BinaryHeap<T> CreateMaxFirstHeap()
		{
			return new BinaryHeap<T>(HeapOrder.MaxFirst);
		}

		private BinaryHeap(HeapOrder order)
		{
			Order = order;
			Data = new List<Handle>();
		}

		public void Enqueue(T item, double priority)
		{
			Data.Add(new Handle(item, priority));
			HeapifyBottomUp(Count - 1);
		}

		public Handle Dequeue()
		{
			Handle result = Data[0];
			Swap(0, Count - 1);
			Data.RemoveAt(Count - 1);

			HeapifyTopDown(0);

			return result;
		}

		public void Clear()
		{
			Data.Clear();
		}

		public bool TryGetPriority(T item, ref double priority)
		{
			foreach (Handle handle in Data)
			{
				if (handle.Value.Equals(item))
				{
					priority = handle.Priority;
					return true;
				}
			}

			return false;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return Data.Select(handle => handle.Value).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/*
		 * Enforces the heap property starting from the given node and
		 * propagating toward the root.
		 */
		private void HeapifyBottomUp(int nodeIndex)
		{
			if (nodeIndex <= 0)
				return;

			int parentIndex = (int)System.Math.Floor((nodeIndex - 1) / 2.0);
			if (!InOrder(parentIndex, nodeIndex))
			{
				Swap(parentIndex, nodeIndex);
				HeapifyBottomUp(parentIndex);
			}
		}

		/*
		 * Enforces the heap property starting from the given node and
		 * propagating toward the leaves.
		 */
		private void HeapifyTopDown(int nodeIndex)
		{
			int leftChildIndex = nodeIndex * 2 + 1;
			int rightChildIndex = leftChildIndex + 1;

			if (leftChildIndex >= Count)
				return;

			int inOrderChildIndex;
			if (rightChildIndex >= Count)
				inOrderChildIndex = leftChildIndex;
			else
				inOrderChildIndex = InOrder(leftChildIndex, rightChildIndex) ? leftChildIndex : rightChildIndex;

			if (!InOrder(nodeIndex, inOrderChildIndex))
			{
				Swap(nodeIndex, inOrderChildIndex);
				HeapifyTopDown(inOrderChildIndex);
			}
		}

		/*
		 * Determines if the node at index p comes at or before the node at index q,
		 * according to the heap's partial ordering.
		 */
		private bool InOrder(int p, int q)
		{
			double firstPriority = Data[p].Priority;
			double secondPriority = Data[q].Priority;

			return (Order == HeapOrder.MinFirst && firstPriority <= secondPriority) ||
				   (Order == HeapOrder.MaxFirst && firstPriority >= secondPriority);
		}

		private void Swap(int p, int q)
		{
			var temp = Data[p];
			Data[p] = Data[q];
			Data[q] = temp;
		}
	}

}
