﻿using System;

namespace Tools.Algorithms.Search {

	/*
	 * Backtracking search is a memory-optimized depth-first search. Instead of keeping
	 * track of the explored set and frontier, only elements on the current search path
	 * are kept in memory. The memory savings come at the expense of potentially
	 * exploring redundant sub-paths.
	 */
	public class BacktrackingSearch<T>
	{
		private readonly ChildGenerator<T> GetChildren;
		private NodePredicate<T> NodePredicate;
		private uint MaxSearchDistance;

		public BacktrackingSearch(ChildGenerator<T> getChildren)
		{
			Validate.IsNotNull(getChildren, "getChildren");

			GetChildren = getChildren;
		}

		public PathNode<T> FindNode(
			T start,
			NodePredicate<T> nodePredicate,
			uint maxSearchDistance = uint.MaxValue)
		{
			Validate.IsNotNull(nodePredicate, "nodePredicate");

			if (maxSearchDistance == 0)
				return null;

			var startNode = new PathNode<T>(start);
			if (nodePredicate(startNode))
				return startNode;

			NodePredicate = nodePredicate;
			MaxSearchDistance = maxSearchDistance;
			return SearchHelper(new PathNode<T>(start));
		}

		private PathNode<T> SearchHelper(PathNode<T> currentNode)
		{
			foreach (T child in GetChildren(currentNode.State))
			{
				var childNode = new PathNode<T>(child, currentNode);
				if (!currentNode.PathContains(child))
				{
					if (NodePredicate(childNode))
					{
						return childNode;
					}
					else if (childNode.CumulativePathLength < MaxSearchDistance)
					{
						PathNode<T> returnValue = SearchHelper(childNode);
						if (returnValue != null)
							return returnValue;
					}
				}
			}

			// Path not found
			return null;
		}
	}

}
