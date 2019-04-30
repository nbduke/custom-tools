using System;
using System.Collections.Generic;

using Tools.DataStructures;

namespace Tools.Algorithms.Search {

	/// <summary>
	/// Least-weight path search (a.k.a. uniform cost search) explores nodes
	/// in order by ascending cumulative path weight, allowing it to find the
	/// least-total-weight path between two nodes.
	/// </summary>
	/// <typeparam name="T">the type of nodes in the graph</typeparam>
	public class LeastWeightPathSearch<T>
	{
		private readonly ChildGenerator<T> GetChildren;
		private readonly EdgeWeightCalculator<T> GetEdgeWeight;

		public LeastWeightPathSearch(
			ChildGenerator<T> getChildren,
			EdgeWeightCalculator<T> getEdgeWeight)
		{
			Validate.IsNotNull(getChildren, "getChildren");
			Validate.IsNotNull(getEdgeWeight, "getEdgeWeight");

			GetChildren = getChildren;
			GetEdgeWeight = getEdgeWeight;
		}

		public IEnumerable<T> FindPath(T start, T end)
		{
			Validate.IsNotNull(start, "start");
			Validate.IsNotNull(end, "end");

			PathNode<T> terminalNode = FindNode(start, node => node.Equals(end));
			if (terminalNode == null)
				return new T[] { };
			else
				return terminalNode.GetPath();
		}

		public PathNode<T> FindNode(
			T start,
			NodePredicate<T> nodePredicate,
			uint maxSearchDistance = uint.MaxValue - 1)
		{
			Validate.IsNotNull(nodePredicate, "nodePredicate");

			var frontier = new Heap<PathNode<T>>(n => n.CumulativePathWeight);
			var explored = new HashSet<PathNode<T>>();
			frontier.Push(new PathNode<T>(start));

			while (frontier.Count > 0)
			{
				PathNode<T> currentNode = frontier.Pop();
				if (nodePredicate(currentNode))
					return currentNode;
				else if (
					currentNode.CumulativePathLength >= maxSearchDistance + 1 ||
					explored.Contains(currentNode)
				)
					continue;

				explored.Add(currentNode);
				foreach (var child in GetChildren(currentNode.State))
				{
					double weight = GetEdgeWeight(currentNode.State, child);
					var childNode = new PathNode<T>(child, currentNode, weight);

					if (!explored.Contains(childNode))
						frontier.Push(childNode);
				}
			}

			// Path not found
			return null;
		}
	}

}
