using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.Algorithms.Search;

namespace Test {

	[TestClass]
	public class BreadthFirstSearchUnitTests
	{
		#region Constructor
		[TestMethod]
		public void Constructor_WithNullChildGenerator_ThrowsArgumentNullException()
		{
			// Act
			Action action = () =>
			{
				var bfs = new BreadthFirstSearch<int>(null);
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
			var bfs = new BreadthFirstSearch<string>(SearchTestHelpers.AnyChildGenerator);

			// Act
			Action action = () =>
			{
				bfs.FindPath(null /*start*/, "end");
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindPath_EndIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<string>(SearchTestHelpers.AnyChildGenerator);

			// Act
			Action action = () =>
			{
				bfs.FindPath("start", null /*end*/);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindPath_StartEqualsEnd_ReturnsPathContainingStart()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<string>(SearchTestHelpers.AnyChildGenerator);
			string start = "foo";
			string end = start;

			// Act
			var path = bfs.FindPath(start, end);

			// Assert
			var expectedPath = new string[] { start };
			Assert.IsTrue(expectedPath.SequenceEqual(path));
		}

		[TestMethod]
		public void FindPath_StartHasNoChildren_ReturnsEmptyPath()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<string>(TestGraphs.EdgelessGraph<string>());

			// Act
			var path = bfs.FindPath("start", "end");

			// Assert
			Assert.AreEqual(0, path.Count());
		}

		[TestMethod]
		public void FindPath_StartAndEndAreNotConnectedAndGraphIsFinite_ReturnsEmptyPath()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<int>(TestGraphs.FiniteGraph(13));
			int start = 1;
			int end = 14;

			// Act
			var path = bfs.FindPath(start, end);

			// Assert
			Assert.AreEqual(0, path.Count());
		}

		[TestMethod]
		public void FindPath_StartAndEndAreConnected_ReturnsPathFromStartToEnd()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<int>(TestGraphs.OnePathGraph());
			int start = 1;
			int end = 4;

			// Act
			var path = bfs.FindPath(start, end);

			// Assert
			var expectedPath = new int[] { 1, 2, 3, 4 };
			Assert.IsTrue(expectedPath.SequenceEqual(path));
		}
		#endregion

		#region FindNode
		[TestMethod]
		public void FindNode_StartIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<string>(SearchTestHelpers.AnyChildGenerator);

			// Act
			Action action = () =>
			{
				bfs.FindNode(null /*start*/, SearchTestHelpers.AnyNodePredicate);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindNode_NodePredicateIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<string>(SearchTestHelpers.AnyChildGenerator);

			// Act
			Action action = () =>
			{
				bfs.FindNode("start", null /*nodePredicate*/);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindNode_MaxSearchDistanceIsZeroAndStartPassesPredicate_ReturnsStartNode()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<string>(SearchTestHelpers.AnyChildGenerator);
			string start = "foo";

			// Act
			var node = bfs.FindNode(start, n => true, 0);

			// Assert
			var expectedNode = new PathNode<string>(start);
			Assert.AreEqual(expectedNode, node);
		}

		[TestMethod]
		public void FindNode_MaxSearchDistanceIsZeroAndStartDoesNotPassPredicate_ReturnsNull()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<string>(SearchTestHelpers.AnyChildGenerator);

			// Act
			var node = bfs.FindNode("start", n => false, 0);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_StartHasNoChildrenAndDoesNotPassPredicate_ReturnsNull()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<string>(TestGraphs.EdgelessGraph<string>());

			// Act
			var node = bfs.FindNode("start", n => false);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_NodePredicateNeverPassesAndGraphIsFinite_ReturnsNull()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<int>(TestGraphs.FiniteGraph(20));
			int anyStart = 10;

			// Act
			var node = bfs.FindNode(anyStart, n => false);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_NodePredicateWouldPassButPathIsLongerThanMaxSearchDistance_ReturnsNull()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<int>(TestGraphs.OnePathGraph());
			int start = 1;
			int end = 10;
			uint maxSearchDistance = 8;

			// Act
			var node = bfs.FindNode(start, n => n.State == end, maxSearchDistance);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_NodePredicatePasses_ReturnsNodeThatPassedPredicateWithPathLeadingToStart()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<int>(TestGraphs.OnePathGraph());
			int start = 1;
			int end = 4;

			// Act
			var node = bfs.FindNode(start, n => n.State == end);

			// Assert
			var expectedNode = new PathNode<int>(end);
			Assert.AreEqual(expectedNode, node);

			var expectedPath = new int[] { 1, 2, 3, 4 };
			Assert.IsTrue(expectedPath.SequenceEqual(node.GetPath()));
			Assert.AreEqual((uint)expectedPath.Length, node.CumulativePathLength);
		}
		#endregion

		#region Special graphs
		[TestMethod]
		public void FindPath_GraphIsABinaryTree_FindsPathBetweenTwoNodes()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<int>(TestGraphs.BinaryTree());
			int start = 1;
			int end = 7;

			// Act
			var path = bfs.FindPath(start, end);

			// Assert
			var expectedPath = new int[] { 1, 3, 7 };
			Assert.IsTrue(expectedPath.SequenceEqual(path));
		}

		[TestMethod]
		public void FindPath_GraphIsACycleContainingEnd_FindsPathFromStartToEnd()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<int>(TestGraphs.OneCycleGraph(8));
			int start = 1;
			int end = 5;

			// Act
			var path = bfs.FindPath(start, end);

			// Assert
			var expectedPath = new int[] { 1, 2, 3, 4, 5 };
			Assert.IsTrue(expectedPath.SequenceEqual(path));
		}

		[TestMethod]
		public void FindPath_GraphIsACycleNotContainingEnd_ReturnsEmptyPath()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<int>(TestGraphs.OneCycleGraph(8));
			int start = 3;
			int end = 9;

			// Act
			var path = bfs.FindPath(start, end);

			// Assert
			Assert.AreEqual(0, path.Count());
		}

		[TestMethod]
		public void FindPath_GraphHasTwoPathsFromStartToEnd_ReturnsShorterPath()
		{
			// Arrange
			var bfs = new BreadthFirstSearch<int>(TestGraphs.TwoAsymmetricalPathsGraph());
			int start = 1;
			int end = 7;

			// Act
			var path = bfs.FindPath(start, end);

			// Assert
			var expectedPath = new int[] { 1, 3, 5, 7 };
			Assert.IsTrue(expectedPath.SequenceEqual(path));
		}
		#endregion
	};

}
