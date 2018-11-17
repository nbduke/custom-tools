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
			var explored = new HashSet<PathNode<T>>();

			forwardFrontier.Add(new PathNode<T>(start));
			reverseFrontier.Add(new PathNode<T>(end));

			while (forwardFrontier.Count > 0 && reverseFrontier.Count > 0)
			{
				forwardFrontier = GetNextLayer(forwardFrontier, explored, GetForwardChildren);
				var match = FindMatchingNodes(forwardFrontier, reverseFrontier);
				if (match != null)
					return ConstructSolution(match);

				reverseFrontier = GetNextLayer(reverseFrontier, explored, GetReverseChildren);
				match = FindMatchingNodes(forwardFrontier, reverseFrontier);
				if (match != null)
					return ConstructSolution(match);
			}

			// Path not found
			return new T[] { };
		}

		private static HashSet<PathNode<T>> GetNextLayer(
			HashSet<PathNode<T>> currentLayer,
			HashSet<PathNode<T>> explored,
			ChildGenerator<T> getChildren)
		{
			var nextLayer = new HashSet<PathNode<T>>();
			foreach (var currentNode in currentLayer)
			{
				explored.Add(currentNode);

				foreach (var child in getChildren(currentNode.State))
				{
					var childNode = new PathNode<T>(child, currentNode);
					if (!explored.Contains(childNode) &&
						!currentLayer.Contains(childNode) &&
						!nextLayer.Contains(childNode))
					{
						nextLayer.Add(childNode);
					}
				}
			}

			return nextLayer;
		}

		private static Tuple<PathNode<T>, PathNode<T>> FindMatchingNodes(
			HashSet<PathNode<T>> forwardFrontier,
			HashSet<PathNode<T>> reverseFrontier)
		{
			PathNode<T> forwardMatch = forwardFrontier.FirstOrDefault(node => reverseFrontier.Contains(node));

			if (forwardMatch != null)
			{
				PathNode<T> reverseMatch = reverseFrontier.First(node => node.Equals(forwardMatch));
				return Tuple.Create(forwardMatch, reverseMatch);
			}
			else
			{
				return null;
			}
		}

		private IEnumerable<T> ConstructSolution(Tuple<PathNode<T>, PathNode<T>> matchingNodes)
		{
			var nodeOnForwardPath = matchingNodes.Item1;
			var nodeOnReversePath = matchingNodes.Item2;
			var pathFromStart = nodeOnForwardPath.GetPath();
			var pathToEnd = nodeOnReversePath.GetPathToRoot();

			if (RepairReversePath != null)
				RepairReversePath(pathToEnd);

			return pathFromStart.Concat(pathToEnd.Skip(1)); // skip the common node
		}
	}

}
