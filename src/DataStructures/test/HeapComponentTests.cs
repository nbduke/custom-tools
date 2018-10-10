using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.DataStructures;

namespace Test
{
	[TestClass]
	public class HeapComponentTests
	{
		[TestMethod]
		public void TestPush()
		{
			Heap<int> heap = new Heap<int>();
			int amountToPush = 10;

			for (int i = 0; i < amountToPush; ++i)
			{
				heap.Push(i);
			}

			Assert.AreEqual(amountToPush, heap.Count);
		}

		[TestMethod]
		public void TestPop()
		{
			Heap<int> heap = new Heap<int>();
			int amountToPush = 10;

			for (int i = 0; i < amountToPush; ++i)
			{
				heap.Push(i);
			}

			for (int i = 0; i < amountToPush; ++i)
			{
				heap.Pop();
			}

			Assert.AreEqual(0, heap.Count);
		}

		[TestMethod]
		public void TestMinFirstOrdering()
		{
			Heap<int> heap = new Heap<int>();
			int amountToPush = 10;
			List<int> items = new List<int>(amountToPush);
			Random rand = new Random();

			for (int i = 0; i < amountToPush; ++i)
			{
				int item = rand.Next(101);
				items.Add(item);
				heap.Push(item);
			}

			items.Sort();

			Assert.AreEqual(items[0], heap.Top);

			for (int i = 0; i < amountToPush; ++i)
			{
				int removed = heap.Pop();
				Assert.AreEqual(items[i], removed);
			}
		}

		[TestMethod]
		public void TestMaxFirstOrdering()
		{
			Heap<int> heap = new Heap<int>((a, b) => b - a);
			int amountToPush = 10;
			List<int> items = new List<int>(amountToPush);
			Random rand = new Random();

			for (int i = 0; i < amountToPush; ++i)
			{
				int item = rand.Next(101);
				items.Add(item);
				heap.Push(item);
			}

			items.Sort();

			Assert.AreEqual(items[amountToPush - 1], heap.Top);

			for (int i = amountToPush - 1; i >= 0; --i)
			{
				int removed = heap.Pop();
				Assert.AreEqual(items[i], removed);
			}
		}
	}
}
