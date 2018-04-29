using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CommonTools.DataStructures;

namespace CommonTools.Tests
{
	[TestClass]
	public class ExtensionsUnitTests
	{
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void MaxLessThanFunctor_CollectionIsEmpty_ThrowsException()
		{
			// Arrange
			List<int> items = new List<int>();
			Func<int, int, bool> lessThan = (int a, int b) => { return a < b; };

			// Act & Assert
			items.Max(lessThan);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void MaxLessThanFunctor_FunctorIsNull_ThrowsException()
		{
			// Arrange
			int[] items = { 0, 1, 2, 3 };
			Func<int, int, bool> lessThan = null;

			// Act & Assert
			items.Max(lessThan);
		}

		[TestMethod]
		public void MaxLessThanFunctor_ItemsAreUniqueAndSorted_ReturnsMaximumElement()
		{
			// Arrange
			int[] items = { -2, -1, 0, 1, 2 };
			Func<int, int, bool> lessThan = (int a, int b) => { return a < b; };
			int expected = 2;

			// Act
			int actual = items.Max(lessThan);

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MaxLessThanFunctor_ItemsAreUniqueAndUnsorted_ReturnsMaximumElement()
		{
			// Arrange
			int[] items = { 0, 2, -1, 1, -2 };
			Func<int, int, bool> lessThan = (int a, int b) => { return a < b; };
			int expected = 2;

			// Act
			int actual = items.Max(lessThan);

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MaxLessThanFunctor_ItemsAreAllEqual_ReturnsMaximumElement()
		{
			// Arrange
			int[] items = { -2, -2, -2, -2 };
			Func<int, int, bool> lessThan = (int a, int b) => { return a < b; };
			int expected = -2;

			// Act
			int actual = items.Max(lessThan);

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ArgMaxWithStandardComparer_ItemsAreUniqueAndSorted_ReturnsIndexOfLastElement()
		{
			// Arrange
			int[] items = { -1, 3, 5, 7, 9 };
			int expected = 4;

			// Act
			int actual = items.ArgMax();

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ArgMaxWithStandardComparer_ItemsAreUniqueAndUnsorted_ReturnsIndexOfMaxElement()
		{
			// Arrange
			int[] items = { 4, 3, 9, -1, 0, 2 };
			int expected = 2;

			// Act
			int actual = items.ArgMax();

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ArgMaxWithStandardComparer_ItemsContainsTwoMaxima_ReturnsIndexOfFirstMaximum()
		{
			// Arrange
			int[] items = { 6, 3, 2, 2, 6, -5, -3 };
			int expected = 0;

			// Act
			int actual = items.ArgMax();

			// Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
