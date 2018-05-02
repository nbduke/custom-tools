using System;
using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/*
	 * Implements a breadth-first search of a graph.
	 */
	public class BreadthFirstSearch<T>
	{
		private readonly ChildGenerator<T> GetChildren;

		public BreadthFirstSearch(ChildGenerator<T> getChildren)
		{
			if (getChildren == null)
				throw new ArgumentNullException("getChildren");

			GetChildren = getChildren;
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

			var startNode = new PathNode<T>(start);
			if (nodePredicate(startNode))
				return startNode;

			var frontier = new Queue<PathNode<T>>();
			var explored = new HashSet<PathNode<T>>();
			frontier.Enqueue(startNode);

			while (frontier.Count > 0)
			{
				PathNode<T> currentNode = frontier.Dequeue();
				explored.Add(currentNode);
				foreach (T child in GetChildren(currentNode.State))
				{
					var childNode = new PathNode<T>(child, currentNode);
					if (!explored.Contains(childNode) && !frontier.Contains(childNode))
					{
						if (nodePredicate(childNode))
							return childNode;
						else if (childNode.CumulativePathLength < maxSearchDistance)
							frontier.Enqueue(childNode);
					}
				}
			}

			// Path not found
			return null;
		}
	}

}
