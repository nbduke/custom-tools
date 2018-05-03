using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Algorithms.Search {

	/*
	 * Bidirectional search is a breadth-first search starting from two nodes
	 * simultaneously. One path proceeds forward, like all other search
	 * algorithms; the other proceeds backward and must be reversed once a
	 * common node is found in both paths.
	 * 
	 * This algorithm is guaranteed to find the shortest path between the two
	 * nodes, if one exists.
	 */
	public class BidirectionalSearch<T>
	{
		private readonly ChildGenerator<T> GetForwardChildren;
		private readonly ChildGenerator<T> GetReverseChildren;
		private readonly Action<IEnumerable<T>> RepairReversePath;

		/// <summary>
		/// Creates a BidirectionalSearch for an undirected graph.
		/// </summary>
		/// <param name="getChildrenUndirected">the ChildGenerator for both search paths</param>
		/// <param name="repairReversePath">an optional function that repairs links between
		/// states in the reverse path. The function will be given the path in the final order,
		/// starting with the node found in both paths</param>
		public BidirectionalSearch(
			ChildGenerator<T> getChildrenUndirected,
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
			ChildGenerator<T> getForwardChildren,
			ChildGenerator<T> getReverseChildren,
			Action<IEnumerable<T>> repairReversePath = null)
		{
			if (getForwardChildren == null)
				throw new ArgumentNullException("getForwardChildren");
			if (getReverseChildren == null)
				throw new ArgumentNullException("getReverseChildren");

			GetForwardChildren = getForwardChildren;
			GetReverseChildren = getReverseChildren;
			RepairReversePath = repairReversePath;
		}

		public IEnumerable<T> FindPath(T start, T end)
		{
			if (start == null || end == null)
				return new T[] { };
			else if (start.Equals(end))
				return new T[] { start };

			var startNode = new PathNode<T>(start);
			var endNode = new PathNode<T>(end);

			var forwardFrontier = new Queue<PathNode<T>>();
			var reverseFrontier = new Queue<PathNode<T>>();
			var explored = new HashSet<PathNode<T>>();

			// Pre-load the frontiers of the start and end states
			foreach (T child in GetForwardChildren(start))
			{
				var childNode = new PathNode<T>(child, startNode);
				if (child.Equals(end)) // check here if start and end are connected
					return childNode.GetPath();
				else
					forwardFrontier.Enqueue(childNode);
			}

			foreach (T child in GetReverseChildren(end))
			{
				reverseFrontier.Enqueue(new PathNode<T>(child, endNode));
			}

			explored.Add(startNode);
			explored.Add(endNode);

			while (forwardFrontier.Count > 0 && reverseFrontier.Count > 0)
			{
				var match = FindMatchingNodes(forwardFrontier, reverseFrontier);
				if (match != null)
					return ConstructSolution(match);

				PathNode<T> forwardNode = forwardFrontier.Dequeue();
				explored.Add(forwardNode);
				foreach (T child in GetForwardChildren(forwardNode.State))
				{
					var childNode = new PathNode<T>(child, forwardNode);
					if (!explored.Contains(childNode) && !forwardFrontier.Contains(childNode))
						forwardFrontier.Enqueue(childNode);
				}

				match = FindMatchingNodes(forwardFrontier, reverseFrontier);
				if (match != null)
					return ConstructSolution(match);

				PathNode<T> reverseNode = reverseFrontier.Dequeue();
				explored.Add(reverseNode);
				foreach (T child in GetReverseChildren(reverseNode.State))
				{
					var childNode = new PathNode<T>(child, reverseNode);
					if (!explored.Contains(childNode) && !reverseFrontier.Contains(childNode))
						reverseFrontier.Enqueue(childNode);
				}
			}

			// Path not found
			return null;
		}

		private static Tuple<PathNode<T>, PathNode<T>> FindMatchingNodes(
			Queue<PathNode<T>> forwardFrontier,
			Queue<PathNode<T>> reverseFrontier)
		{
			var forwardSet = new HashSet<PathNode<T>>(forwardFrontier);
			forwardSet.IntersectWith(reverseFrontier);

			if (forwardSet.Count > 0)
			{
				PathNode<T> forwardNode = forwardSet.First();
				foreach (var reverseNode in reverseFrontier)
				{
					if (reverseNode.Equals(forwardNode))
						return new Tuple<PathNode<T>, PathNode<T>>(forwardNode, reverseNode);
				}
			}

			return null; // no common nodes
		}

		private IEnumerable<T> ConstructSolution(Tuple<PathNode<T>, PathNode<T>> matchingNodes)
		{
			var nodeOnForwardPath = matchingNodes.Item1;
			var nodeOnReversePath = matchingNodes.Item2;
			var pathFromStart = nodeOnForwardPath.GetPath();
			var pathToEnd = nodeOnReversePath.GetPath().Reverse();

			if (RepairReversePath != null)
				RepairReversePath(pathToEnd);

			return pathFromStart.Concat(pathToEnd.Skip(1)); // skip the common node
		}
	}

}
