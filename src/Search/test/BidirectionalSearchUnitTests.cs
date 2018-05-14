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
		[TestMethod]
		public void Constructor_WithNullUndirectedChildGenerator_ThrowsArgumentNullException()
		{
			// Act
			Action action = () =>
			{
				var bidi = new BidirectionalSearch<int>(null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void Constructor_WithNullForwardChildGenerator_ThrowsArgumentNullException()
		{
			// Act
			Action action = () =>
			{
				var bidi = new BidirectionalSearch<int>(null, SearchTestHelpers.AnyChildGenerator);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void Constructor_WithNullReverseChildGenerator_ThrowsArgumentNullException()
		{
			// Act
			Action action = () =>
			{
				var bidi = new BidirectionalSearch<int>(SearchTestHelpers.AnyChildGenerator, null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}
		#endregion

		#region FindPath
		[TestMethod]
		public void FindPath_StartIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var bidi = new BidirectionalSearch<string>(SearchTestHelpers.AnyChildGenerator);

			// Act
			Action action = () =>
			{
				bidi.FindPath(null, "end");
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindPath_EndIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var bidi = new BidirectionalSearch<string>(SearchTestHelpers.AnyChildGenerator);

			// Act
			Action action = () =>
			{
				bidi.FindPath("start", null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindPath_StartEqualsEnd_ReturnsPathContainingStart()
		{
			// Arrange
			var bidi = new BidirectionalSearch<string>(SearchTestHelpers.AnyChildGenerator);
			string start = "foo";
			string end = start;

			// Act
			var path = bidi.FindPath(start, end);

			// Assert
			var expectedPath = new string[] { start };
			CollectionAssert.AreEqual(expectedPath, path.ToList());
		}

		[TestMethod]
		public void FindPath_StartAndEndHaveNoChildren_ReturnsEmptyPath()
		{
			// Arrange
			var bidi = new BidirectionalSearch<string>(TestGraphs.EdgelessGraph<string>());

			// Act
			var path = bidi.FindPath("start", "end");

			// Assert
			Assert.AreEqual(0, path.Count());
		}

		[TestMethod]
		public void FindPath_StartAndEndAreNotConnectedAndGraphIsFinite_ReturnsEmptyPath()
		{
			// Arrange
			var bidi = new BidirectionalSearch<int>(TestGraphs.FiniteGraph(6));
			int start = -2;
			int end = 5;

			// Act
			var path = bidi.FindPath(start, end);

			// Assert
			Assert.AreEqual(0, path.Count());
		}

		[TestMethod]
		public void FindPath_EndIsInFrontierOfStart_ReturnsPathFromStartToEndOfLengthTwo()
		{
			// Arrange
			var bidi = new BidirectionalSearch<int>(TestGraphs.OnePathGraph());
			int start = 2;
			int end = 3;

			// Act
			var path = bidi.FindPath(start, end);

			// Assert
			var expectedPath = new int[] { start, end };
			CollectionAssert.AreEqual(expectedPath, path.ToList());
		}

		[TestMethod]
		public void FindPath_StartAndEndAreConnected_ReturnsPathFromStartToEnd()
		{
			// Arrange
			var bidi = new BidirectionalSearch<int>(
				TestGraphs.TwoAsymmetricalPathsGraph(),
				TestGraphs.ReverseTwoAsymmetricalPathsGraph());
			int start = 1;
			int end = 7;

			// Act
			var path = bidi.FindPath(start, end);

			// Assert
			var expectedPath = new int[] { 1, 3, 5, 7 };
			CollectionAssert.AreEqual(expectedPath, path.ToList());
		}

		[TestMethod]
		public void FindPath_StartAndEndAreConnectedAndRepairReversePathIsGiven_CallsRepairReversePathWithCorrectPath()
		{
			// Arrange
			IEnumerable<int> actualReversePath = null;
			var bidi = new BidirectionalSearch<int>(
				TestGraphs.TwoAsymmetricalPathsGraph(),
				TestGraphs.ReverseTwoAsymmetricalPathsGraph(),
				(reversePath) => { actualReversePath = reversePath; });
			int start = 1;
			int end = 7;

			// Act
			var path = bidi.FindPath(start, end);

			// Assert
			var expectedReversePath = new int[] { 5, 7 };
			CollectionAssert.AreEqual(expectedReversePath, actualReversePath.ToList());
		}
		#endregion
	};

}
