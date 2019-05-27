using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.Algorithms.Search;

namespace Test {

	[TestClass]
	public class FlexibleBacktrackingSearchUnitTests
	{
		#region Constructor
		[TestMethod]
		public void Constructor_WithNullChildGenerator_ThrowsArgumentNullException()
		{
			// Act
			Action action = () =>
			{
				var fbs = new FlexibleBacktrackingSearch<int>(null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}
		#endregion

		#region Search
		[TestMethod]
		public void Search_WithNullStartState_ThrowsArgumentNullException()
		{
			// Arrange
			var fbs = new FlexibleBacktrackingSearch<string>(SearchTestHelpers.AnyChildGenerator);

			// Act
			Action action = () =>
			{
				fbs.Search(null, node => NodeOption.Backtrack);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void Search_WithNullNodeProcessor_ThrowsArgumentNullException()
		{
			// Arrange
			var fbs = new FlexibleBacktrackingSearch<int>(SearchTestHelpers.AnyChildGenerator);

			// Act
			Action action = () =>
			{
				fbs.Search(0, null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void Search_WithFiniteGraphAndContinueNodeOption_ProcessesEveryNode()
		{
			// Arrange
			var fbs = new FlexibleBacktrackingSearch<int>(TestGraphs.FiniteGraph(4));
			var processedStates = new List<int>();

			// Act
			fbs.Search(0, n =>
			{
				processedStates.Add(n.State);
				return NodeOption.Continue;
			});

			// Assert
			var expectedStates = new int[] { 0, 1, 2, 3, 4 };
			CollectionAssert.AreEqual(expectedStates, processedStates);
		}

		[TestMethod]
		public void Search_WithFiniteGraphAndContinueNodeOptionAndMaxPathLength_ProcessesNodesUpToMaxLength()
		{
			// Arrange
			var fbs = new FlexibleBacktrackingSearch<int>(TestGraphs.FiniteGraph(10));
			var processedStates = new List<int>();
			uint maxPathLength = 3;

			// Act
			fbs.Search(1, n =>
			{
				processedStates.Add(n.State);
				return NodeOption.Continue;
			}, maxPathLength);

			// Assert
			var expectedStates = new int[] { 1, 2, 3 };
			CollectionAssert.AreEqual(expectedStates, processedStates);
		}

		[TestMethod]
		public void Search_WithStopNodeAction_ProcessesOnlyStartNode()
		{
			// Arrange
			var fbs = new FlexibleBacktrackingSearch<int>(TestGraphs.OnePathGraph());
			var processedStates = new List<int>();
			int start = 4;

			// Act
			fbs.Search(start, n =>
			{
				processedStates.Add(n.State);
				return NodeOption.Stop;
			});

			// Assert
			var expectedStates = new int[] { start };
			CollectionAssert.AreEqual(expectedStates, processedStates);
		}

		[TestMethod]
		public void Search_WithBacktrackNodeOption_ProcessesOnlyStartNode()
		{
			// Arrange
			var fbs = new FlexibleBacktrackingSearch<int>(TestGraphs.OnePathGraph());
			var processedStates = new List<int>();
			int start = -3;

			// Act
			fbs.Search(start, n =>
			{
				processedStates.Add(n.State);
				return NodeOption.Backtrack;
			});

			// Assert
			var expectedStates = new int[] { start };
			CollectionAssert.AreEqual(expectedStates, processedStates);
		}

		[TestMethod]
		public void Search_WithBacktrackNodeOptionAfterStartNode_ProcessesChildrenOfStartNode()
		{
			// Arrange
			var fbs = new FlexibleBacktrackingSearch<int>(TestGraphs.BinaryTree());
			var processedStates = new List<int>();
			int start = 1;

			// Act
			fbs.Search(start, n =>
			{
				processedStates.Add(n.State);
				return n.State == start ? NodeOption.Continue : NodeOption.Backtrack;
			});

			// Assert
			var expectedStates = new int[] { start, start * 2, start * 2 + 1 };
			CollectionAssert.AreEqual(expectedStates, processedStates);
		}

		[TestMethod]
		public void Search_GraphIsCyclicalButPathIsCheckedForDuplicates_ProcessesEachNodeInCycleOnce()
		{
			// Arrange
			var fbs = new FlexibleBacktrackingSearch<int>(TestGraphs.OneCycleGraph(4));
			var processedStates = new List<int>();
			int start = 2;

			// Act
			fbs.Search(start, n =>
			{
				processedStates.Add(n.State);
				return NodeOption.Continue;
			});

			// Assert
			var expectedStates = new int[] { 2, 3, 0, 1 };
			CollectionAssert.AreEqual(expectedStates, processedStates);
		}

		[TestMethod]
		public void Search_GraphIsCyclicalAndPathNotCheckedForDuplicates_ProcessesNodesRepeatedly()
		{
			// Arrange
			var fbs = new FlexibleBacktrackingSearch<int>(TestGraphs.OneCycleGraph(4), true);
			var processedStates = new List<int>();
			int start = 2;
			uint maxPathLength = 6;

			// Act
			fbs.Search(start, n =>
			{
				processedStates.Add(n.State);
				return NodeOption.Continue;
			}, maxPathLength);

			// Assert
			var expectedStates = new int[] { 2, 3, 0, 1, 2, 3 };
			CollectionAssert.AreEqual(expectedStates, processedStates);
		}
		#endregion
	}

}