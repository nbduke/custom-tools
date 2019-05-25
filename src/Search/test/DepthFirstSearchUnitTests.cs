using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.Algorithms.Search;

namespace Test {

	[TestClass]
	public class DepthFirstSearchUnitTests
	{
		#region Constructor
		[TestMethod]
		public void Constructor_WithNullChildGenerator_ThrowsArgumentNullException()
		{
			// Act
			Action action = () =>
			{
				var dfs = new DepthFirstSearch<int>(null /*getChildren*/);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}
		#endregion

		#region FindNode
		[TestMethod]
		public void FindNode_StartIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var dfs = new DepthFirstSearch<string>(SearchTestHelpers.AnyChildGenerator);

			// Act
			Action action = () =>
			{
				dfs.FindNode(null /*start*/, SearchTestHelpers.AnyPredicate);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindNode_NodePredicateIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var dfs = new DepthFirstSearch<string>(SearchTestHelpers.AnyChildGenerator);

			// Act
			Action action = () =>
			{
				dfs.FindNode("start", null /*nodePredicate*/);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindNode_MaxPathLengthIsZero_ReturnsNull()
		{
			// Arrange
			var dfs = new DepthFirstSearch<string>(SearchTestHelpers.AnyChildGenerator);
			string start = "foo";

			// Act
			var node = dfs.FindNode(start, n => true, 0);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_StartHasNoChildrenAndDoesNotPassPredicate_ReturnsNull()
		{
			// Arrange
			var dfs = new DepthFirstSearch<string>(TestGraphs.EdgelessGraph<string>());

			// Act
			var node = dfs.FindNode("start", n => false);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_NodePredicateNeverPassesAndGraphIsFinite_ReturnsNull()
		{
			// Arrange
			var dfs = new DepthFirstSearch<int>(TestGraphs.FiniteGraph(21));
			int start = 13;

			// Act
			var node = dfs.FindNode(start, n => false);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_NodePredicateWouldPassButPathIsLongerThanMaxPathLength_ReturnsNull()
		{
			// Arrange
			var dfs = new DepthFirstSearch<int>(TestGraphs.OnePathGraph());
			int start = 3;
			int end = 15;
			uint maxPathLength = 12;

			// Act
			var node = dfs.FindNode(start, s => s == end, maxPathLength);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_NodePredicatePasses_ReturnsNodeThatPassedPredicateWithPathLeadingToStart()
		{
			// Arrange
			var dfs = new DepthFirstSearch<int>(TestGraphs.OnePathGraph());
			int start = 12;
			int end = 15;

			// Act
			var node = dfs.FindNode(start, s => s == end);

			// Assert
			var expectedNode = new PathNode<int>(end);
			Assert.AreEqual(expectedNode, node);

			var expectedPath = new int[] { 12, 13, 14, 15 };
			CollectionAssert.AreEqual(expectedPath, node.GetPath().ToList());
			Assert.AreEqual((uint)expectedPath.Length, node.CumulativePathLength);
		}
		#endregion

		#region Special graphs
		[TestMethod]
		public void FindNode_GraphIsABinaryTree_ReturnsNodeOnExpectedPath()
		{
			// Arrange
			var dfs = new DepthFirstSearch<int>(TestGraphs.BinaryTree());
			int start = 1;
			int end = 7;

			// Act
			var node = dfs.FindNode(start, s => s == end);

			// Assert
			var expectedNode = new PathNode<int>(end);
			Assert.AreEqual(expectedNode, node);

			var expectedPath = new int[] { 1, 3, 7 };
			CollectionAssert.AreEqual(expectedPath, node.GetPath().ToList());
		}

		[TestMethod]
		public void FindNode_GraphIsACycleAndNodePredicatePasses_ReturnsNodeThatPassedPredicate()
		{
			// Arrange
			var dfs = new DepthFirstSearch<int>(TestGraphs.OneCycleGraph(6));
			int start = 4;
			int end = 2;

			// Act
			var node = dfs.FindNode(start, s => s == end);

			// Assert
			var expectedNode = new PathNode<int>(end);
			Assert.AreEqual(expectedNode, node);

			var expectedPath = new int[] { 4, 5, 0, 1, 2 };
			CollectionAssert.AreEqual(expectedPath, node.GetPath().ToList());
		}

		[TestMethod]
		public void FindNode_GraphIsACycleAndNodePredicateDoesNotPass_ReturnsNull()
		{
			// Arrange
			var dfs = new DepthFirstSearch<int>(TestGraphs.OneCycleGraph(4));
			int start = 2;

			// Act
			var node = dfs.FindNode(start, n => false);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_GraphsHasTwoPathsFromStartToTargetNode_ReturnsNodeOnFirstPathExplored()
		{
			// Arrange
			var dfs = new DepthFirstSearch<int>(TestGraphs.TwoAsymmetricalPathsGraph());
			int start = 1;
			int end = 7;

			// Act
			var node = dfs.FindNode(start, s => s == end);

			// Assert
			var expectedNode = new PathNode<int>(end);
			Assert.AreEqual(expectedNode, node);

			var expectedPath = new int[] { 1, 2, 4, 6, 7 };
			CollectionAssert.AreEqual(expectedPath, node.GetPath().ToList());
		}
		#endregion
	}

}
