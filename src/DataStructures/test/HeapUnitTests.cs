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

		[TestMethod]
		public void Constructor_WithNullComparer_ThrowsArgumentNullException()
		{
			// Act
			Action action = () =>
			{
				IComparer<int> c = null;
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
		public void Count_AfterPushingSomeElements_ReturnsNumberOfElementsPushed()
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
		public void Count_AfterPoppingAllElements_ReturnsZero()
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
		public void Top_AfterPushingOneElement_ReturnsThatElement()
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
		public void Top_AfterPushingTwoElementsWithDefaultComparer_ReturnsLesserElement()
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
		public void Top_AfterPushingTwoElementsWithCustomComparer_ReturnsLesserElementAccordingToComparer()
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
		public void Pop_AfterPushingOneElement_RemovesAndReturnsThatElement()
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
		public void Pop_AfterPushingSomeElements_RemovesAndReturnsTheLeastElement()
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
		public void Pop_AllElements_ElementsAreRemovedInIncreasingOrder()
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
		public void Clear_WithAnyHeap_RemovesAllElements()
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
