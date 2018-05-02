﻿using System;
using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/*
	 * Implements a depth-first search of a graph.
	 */
	public class DepthFirstSearch<T>
	{
		private readonly ChildGenerator<T> GetChildren;

		public DepthFirstSearch(ChildGenerator<T> getChildren)
		{
			if (getChildren == null)
				throw new ArgumentNullException("getChildren");

			GetChildren = getChildren;
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

			var frontier = new Stack<PathNode<T>>();
			var explored = new HashSet<PathNode<T>>();
			frontier.Push(startNode);

			while (frontier.Count > 0)
			{
				PathNode<T> currentNode = frontier.Pop();
				explored.Add(currentNode);
				foreach (T child in GetChildren(currentNode.State))
				{
					var childNode = new PathNode<T>(child, currentNode);
					if (!explored.Contains(childNode) && !frontier.Contains(childNode))
					{
						if (nodePredicate(childNode))
							return childNode;
						else if (childNode.CumulativePathLength < maxSearchDistance)
							frontier.Push(childNode);
					}
				}
			}

			// Path not found
			return null;
		}
	}

}
