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
		private readonly Func<T, IEnumerable<Tuple<T, double>>> GetWeightedChildren;

		public LeastWeightPathSearch(
			Func<T, IEnumerable<Tuple<T, double>>> getWeightedChildren
		)
		{
			Validate.IsNotNull(getWeightedChildren, "getWeightedChildren");

			GetWeightedChildren = getWeightedChildren;
		}

		public IEnumerable<T> FindPath(
			T start,
			T end,
			uint maxSearchDistance = uint.MaxValue - 1
		)
		{
			Validate.IsNotNull(end, "end");

			PathNode<T> terminalNode = FindNode(
				start,
				state => state.Equals(end),
				maxSearchDistance
			);

			if (terminalNode == null)
				return new T[] { };
			else
				return terminalNode.GetPath();
		}

		public PathNode<T> FindNode(
			T start,
			Func<T, bool> nodePredicate,
			uint maxSearchDistance = uint.MaxValue - 1
		)
		{
			Validate.IsNotNull(nodePredicate, "nodePredicate");

			var frontier = new Heap<PathNode<T>>(n => n.CumulativePathWeight);
			var explored = new HashSet<T>();
			frontier.Push(new PathNode<T>(start));

			while (frontier.Count > 0)
			{
				PathNode<T> currentNode = frontier.Pop();
				if (nodePredicate(currentNode.State))
					return currentNode;
				else if (
					currentNode.CumulativePathLength >= maxSearchDistance + 1 ||
					explored.Contains(currentNode.State)
				)
					continue;

				explored.Add(currentNode.State);
				foreach (var childAndWeight in GetWeightedChildren(currentNode.State))
				{
					T child = childAndWeight.Item1;
					double weight = childAndWeight.Item2;
					if (!explored.Contains(child))
						frontier.Push(new PathNode<T>(child, currentNode, weight));
				}
			}

			// Path not found
			return null;
		}
	}

}
