/*
 * BinaryHeap.cs
 * 
 * Nathan Duke
 * 1/31/15
 * 
 * Contains the BinaryHeap<T> class and the HeapOrder enum.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CommonTools { namespace DataStructures {

	// Specifies the partial ordering of elements in a heap. This
	// determines the element at the top of the heap.
	public enum HeapOrder
	{
		MinFirst,
		MaxFirst
	}

	// A heap in which each node has at most two children. The heap is implemented as
	// an array with the Top at index 0. The heap order is configurable at construction.
	public class BinaryHeap<T> : IEnumerable<T>
	{
#region Support classes

		// A tuple consisting of an element in the heap and its real-valued priority.
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

#endregion


		public int Count { get { return Data.Count; } }
		public HeapOrder DataOrder { get; private set; }
		public Handle Top
		{
			get { return Data[0]; }
		}

		private List<Handle> Data { get; set; }


		static public BinaryHeap<T> CreateMinFirstHeap()
		{
			return new BinaryHeap<T>(HeapOrder.MinFirst);
		}

		static public BinaryHeap<T> CreateMaxFirstHeap()
		{
			return new BinaryHeap<T>(HeapOrder.MaxFirst);
		}

		private BinaryHeap(HeapOrder dataOrder)
		{
			DataOrder = dataOrder;
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

		public bool Contains(T item)
		{
			foreach (Handle datum in Data)
			{
				if (datum.Value.Equals(item))
					return true;
			}

			return false;
		}

		public bool TryGetPriority(T item, ref double priority)
		{
			foreach (Handle datum in Data)
			{
				if (datum.Value.Equals(item))
				{
					priority = datum.Priority;
					return true;
				}
			}

			return false;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return Data.Select(handle => handle.Value).GetEnumerator();
		}

#region Helper methods

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Enforces the heap property starting from the given node and
		// propagating toward the root.
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

		// Enforces the heap property starting from the given node and
		// propagating toward the leaves.
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

		// Uses DataOrder to determine if the node at index p comes at or before the node
		// at index r according to this heap's partial ordering.
		private bool InOrder(int p, int q)
		{
			double firstPriority = Data[p].Priority;
			double secondPriority = Data[q].Priority;

			return (DataOrder == HeapOrder.MinFirst && firstPriority <= secondPriority) ||
				   (DataOrder == HeapOrder.MaxFirst && firstPriority >= secondPriority);
		}

		// Swaps two nodes in the heap.
		private void Swap(int p, int q)
		{
			var temp = Data[p];
			Data[p] = Data[q];
			Data[q] = temp;
		}

#endregion
	}

}}
