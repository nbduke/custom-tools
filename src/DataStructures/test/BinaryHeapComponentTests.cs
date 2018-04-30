using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.DataStructures;

namespace Test
{
	[TestClass]
	public class BinaryHeapComponentTests
	{
		[TestMethod]
		public void TestEnqueue()
		{
			BinaryHeap<int> heap = BinaryHeap<int>.CreateMinFirstHeap();
			int amountToEnqueue = 10;

			for (int i = 0; i < amountToEnqueue; ++i)
			{
				heap.Enqueue(i, i);
			}

			Assert.AreEqual(amountToEnqueue, heap.Count);
		}

		[TestMethod]
		public void TestDequeue()
		{
			BinaryHeap<int> heap = BinaryHeap<int>.CreateMinFirstHeap();
			int amountToEnqueue = 10;

			for (int i = 0; i < amountToEnqueue; ++i)
			{
				heap.Enqueue(i, i);
			}

			for (int i = 0; i < amountToEnqueue; ++i)
			{
				var removed = heap.Dequeue();
			}

			Assert.AreEqual(0, heap.Count);
		}

		[TestMethod]
		public void TestMinFirstOrdering()
		{
			BinaryHeap<int> heap = BinaryHeap<int>.CreateMinFirstHeap();
			int amountToEnqueue = 10;
			List<int> items = new List<int>(amountToEnqueue);
			Random rand = new Random();

			for (int i = 0; i < amountToEnqueue; ++i)
			{
				int itemAndPriority = rand.Next(101);
				items.Add(itemAndPriority);
				heap.Enqueue(itemAndPriority, itemAndPriority);
			}

			items.Sort();

			Assert.AreEqual(items[0], heap.Top.Value);

			for (int i = 0; i < amountToEnqueue; ++i)
			{
				var removed = heap.Dequeue();
				Assert.AreEqual(items[i], removed.Value);
			}
		}

		[TestMethod]
		public void TestMaxFirstOrdering()
		{
			BinaryHeap<int> heap = BinaryHeap<int>.CreateMaxFirstHeap();
			int amountToEnqueue = 10;
			List<int> items = new List<int>(amountToEnqueue);
			Random rand = new Random();

			for (int i = 0; i < amountToEnqueue; ++i)
			{
				int itemAndPriority = rand.Next(101);
				items.Add(itemAndPriority);
				heap.Enqueue(itemAndPriority, itemAndPriority);
			}

			items.Sort();

			Assert.AreEqual(items[amountToEnqueue - 1], heap.Top.Value);

			for (int i = amountToEnqueue - 1; i >= 0; --i)
			{
				var removed = heap.Dequeue();
				Assert.AreEqual(items[i], removed.Value);
			}
		}
	}
}
