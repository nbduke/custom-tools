using System;

namespace Tools.Algorithms.Search {

	/*
	 * Like BacktrackingSearch, FlexibleBacktrackingSearch is a memory-optimized
	 * depth-first search. Rather than terminating when a particular node is found,
	 * however, FlexibleBacktrackingSearch enables searching over arbitrary regions
	 * of the graph, applying a customizable action at each node.
	 * 
	 * The NodeAction given to the Search method allows one of the following behaviors at
	 * each node:
	 *		Stop:					the algorithm terminates (identical to BacktrackingSearch)
	 *		Continue:				the algorithm extends the search path through the current node
	 *		BacktrackAndContinue:	the algorithm backtracks to the node's parent and continues with its next child
	 */
	public class FlexibleBacktrackingSearch<T>
	{
		private readonly ChildGenerator<T> GetChildren;
		private readonly bool AssumeChildrenNotInPath;
		private NodeAction<T> ProcessNode;
		private uint MaxSearchDistance;

		/// <summary>
		/// Creates a FlexibleBacktrackingSearch.
		/// </summary>
		/// <param name="getChildren">generates child states</param>
		/// <param name="assumeChildrenNotInPath">if true, the algorithm will not check
		/// whether each child is already in the current path. BEWARE: This is a performance
		/// optimization for special cases. If in doubt, leave this false.</param>
		public FlexibleBacktrackingSearch(
			ChildGenerator<T> getChildren,
			bool assumeChildrenNotInPath = false)
		{
			if (getChildren == null)
				throw new ArgumentNullException("getChildren");

			GetChildren = getChildren;
			AssumeChildrenNotInPath = assumeChildrenNotInPath;
		}

		public void Search(
			T start,
			NodeAction<T> processNode,
			uint maxSearchDistance = uint.MaxValue)
		{
			if (processNode == null)
				throw new ArgumentNullException("processNode");
			else if (maxSearchDistance == 0)
				return;

			var startNode = new PathNode<T>(start);
			switch (processNode(startNode))
			{
				case NodeOption.Stop:
					return;
				case NodeOption.Continue:
					break;
				case NodeOption.BacktrackThenContinue:
					return; // there is nothing to backtrack to
				default:
					throw new ArgumentException("Unknown NodeOption");
			}

			ProcessNode = processNode;
			MaxSearchDistance = maxSearchDistance;
			SearchHelper(startNode);
		}

		private bool SearchHelper(PathNode<T> currentNode)
		{
			foreach (T child in GetChildren(currentNode.State))
			{
				var childNode = new PathNode<T>(child, currentNode);
				if (AssumeChildrenNotInPath || !currentNode.PathContains(child))
				{
					switch (ProcessNode(childNode))
					{
						case NodeOption.Stop:
							return false;
						case NodeOption.Continue:
							break;
						case NodeOption.BacktrackThenContinue:
							continue;
						default:
							throw new ArgumentException("Unknown NodeOption");
					}

					if (childNode.CumulativePathLength < MaxSearchDistance)
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
