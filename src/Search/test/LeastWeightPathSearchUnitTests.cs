using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.Algorithms.Search;

namespace Test {

	[TestClass]
	public class LeastWeightPathSearchUnitTests
	{
		#region Constructor
		private void Constructor_WithNullArgument_ThrowsArgumentNullException(
			ChildGenerator<int> getChildren,
			EdgeWeightCalculator<int> getEdgeWeight)
		{
			// Act
			Action action = () =>
			{
				var lwps = new LeastWeightPathSearch<int>(getChildren, getEdgeWeight);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void Constructor_WithNullChildGenerator_ThrowsArgumentNullException()
		{
			Constructor_WithNullArgument_ThrowsArgumentNullException(
				null,
				AnyEdgeWeightCalculator);
		}

		[TestMethod]
		public void Constructor_WithNullEdgeWeightCalculator_ThrowsArgumentNullException()
		{
			Constructor_WithNullArgument_ThrowsArgumentNullException(
				SearchTestHelpers.AnyChildGenerator,
				null);
		}
		#endregion

		#region FindPath
		[TestMethod]
		public void FindPath_StartIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var lwps = AnyLWPS<string>();

			// Act
			Action action = () =>
			{
				lwps.FindPath(null /*start*/, "end");
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindPath_EndIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var lwps = AnyLWPS<string>();

			// Act
			Action action = () =>
			{
				lwps.FindPath("start", null /*end*/);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindPath_StartEqualsEnd_ReturnsPathContainingStart()
		{
			// Arrange
			var lwps = AnyLWPS<string>();
			string start = "foo";
			string end = start;

			// Act
			var path = lwps.FindPath(start, end);

			// Assert
			var expectedPath = new string[] { start };
			Assert.IsTrue(expectedPath.SequenceEqual(path));
		}

		[TestMethod]
		public void FindPath_StartHasNoChildren_ReturnsEmptyPath()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<string>(
				TestGraphs.EdgelessGraph<string>(),
				AnyEdgeWeightCalculator);

			// Act
			var path = lwps.FindPath("start", "end");

			// Assert
			Assert.AreEqual(0, path.Count());
		}

		[TestMethod]
		public void FindPath_StartAndEndAreNotConnectedAndGraphIsFinite_ReturnsEmptyPath()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				TestGraphs.FiniteGraph(10),
				AnyEdgeWeightCalculator);
			int start = 0;
			int end = 20;

			// Act
			var path = lwps.FindPath(start, end);

			// Assert
			Assert.AreEqual(0, path.Count());
		}

		[TestMethod]
		public void FindPath_StartAndEndAreConnected_ReturnsPathFromStartToEnd()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				TestGraphs.OnePathGraph(),
				AnyEdgeWeightCalculator);
			int start = 0;
			int end = 9;

			// Act
			var path = lwps.FindPath(start, end);

			// Assert
			var expectedPath = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			Assert.IsTrue(expectedPath.SequenceEqual(path));
		}

