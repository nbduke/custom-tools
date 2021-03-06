﻿using System;
using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/// <summary>
	/// Implements a breadth-first search of a graph.
	/// </summary>
	/// <typeparam name="T">the type of nodes in the graph</typeparam>
	public class BreadthFirstSearch<T>
	{
		private readonly Func<T, IEnumerable<T>> GetChildren;

		public BreadthFirstSearch(Func<T, IEnumerable<T>> getChildren)
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
			Func<T, bool> predicate,
			uint maxPathLength = uint.MaxValue
		)
		{
			Validate.IsNotNull(predicate, "predicate");

			var startNode = new PathNode<T>(start);
			if (maxPathLength == 0)
				return null;
			if (predicate(start))
				return startNode;

			var frontier = new Queue<PathNode<T>>();
			var explored = new HashSet<T>();
			frontier.Enqueue(startNode);

			while (frontier.Count > 0)
			{
				PathNode<T> currentNode = frontier.Dequeue();
				if (explored.Contains(currentNode.State))
					continue;

				explored.Add(currentNode.State);
				foreach (T child in GetChildren(currentNode.State))
				{
					if (!explored.Contains(child))
					{
						var childNode = new PathNode<T>(child, currentNode);
						if (predicate(child))
							return childNode;
						else if (childNode.CumulativePathLength < maxPathLength)
							frontier.Enqueue(childNode);
					}
				}
			}

			// Path not found
			return null;
		}
	}

}
