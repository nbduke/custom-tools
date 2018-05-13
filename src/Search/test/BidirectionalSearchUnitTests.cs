using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.Algorithms.Search;

namespace Test {

	[TestClass]
	public class BidirectionalSearchUnitTests
	{
		#region Constructor

		#endregion

		[TestMethod]
		public void FindPath_PathsNeverMeetButEndIsOnPathFromStart_ReturnsPathFromStartToEnd()
		{
			// Arrange
			var bidi = new BidirectionalSearch<int>(TestGraphs.BinaryTree());
			int start = 1;
			int end = 10;

			// Act
			var path = bidi.FindPath(start, end);

			// Assert
			var expectedPath = new int[] { 1, 2, 5, 10 };
			Assert.IsTrue(expectedPath.SequenceEqual(path));
		}

		[TestMethod]
		public void FindPath_PathsNeverMeetButStartIsOnPathFromEnd_ReturnsPathFromStartToEnd()
		{
			// Arrange
			var bidi = new BidirectionalSearch<int>(TestGraphs.BinaryTree());
			int start = 10;
			int end = 1;

			// Act
			var path = bidi.FindPath(start, end);

			// Assert
			var expectedPath = new int[] { 10, 5, 2, 1 };
			Assert.IsTrue(expectedPath.SequenceEqual(path));
		}
	};

}
