using System;
using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/// <summary>
	/// Specifies a behavior for FlexibleBacktrackingSearch to take at each node.
	/// </summary>
	public enum NodeOption
	{
		// Terminates the algorithm (this makes the algorithm identical to BacktrackingSearch)
		Stop,

		// The algorithm extends the search path through the current node
		Continue,

		// The algorithm backtracks to the current node's parent and continues with the next child
		Backtrack
	}

	/// <summary>
	/// Like BacktrackingSearch, FlexibleBacktrackingSearch is a memory-optimized
	/// depth-first search, but rather than terminating when a particular node is found,
	/// FlexibleBacktrackingSearch enables searching over arbitrary regions of the graph,
	/// applying a customizable action at each node.
	/// </summary>
	public class FlexibleBacktrackingSearch<T>
	{
		private readonly Func<T, IEnumerable<T>> GetChildren;
		private readonly bool AssumeChildrenNotInPath;
		private Func<PathNode<T>, NodeOption> ProcessNode;
		private uint MaxPathLength;

		/// <summary>
		/// Creates a FlexibleBacktrackingSearch.
		/// </summary>
		/// <param name="getChildren">generates child states</param>
		/// <param name="assumeChildrenNotInPath">if true, the algorithm will not check
		/// whether each child is already in the current path. BEWARE: This is a performance
		/// optimization for special cases. If in doubt, leave this false.</param>
		public FlexibleBacktrackingSearch(
			Func<T, IEnumerable<T>> getChildren,
			bool assumeChildrenNotInPath = false)
		{
			Validate.IsNotNull(getChildren, "getChildren");

			GetChildren = getChildren;
			AssumeChildrenNotInPath = assumeChildrenNotInPath;
		}

		public void Search(
			T start,
			Func<PathNode<T>, NodeOption> processNode,
			uint maxPathLength = uint.MaxValue)
		{
			Validate.IsNotNull(start, "start");
			Validate.IsNotNull(processNode, "processNode");

			if (maxPathLength == 0)
				return;

			var startNode = new PathNode<T>(start);
			switch (processNode(startNode))
			{
				case NodeOption.Stop:
				case NodeOption.Backtrack:
					return;
				case NodeOption.Continue:
					break;
				default:
					throw new ArgumentException("Unknown NodeOption");
			}

			ProcessNode = processNode;
			MaxPathLength = maxPathLength;
			SearchHelper(startNode);
		}

		private bool SearchHelper(PathNode<T> currentNode)
		{
			foreach (T child in GetChildren(currentNode.State))
			{
				if (AssumeChildrenNotInPath || !currentNode.PathContains(child))
				{
					var childNode = new PathNode<T>(child, currentNode);
					switch (ProcessNode(childNode))
					{
						case NodeOption.Stop:
							return false;
						case NodeOption.Continue:
							break;
						case NodeOption.Backtrack:
							continue;
						default:
							throw new ArgumentException("Unknown NodeOption");
					}

					if (childNode.CumulativePathLength < MaxPathLength)
					{
						if (!SearchHelper(childNode))
							return false;
					}
				}
			}

			return true;
		}
	}

}
