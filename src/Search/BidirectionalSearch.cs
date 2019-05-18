using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Algorithms.Search {

	/// <summary>
	/// Bidirectional search is a breadth-first search starting from two nodes
	/// simultaneously.<para/>
	/// One path proceeds forward, like all other search algorithms; the other
	/// proceeds backward and must be reversed once a common node is found in both
	/// paths. The algorithm is guaranteed to find the shortest path between the two
	/// nodes, if one exists.
	/// </summary>
	public class BidirectionalSearch<T>
	{
		private readonly Func<T, IEnumerable<T>> GetForwardChildren;
		private readonly Func<T, IEnumerable<T>> GetReverseChildren;
		private readonly Action<IEnumerable<T>> RepairReversePath;

		/// <summary>
		/// Creates a BidirectionalSearch for an undirected graph.
		/// </summary>
		/// <param name="getChildrenUndirected">the ChildGenerator for both search paths</param>
		/// <param name="repairReversePath">an optional function that repairs links between
		/// states in the reverse path. The function will be given the path in the final order,
		/// starting with the node found in both paths</param>
		public BidirectionalSearch(
			Func<T, IEnumerable<T>> getChildrenUndirected,
			Action<IEnumerable<T>> repairReversePath = null)
			: this(getChildrenUndirected, getChildrenUndirected, repairReversePath)
		{
		}

		/// <summary>
		/// Creates a BidirectionalSearch for a directed graph.
		/// </summary>
		/// <param name="getForwardChildren">the ChildGenerator for the forward path</param>
		/// <param name="getReverseChildren">the ChildGenerator for the reverse path.
		/// When the path is reversed at the end, states returned by this function will
		/// become parents of the state passed in.</param>
		/// <param name="repairReversePath">an optional function that repairs links between
		/// states in the reverse path. The function will be given the path in the final order,
		/// starting with the node found in both paths</param>
		public BidirectionalSearch(
			Func<T, IEnumerable<T>> getForwardChildren,
			Func<T, IEnumerable<T>> getReverseChildren,
			Action<IEnumerable<T>> repairReversePath = null)
		{
			Validate.IsNotNull(getForwardChildren, "getForwardChildren");
			Validate.IsNotNull(getReverseChildren, "getReverseChildren");

			GetForwardChildren = getForwardChildren;
			GetReverseChildren = getReverseChildren;
			RepairReversePath = repairReversePath;
		}

		public IEnumerable<T> FindPath(T start, T end)
		{
			Validate.IsNotNull(start, "start");
			Validate.IsNotNull(end, "end");

			if (start.Equals(end))
				return new T[] { start };

			var forwardFrontier = new HashSet<PathNode<T>>();
			var reverseFrontier = new HashSet<PathNode<T>>();
			var explored = new HashSet<T>();
			IEnumerable<T> solution = null;

			forwardFrontier.Add(new PathNode<T>(start));
			reverseFrontier.Add(new PathNode<T>(end));

			while (forwardFrontier.Count > 0 && reverseFrontier.Count > 0)
			{
				UpdateFrontier(
					ref forwardFrontier,
					reverseFrontier,
					explored,
					GetForwardChildren,
					(forwardNode, reverseNode) =>
					{
						solution = ConstructSolution(forwardNode, reverseNode);
					}
				);

				if (solution != null)
					return solution;

				UpdateFrontier(
					ref reverseFrontier,
					forwardFrontier,
					explored,
					GetReverseChildren,
					(reverseNode, forwardNode) =>
					{
						solution = ConstructSolution(forwardNode, reverseNode);
					}
				);

				if (solution != null)
					return solution;
			}

			// Path not found
			return new T[] { };
		}

		/*
		 * Updates either the forward or the reverse frontier by exploring every unexplored
		 * node in the frontier and checking for child membership in the other frontier. If
		 * a matching child is found, then a solution exists.
		 */
		private static void UpdateFrontier(
			ref HashSet<PathNode<T>> currentFrontier,
			HashSet<PathNode<T>> otherFrontier,
			HashSet<T> explored,
			Func<T, IEnumerable<T>> getChildren,
			Action<PathNode<T>, PathNode<T>> onMatchFound)
		{
			var nextFrontier = new HashSet<PathNode<T>>();
			foreach (var currentNode in currentFrontier)
			{
				explored.Add(currentNode.State);
				foreach (var child in getChildren(currentNode.State))
				{
					var childNode = new PathNode<T>(child, currentNode);
					if (!explored.Contains(child) &&
						!currentFrontier.Contains(childNode))
					{
						if (otherFrontier.Contains(childNode))
						{
							var matchingNode = otherFrontier.First(node => node.Equals(childNode));
							onMatchFound(childNode, matchingNode);
							return;
						}
						else
						{
							nextFrontier.Add(childNode);
						}
					}
				}
			}

			currentFrontier = nextFrontier;
		}

		private IEnumerable<T> ConstructSolution(PathNode<T> forwardPathNode, PathNode<T> reversePathNode)
		{
			var pathFromStart = forwardPathNode.GetPath();
			var pathToEnd = reversePathNode.GetPathToRoot();

			if (RepairReversePath != null)
				RepairReversePath(pathToEnd);

			return pathFromStart.Concat(pathToEnd.Skip(1)); // skip the common node
		}
	}

}