		[TestMethod]
		public void FindPath_StartAndEndAreConnectedByTwoPathsWithDifferentWeight_ReturnsPathWithLessWeight()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				TestGraphs.TwoAsymmetricalPathsGraph(),
				(int parent, int child) =>
				{
					// Give the even path (which is also the longer path in this graph)
					// less weight.
					if (child % 2 == 0)
						return 0.49;
					else
						return 0.75;
				});
			int start = 1;
			int end = 7;

			// Act
			var path = lwps.FindPath(start, end);

			// Assert
			var expectedPath = new int[] { 1, 2, 4, 6, 7 };
			Assert.IsTrue(expectedPath.SequenceEqual(path));
		}
		#endregion

		#region FindNode
		[TestMethod]
		public void FindNode_StartIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var lwps = AnyLWPS<string>();

			// Act
			Action action = () =>
			{
				lwps.FindNode(null /*start*/, SearchTestHelpers.AnyNodePredicate);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindNode_NodePredicateIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var lwps = AnyLWPS<string>();

			// Act
			Action action = () =>
			{
				lwps.FindNode("start", null /*nodePredicate*/);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindNode_MaxSearchDistanceIsZeroAndStartPassesPredicate_ReturnsStartNode()
		{
			// Arrange
			var lwps = AnyLWPS<string>();
			string start = "foo";

			// Act
			var node = lwps.FindNode(start, n => true, 0);

			// Assert
			var expectedNode = new PathNode<string>(start);
			Assert.AreEqual(expectedNode, node);
		}

		[TestMethod]
		public void FindNode_MaxSearchDistanceIsZeroAndStartDoesNotPassPredicate_ReturnsNull()
		{
			// Arrange
			var lwps = AnyLWPS<string>();

			// Act
			var node = lwps.FindNode("start", n => false, 0);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_StartHasNoChildrenAndDoesNotPassPredicate_ReturnsNull()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<string>(
				TestGraphs.EdgelessGraph<string>(),
				AnyEdgeWeightCalculator);

			// Act
			var node = lwps.FindNode("start", n => false);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_NodePredicateNeverPassesAndGraphIsFinite_ReturnsNull()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				TestGraphs.FiniteGraph(3),
				AnyEdgeWeightCalculator);
			int start = 2;

			// Act
			var node = lwps.FindNode(start, n => false);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_NodePredicateWouldPassButPathIsLongerThanMaxSearchDistance_ReturnsNull()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				TestGraphs.OnePathGraph(),
				AnyEdgeWeightCalculator);
			int start = -1;
			int end = 7;
			uint maxSearchDistance = 2;

			// Act
			var node = lwps.FindNode(start, n => n.State == end, maxSearchDistance);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_NodePredicatePasses_ReturnsNodeThatPassedPredicateWithPathLeadingToStart()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				TestGraphs.OnePathGraph(),
				AnyEdgeWeightCalculator);
			int start = -3;
			int end = 0;

			// Act
			var node = lwps.FindNode(start, n => n.State == end);

			// Assert
			var expectedNode = new PathNode<int>(end);
			Assert.AreEqual(expectedNode, node);

			var expectedPath = new int[] { -3, -2, -1, 0 };
			Assert.IsTrue(expectedPath.SequenceEqual(node.GetPath()));
			Assert.AreEqual((uint)expectedPath.Length, node.CumulativePathLength);
		}

		[TestMethod]
		public void FindNode_EdgesHaveNonzeroCostAndNodePredicatePasses_ReturnsNodeWithCorrectCumulativePathWeight()
		{
			// Arrange
			double weight = 3.14;
			var lwps = new LeastWeightPathSearch<int>(
				TestGraphs.OnePathGraph(),
				ConstantEdgeWeightCalculator<int>(weight));
			int start = 0;
			int end = 12;

			// Act
			var node = lwps.FindNode(start, n => n.State == end);

			// Assert
			double expectedTotalWeight = (end - start) * weight;
			Assert.AreEqual(expectedTotalWeight, node.CumulativePathWeight);
		}

		[TestMethod]
		public void FindNode_NodePredicatePassesForTwoNodesAtSameDistanceFromStartButDifferentWeights_ReturnsNodeOnPathWithLessWeight()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				TestGraphs.BinaryTree(),
				(int parent, int child) =>
				{
					// Give odd nodes more weight
					if (child == parent * 2)
						return 1;
					else
						return 1.5;
				});

			// Act
			// In a binary tree, nodes 8 and 9 are equidistant from the root
			// (both are children of node 4).
			var node = lwps.FindNode(1, n =>
			{
				return n.State == 8 || n.State == 9;
			});

			// Assert
			var expectedNode = new PathNode<int>(8);
			Assert.AreEqual(expectedNode, node);
		}
		#endregion

		#region Special graphs
		[TestMethod]
		public void FindPath_GraphContainsNegativeCycleAndEndIsNotOnPath_ReturnsEmptyPath()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				TestGraphs.OneCycleGraph(10),
				(int parent, int child) =>
				{
					return -2; // negative edge weights
				});

			int start = 3;
			int end = 100;

			// Act
			var path = lwps.FindPath(start, end);

			// Assert
			Assert.AreEqual(0, path.Count());
		}
		#endregion

		private LeastWeightPathSearch<T> AnyLWPS<T>()
		{
			return new LeastWeightPathSearch<T>(
				SearchTestHelpers.AnyChildGenerator,
				AnyEdgeWeightCalculator);
		}

		private static double AnyEdgeWeightCalculator<T>(T parent, T child)
		{
			return 0;
		}

		private static EdgeWeightCalculator<T> ConstantEdgeWeightCalculator<T>(double weight)
		{
			return (T parent, T child) =>
			{
				return weight;
			};
		}
	};

}