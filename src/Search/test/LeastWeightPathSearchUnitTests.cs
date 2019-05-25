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
		[TestMethod]
		public void Constructor_WithNullChildGenerator_ThrowsArgumentNullException()
		{
			// Act
			Action action = () =>
			{
				var search = new LeastWeightPathSearch<int>(null);
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
			CollectionAssert.AreEqual(expectedPath, path.ToList());
		}

		[TestMethod]
		public void FindPath_StartHasNoChildren_ReturnsEmptyPath()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<string>(
				EdgeWeighter.ConstantWeight(TestGraphs.EdgelessGraph<string>())
			);

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
				EdgeWeighter.ConstantWeight(TestGraphs.FiniteGraph(10))
			);
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
				EdgeWeighter.ConstantWeight(TestGraphs.OnePathGraph())
			);
			int start = 0;
			int end = 9;

			// Act
			var path = lwps.FindPath(start, end);

			// Assert
			var expectedPath = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			CollectionAssert.AreEqual(expectedPath, path.ToList());
		}

		[TestMethod]
		public void FindPath_StartAndEndAreConnectedByTwoPathsWithDifferentWeight_ReturnsPathWithLessWeight()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				EdgeWeighter.VariableWeight(TestGraphs.TwoAsymmetricalPathsGraph(),
				(int child) =>
				{
					// Give the even path (which is also the longer path in this graph)
					// less weight.
					if (child % 2 == 0)
						return 0.49;
					else
						return 0.75;
				})
			);
			int start = 1;
			int end = 7;

			// Act
			var path = lwps.FindPath(start, end);

			// Assert
			var expectedPath = new int[] { 1, 2, 4, 6, 7 };
			CollectionAssert.AreEqual(expectedPath, path.ToList());
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
				lwps.FindNode(null /*start*/, SearchTestHelpers.AnyPredicate);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindNode_PredicateIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var lwps = AnyLWPS<string>();

			// Act
			Action action = () =>
			{
				lwps.FindNode("start", null /*predicate*/);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindNode_MaxPathLengthIsZero_ReturnsNull()
		{
			// Arrange
			var lwps = AnyLWPS<string>();
			string start = "foo";

			// Act
			var node = lwps.FindNode(start, n => true, 0);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_StartHasNoChildrenAndDoesNotPassPredicate_ReturnsNull()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<string>(
				EdgeWeighter.ConstantWeight(TestGraphs.EdgelessGraph<string>())
			);

			// Act
			var node = lwps.FindNode("start", n => false);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_PredicateNeverPassesAndGraphIsFinite_ReturnsNull()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				EdgeWeighter.ConstantWeight(TestGraphs.FiniteGraph(3))
			);
			int start = 2;

			// Act
			var node = lwps.FindNode(start, n => false);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_PredicateWouldPassButPathIsLongerThanMaxPathLength_ReturnsNull()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				EdgeWeighter.ConstantWeight(TestGraphs.OnePathGraph())
			);
			int start = -1;
			int end = 7;
			uint maxPathLength = 8;

			// Act
			var node = lwps.FindNode(start, s => s == end, maxPathLength);

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_PredicatePasses_ReturnsNodeThatPassedPredicateWithPathLeadingToStart()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				EdgeWeighter.ConstantWeight(TestGraphs.OnePathGraph())
			);
			int start = -3;
			int end = 0;

			// Act
			var node = lwps.FindNode(start, s => s == end);

			// Assert
			var expectedNode = new PathNode<int>(end);
			Assert.AreEqual(expectedNode, node);

			var expectedPath = new int[] { -3, -2, -1, 0 };
			CollectionAssert.AreEqual(expectedPath, node.GetPath().ToList());
			Assert.AreEqual((uint)expectedPath.Length, node.CumulativePathLength);
		}

		[TestMethod]
		public void FindNode_EdgesHaveNonzeroCostAndPredicatePasses_ReturnsNodeWithCorrectCumulativePathWeight()
		{
			// Arrange
			double weight = 3.14;
			var lwps = new LeastWeightPathSearch<int>(
				EdgeWeighter.ConstantWeight(TestGraphs.OnePathGraph(), weight)
			);
			int start = 0;
			int end = 12;

			// Act
			var node = lwps.FindNode(start, s => s == end);

			// Assert
			double expectedTotalWeight = (end - start) * weight;
			Assert.AreEqual(expectedTotalWeight, node.CumulativePathWeight);
		}

		[TestMethod]
		public void FindNode_PredicatePassesForTwoNodesAtSameDistanceFromStartButDifferentWeights_ReturnsNodeOnPathWithLessWeight()
		{
			// Arrange
			var lwps = new LeastWeightPathSearch<int>(
				EdgeWeighter.VariableWeight(TestGraphs.BinaryTree(),
				(int child) =>
				{
					// Give even nodes less weight
					if (child % 2 == 0)
						return 1;
					else
						return 1.5;
				})
			);

			// Act
			// In a binary tree, nodes 8 and 9 are equidistant from the root
			// (both are children of node 4).
			var node = lwps.FindNode(1, s =>
			{
				return s == 8 || s == 9;
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
				EdgeWeighter.ConstantWeight(TestGraphs.OneCycleGraph(10), -2)
			);
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
			return new LeastWeightPathSearch<T>(SearchTestHelpers.AnyWeightedChildGenerator);
		}
	};

}
