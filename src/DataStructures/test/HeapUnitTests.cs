using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.DataStructures;

namespace Test
{
	[TestClass]
	public class HeapUnitTests
	{
		#region Constructor
		[TestMethod]
		public void Constructor_WithNullComparison_ThrowsArgumentNullException()
		{
			// Act
			Action action = () =>
			{
				Comparison<int> c = null;
				var heap = new Heap<int>(c);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}
		#endregion

		#region Count and Push
		[TestMethod]
		public void Count_BeforePushingAnything_ReturnsZero()
		{
			// Arrange
			var heap = new Heap<int>();

			// Act
			int count = heap.Count;

			// Assert
			Assert.AreEqual(0, count);
		}

		[TestMethod]
		public void Count_AfterPushingSomeItems_ReturnsNumberOfItemsPushed()
		{
			// Arrange
			var heap = new Heap<int>();
			int expectedCount = 12;

			for (int i = 0; i < expectedCount; ++i)
			{
				heap.Push(45);
			}

			// Act
			int count = heap.Count;

			// Assert
			Assert.AreEqual(expectedCount, count);
		}

		[TestMethod]
		public void Count_AfterPushingWhenHeapIsAtMaxCapacity_ReturnsMaxCapacity()
		{
			// Arrange
			int maxCapacity = 2;
			var heap = new Heap<int>(maxCapacity);
			heap.Push(1);
			heap.Push(5);

			// Act
			heap.Push(2);
			int count = heap.Count;

			// Assert
			Assert.AreEqual(maxCapacity, count);
		}

		[TestMethod]
		public void Count_AfterPoppingAllItems_ReturnsZero()
		{
			// Arrange
			var heap = new Heap<int>();
			heap.Push(12);
			heap.Pop();

			// Act
			int count = heap.Count;

			// Assert
			Assert.AreEqual(0, count);
		}
		#endregion

		#region Top and Push
		[TestMethod]
		public void Top_BeforePushingAnything_ThrowsArgumentOutOfRangeException()
		{
			// Arrange
			var heap = new Heap<int>();

			// Act
			Action action = () =>
			{
				int top = heap.Top;
			};

			// Assert
			Assert.ThrowsException<ArgumentOutOfRangeException>(action);
		}

		[TestMethod]
		public void Top_AfterPushingOneItem_ReturnsThatItem()
		{
			// Arrange
			var heap = new Heap<double>();
			double value = 3.14;
			heap.Push(value);

			// Act
			double top = heap.Top;

			// Assert
			Assert.AreEqual(value, top);
		}

		[TestMethod]
		public void Top_AfterPushingTwoItemsWithDefaultComparer_ReturnsLesserItem()
		{
			// Arrange
			var heap = new Heap<double>();
			double lesser = 1.5;
			double greater = 40.25;
			heap.Push(lesser);
			heap.Push(greater);

			// Act
			double top = heap.Top;

			// Assert
			Assert.AreEqual(lesser, top);
		}

		[TestMethod]
		public void Top_AfterPushingTwoItemsWithCustomComparison_ReturnsLesserItemAccordingToComparer()
		{
			// Arrange
			var heap = new Heap<string>((a, b) => a.Length - b.Length); // compared by length
			string lesser = "cat";
			string greater = "caterpillar";
			heap.Push(greater);
			heap.Push(lesser);

			// Act
			string top = heap.Top;

			// Assert
			Assert.AreEqual(lesser, top);
		}

		[TestMethod]
		public void Top_AfterPushingTwoItemsWithKeySelector_ReturnsItemWithLesserKey()
		{
			// Arrange
			var heap = new Heap<DateTime>(date => date.Minute); // compared by minute component
			DateTime lesser = new DateTime(2019, 3, 28, 10, 15, 32);
			DateTime greater = new DateTime(1991, 9, 10, 11, 29, 0);
			heap.Push(lesser);
			heap.Push(greater);

			// Act
			DateTime top = heap.Top;

			// Assert
			Assert.AreEqual(lesser, top);
		}

		[TestMethod]
		public void Top_AfterPushingWhenHeapIsAtMaxCapacity_ReturnsSecondLeastItemOfTheEntireSet()
		{
			// Arrange
			var heap = new Heap<int>(3);
			heap.Push(50);
			heap.Push(21);
			heap.Push(45);

			// Act
			heap.Push(39);
			int top = heap.Top;

			// Assert
			Assert.AreEqual(39, top);
		}
		#endregion

		#region Pop
		[TestMethod]
		public void Pop_BeforePushingAnything_ThrowsArgumentOutOfRangeException()
		{
			// Arrange
			var heap = new Heap<char>();

			// Act
			Action action = () =>
			{
				heap.Pop();
			};

			// Assert
			Assert.ThrowsException<ArgumentOutOfRangeException>(action);
		}

		[TestMethod]
		public void Pop_AfterPushingOneItem_RemovesAndReturnsThatItem()
		{
			// Arrange
			var heap = new Heap<char>();
			char value = 'r';
			heap.Push(value);

			// Act
			char removed = heap.Pop();

			// Assert
			Assert.AreEqual(value, removed);
			Assert.IsFalse(heap.Contains(removed));
		}

		[TestMethod]
		public void Pop_AfterPushingSomeItems_RemovesAndReturnsTheLeastItem()
		{
			// Arrange
			var heap = new Heap<int>();
			int leastValue = 9;

			for (int i = 0; i < 10; ++i)
			{
				heap.Push(leastValue + i);
			}

			// Act
			int removed = heap.Pop();

			// Assert
			Assert.AreEqual(leastValue, removed);
			Assert.IsFalse(heap.Contains(removed));
		}

		[TestMethod]
		public void Pop_AllItems_ItemsAreRemovedInIncreasingOrder()
		{
			// Arrange
			Heap<int> heap = new Heap<int>();
			List<int> expectedItems = new List<int>();
			Random rand = new Random();

			for (int i = 0; i < 20; ++i)
			{
				int item = rand.Next(100);
				expectedItems.Add(item);
				heap.Push(item);
			}

			expectedItems.Sort();

			// Act & Assert
			for (int i = 0; i < expectedItems.Count; ++i)
			{
				int removed = heap.Pop();
				Assert.AreEqual(expectedItems[i], removed);
			}
		}
		#endregion

		#region Clear
		[TestMethod]
		public void Clear_WithAnyHeap_RemovesAllItems()
		{
			// Arrange
			var heap = new Heap<int>();
			heap.Push(10);
			heap.Push(3);

			// Act
			heap.Clear();

			// Assert
			Assert.AreEqual(0, heap.Count);
			Assert.IsFalse(heap.Contains(10));
		}
		#endregion
	}
}
