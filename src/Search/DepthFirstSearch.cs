using System;
using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/// <summary>
	/// Implements a depth-first search of a graph.
	/// </summary>
	/// <typeparam name="T">the type of nodes in the graph</typeparam>
	public class DepthFirstSearch<T>
	{
		private readonly Func<T, IEnumerable<T>> GetChildren;

		public DepthFirstSearch(Func<T, IEnumerable<T>> getChildren)
		{
			Validate.IsNotNull(getChildren, "getChildren");

			GetChildren = getChildren;
		}

		public IEnumerable<T> FindPath(
			T start,
			T end,
			uint maxPathLength = uint.MaxValue
		)
		{
			Validate.IsNotNull(end, "end");

			PathNode<T> terminalNode = FindNode(
				start,
				state => state.Equals(end),
				maxPathLength
			);
			if (terminalNode == null)
				return new T[] { };
			else
				return terminalNode.GetPath();
		}

		public PathNode<T> FindNode(
			T start,
			Func<T, bool> nodePredicate,
			uint maxPathLength = uint.MaxValue
		)
		{
			Validate.IsNotNull(nodePredicate, "nodePredicate");

			var startNode = new PathNode<T>(start);
			if (maxPathLength == 0)
				return null;
			if (nodePredicate(start))
				return startNode;

			var frontier = new Stack<PathNode<T>>();
			var explored = new HashSet<T>();
			frontier.Push(startNode);

			while (frontier.Count > 0)
			{
				PathNode<T> currentNode = frontier.Pop();
				if (explored.Contains(currentNode.State))
					continue;

				explored.Add(currentNode.State);
				foreach (T child in GetChildren(currentNode.State))
				{
					if (!explored.Contains(child))
					{
						var childNode = new PathNode<T>(child, currentNode);
						if (nodePredicate(child))
							return childNode;
						else if (childNode.CumulativePathLength < maxPathLength)
							frontier.Push(childNode);
					}
				}
			}

			// Path not found
			return null;
		}
	}

}
