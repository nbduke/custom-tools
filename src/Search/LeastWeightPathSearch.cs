using System;
using System.Collections.Generic;

using Tools.DataStructures;

namespace Tools.Algorithms.Search {

	/*
	 * Least-weight path search (a.k.a. uniform cost search) explores nodes
	 * in order by ascending cumulative path weight. It can find the optimal
	 * (i.e. least-total-weight) path between two nodes in a graph with no
	 * negative cycles.
	 */
	public class LeastWeightPathSearch<T>
	{
		private readonly ChildGenerator<T> GetChildren;
		private readonly EdgeWeightCalculator<T> GetEdgeWeight;

		public LeastWeightPathSearch(
			ChildGenerator<T> getChildren,
			EdgeWeightCalculator<T> getEdgeWeight)
		{
			if (getChildren == null)
				throw new ArgumentNullException("getChildren");
			if (getEdgeWeight == null)
				throw new ArgumentNullException("getEdgeWeight");

			GetChildren = getChildren;
			GetEdgeWeight = getEdgeWeight;
		}

		public IEnumerable<T> FindPath(T start, T end)
		{
			if (start == null || end == null)
				return null;

			PathNode<T> terminalNode = FindNode(start, node => node.Equals(end));
			if (terminalNode == null)
				return new T[] { };
			else
				return terminalNode.GetPath();
		}

		public PathNode<T> FindNode(
			T start,
			NodePredicate<T> nodePredicate,
			uint maxSearchDistance = uint.MaxValue)
		{
			if (nodePredicate == null)
				throw new ArgumentNullException("nodePredicate");
			else if (maxSearchDistance == 0)
				return null;

			var frontier = BinaryHeap<PathNode<T>>.CreateMinFirstHeap();
			var explored = new HashSet<PathNode<T>>();
			frontier.Enqueue(new PathNode<T>(start), 0);

			while (frontier.Count > 0)
			{
				PathNode<T> currentNode = frontier.Dequeue().Value;
				if (nodePredicate(currentNode))
					return currentNode;
				else if (currentNode.CumulativePathLength >= maxSearchDistance)
					continue;

				explored.Add(currentNode);
				foreach (var child in GetChildren(currentNode.State))
				{
					double weight = GetEdgeWeight(currentNode.State, child);
					var childNode = new PathNode<T>(child, currentNode, weight);

					if (!explored.Contains(childNode))
					{
						double priorPathWeight = 0;
						if (!frontier.TryGetPriority(childNode, ref priorPathWeight) ||
							 priorPathWeight > childNode.CumulativePathWeight)
						{
							frontier.Enqueue(childNode, childNode.CumulativePathWeight);
						}
					}
				}
			}

			// Path not found
			return null;
		}
	}

}
