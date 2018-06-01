using System;
using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/// <summary>
	/// Implements a breadth-first search of a graph.
	/// </summary>
	/// <typeparam name="T">the type of nodes in the graph</typeparam>
	public class BreadthFirstSearch<T>
	{
		private readonly ChildGenerator<T> GetChildren;

		public BreadthFirstSearch(ChildGenerator<T> getChildren)
		{
			Validate.IsNotNull(getChildren, "getChildren");

			GetChildren = getChildren;
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

			var startNode = new PathNode<T>(start);
			if (nodePredicate(startNode))
				return startNode;
			else if (maxSearchDistance == 0)
				return null;

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
						else if (childNode.CumulativePathLength < maxSearchDistance + 1)
							frontier.Enqueue(childNode);
					}
				}
			}

			// Path not found
			return null;
		}
	}

}
