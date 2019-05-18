using System;
using System.Collections.Generic;

using Tools.DataStructures;

namespace Tools.Algorithms.Search {

	/// <summary>
	/// A* search explores nodes in order by ascending estimated total path
	/// weight. The total path weight for a node N is the cumulative path
	/// weight from the start to N plus the estimated weight from N to the
	/// goal.<para/>
	/// Algorithm correctness requires that the weight estimate be admissible
	/// and consistent; that is, it must never overestimate the weight, and it
	/// must satisfy the triangle rule.
	/// </summary>
	/// <typeparam name="T">the type of nodes in the graph</typeparam>
	public class AStarSearch<T>
	{
		private readonly Func<T, IEnumerable<Tuple<T, double>>> GetWeightedChildren;

		public AStarSearch(Func<T, IEnumerable<Tuple<T, double>>> getWeightedChildren)
		{
			Validate.IsNotNull(getWeightedChildren, "getWeightedChildren");
			GetWeightedChildren = getWeightedChildren;
		}

		public IEnumerable<T> FindPath(
			T start,
			T end,
			Func<T, double> estimateRemainingPathWeight
		)
		{
			Validate.IsNotNull(end, "end");

			PathNode<T> terminalNode = FindNode(
				start,
				node => node.Equals(end),
				estimateRemainingPathWeight
			);

			if (terminalNode == null)
				return new T[] { };
			else
				return terminalNode.GetPath();
		}

		public PathNode<T> FindNode(
			T start,
			Func<PathNode<T>, bool> nodePredicate,
			Func<T, double> estimateRemainingPathWeight,
			uint maxSearchDistance = uint.MaxValue - 1
		)
		{
			Validate.IsNotNull(nodePredicate, "nodePredicate");
			Validate.IsNotNull(estimateRemainingPathWeight, "estimateRemainingPathWeight");

			var frontier = new Heap<Tuple<PathNode<T>, double>>(t => t.Item2);
			var explored = new HashSet<PathNode<T>>();
			var startNode = new PathNode<T>(start);
			frontier.Push(Tuple.Create(startNode, 0.0));

			while (frontier.Count > 0)
			{
				var currentNode = frontier.Pop().Item1;
				if (nodePredicate(currentNode))
					return currentNode;
				else if (
					currentNode.CumulativePathLength >= maxSearchDistance + 1 ||
					explored.Contains(currentNode)
				)
					continue;

				explored.Add(currentNode);
				foreach (var childAndWeight in GetWeightedChildren(currentNode.State))
				{
					T child = childAndWeight.Item1;
					double weight = childAndWeight.Item2;
					var childNode = new PathNode<T>(child, currentNode, weight);

					if (!explored.Contains(childNode))
					{
						double totalWeightEstimate =
							childNode.CumulativePathWeight +
							estimateRemainingPathWeight(child);
						frontier.Push(Tuple.Create(childNode, totalWeightEstimate));
					}
				}
			}

			// Path not found
			return null;
		}
	}

}