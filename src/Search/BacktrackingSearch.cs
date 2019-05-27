using System;
using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/// <summary>
	/// Backtracking search is a memory-optimized depth-first search.<para/>
	/// Instead of keeping track of the explored set and frontier, only elements
	/// on the current search path are kept in memory. The memory savings come at
	/// the expense of potentially exploring redundant sub-paths.
	/// </summary>
	public class BacktrackingSearch<T>
	{
		private readonly Func<T, IEnumerable<T>> GetChildren;
		private readonly bool AssumeChildrenNotInPath;
		private Func<T, bool> Predicate;
		private uint MaxPathLength;

		/// <summary>
		/// Creates a BacktrackingSearch.
		/// </summary>
		/// <param name="getChildren">generates child states</param>
		/// <param name="assumeChildrenNotInPath">if true, the algorithm will not check
		/// whether each child is already in the current path. BEWARE: This is a performance
		/// optimization for special cases. If in doubt, leave this false.</param>
		public BacktrackingSearch(
			Func<T, IEnumerable<T>> getChildren,
			bool assumeChildrenNotInPath = false
		)
		{
			Validate.IsNotNull(getChildren, "getChildren");

			GetChildren = getChildren;
			AssumeChildrenNotInPath = assumeChildrenNotInPath;
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
			uint maxPathLength = uint.MaxValue)
		{
			Validate.IsNotNull(start, "start");
			Validate.IsNotNull(predicate, "predicate");

			var startNode = new PathNode<T>(start);
			if (maxPathLength == 0)
				return null;
			if (predicate(start))
				return startNode;

			Predicate = predicate;
			MaxPathLength = maxPathLength;
			return SearchHelper(startNode);
		}

		private PathNode<T> SearchHelper(PathNode<T> currentNode)
		{
			foreach (T child in GetChildren(currentNode.State))
			{
				var childNode = new PathNode<T>(child, currentNode);
				if (AssumeChildrenNotInPath || !currentNode.PathContains(child))
				{
					if (Predicate(childNode.State))
					{
						return childNode;
					}
					else if (childNode.CumulativePathLength < MaxPathLength)
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
