using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.DataStructures;

namespace Test
{
	[TestClass]
	public class CounterUnitTests
	{
		#region Getter and Setter
		[TestMethod]
		public void Getter_ItemIsNotInCounter_ReturnsZero()
		{
			// Arrange
			var counter = new Counter<string>();

			// Act
			int count = counter["foo"];

			// Assert
			Assert.AreEqual(0, count);
		}

		[TestMethod]
		public void Getter_ItemIsInCounter_ReturnsCountOfItem()
		{
			// Arrange
			var counter = new Counter<string>();
			string item = "foo";
			int expectedCount = 35;

			counter[item] = expectedCount;

			// Act
			int count = counter[item];

			// Assert
			Assert.AreEqual(expectedCount, count);
		}

		[TestMethod]
		public void Setter_CountIsNegative_ThrowsArgumentException()
		{
			// Arrange
			var counter = new Counter<string>();

			// Act
			Action action = () =>
			{
				counter["not allowed"] = -1;
			};

			// Assert
			Assert.ThrowsException<ArgumentException>(action);
		}

		[TestMethod]
		public void Setter_ItemIsNotInCounter_SetsCount()
		{
			// Arrange
			var counter = new Counter<string>();
			string item = "baz";
			int expectedCount = 8;

			// Act
			counter[item] = expectedCount;

			// Assert
			Assert.AreEqual(expectedCount, counter[item]);
		}

		[TestMethod]
		public void Setter_ItemIsInContainer_OverwritesCount()
		{
			// Arrange
			var counter = new Counter<string>();
			string item = "abc";
			int expectedCount = 4;

			counter[item] = 30;

			// Act
			counter[item] = expectedCount;

			// Assert
			Assert.AreEqual(expectedCount, counter[item]);
		}
		#endregion

		#region Sum
		[TestMethod]
		public void Sum_CounterIsEmpty_ReturnsZero()
		{
			// Arrange
			var counter = new Counter<string>();

			// Act
			int sum = counter.Sum;

			// Assert
			Assert.AreEqual(0, sum);
		}

		[TestMethod]
		public void Sum_CounterOnlyContainsItemsWithZeroCount_ReturnsZero()
		{
			// Arrange
			var counter = new Counter<string>();
			counter["hello"] = 0;
			counter["world"] = 0;

			// Act
			int sum = counter.Sum;

			// Assert
			Assert.AreEqual(0, sum);
		}

		[TestMethod]
		public void Sum_CounterContainsItemsWithNonzeroCount_ReturnsSumOfCounts()
		{
			// Arrange
			var counter = new Counter<string>();
			counter["hello"] = 5;
			counter["world"] = 4;
			counter["!"] = 0;

			// Act
			int sum = counter.Sum;

			// Assert
			Assert.AreEqual(9, sum);
		}
		#endregion

		#region Increment
		[TestMethod]
		public void Increment_ArgumentIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var counter = new Counter<string>();

			// Act
			Action action = () =>
			{
				counter.Increment(null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void Increment_ItemIsNotInCounter_SetsItemCountToOne()
		{
			// Arrange
			var counter = new Counter<string>();
			string item = "foo";

			// Act
			counter.Increment(item);

			// Assert
			Assert.AreEqual(1, counter[item]);
		}

		[TestMethod]
		public void Increment_ItemIsInCounter_IncreasesItemCountByOne()
		{
			// Arrange
			var counter = new Counter<string>();
			string item = "cats";
			int initialCount = 12;

			counter[item] = initialCount;

			// Act
			counter.Increment(item);

			// Assert
			Assert.AreEqual(initialCount + 1, counter[item]);
		}
		#endregion

		#region TryDecrement
		[TestMethod]
		public void TryDecrement_ItemIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var counter = new Counter<string>();

			// Act
			Action action = () =>
			{
				counter.TryDecrement(null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void TryDecrement_ItemIsNotInCounter_ReturnsFalse()
		{
			// Arrange
			var counter = new Counter<string>();

			// Act
			bool result = counter.TryDecrement("foo");

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void TryDecrement_ItemIsInCounterWithZeroCount_DoesNotModifyCountAndReturnsFalse()
		{
			// Arrange
			var counter = new Counter<string>();
			string item = "unicorn";

			counter[item] = 0;

			// Act
			bool result = counter.TryDecrement(item);

			// Assert
			Assert.IsFalse(result);
			Assert.AreEqual(0, counter[item]);
		}

		[TestMethod]
		public void TryDecrement_ItemIsInCounterWithCountOfOne_SetsCountToZeroAndReturnsTrue()
		{
			// Arrange
			var counter = new Counter<string>();
			string item = "just one";

			counter[item] = 1;

			// Act
			bool result = counter.TryDecrement(item);

			// Assert
			Assert.AreEqual(0, counter[item]);
		}

		[TestMethod]
		public void TryDecrement_ItemIsInCounterWithNonzeroCount_DecreasesCountByOneAndReturnsTrue()
		{
			// Arrange
			var counter = new Counter<string>();
			string item = "kids";
			int initialCount = 3;

			counter[item] = initialCount;

			// Act
			bool result = counter.TryDecrement(item);

			// Assert
			Assert.IsTrue(result);
			Assert.AreEqual(initialCount - 1, counter[item]);
		}
		#endregion

		#region Clear
		[TestMethod]
		public void Clear_AtAnyTime_RemovesAllEntriesFromTheCounter()
		{
			// Arrange
			var counter = new Counter<string>();
			counter.Increment("foo");
			counter["bar"] = 5;

			// Act
			counter.Clear();

			// Assert
			Assert.AreEqual(0, counter.Sum);
			Assert.AreEqual(0, counter.Count());
		}
		#endregion
	}
}