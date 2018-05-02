using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Algorithms.Search {

	/*
	 * Bidirectional search is a breadth-first search starting from two nodes
	 * simultaneously. It is guaranteed to find the shortest path between the two
	 * nodes, if one exists.
	 */
	public class BidirectionalSearch<T>
	{
		private readonly ChildGenerator<T> GetForwardChildren;
		private readonly ChildGenerator<T> GetReverseChildren;
		private CustomPathMergeFunction<T> MergePaths;

		/// <summary>
		/// Creates a BidirectionalSearch for an undirected graph.
		/// </summary>
		/// <param name="getChildrenUndirected">the ChildGenerator for both search paths</param>
		public BidirectionalSearch(ChildGenerator<T> getChildrenUndirected)
			: this(getChildrenUndirected, getChildrenUndirected)
		{
		}

		/// <summary>
		/// Creates a BidirectionalSearch for a directed graph.
		/// </summary>
		/// <param name="getChildren">generates child states</param>
		/// <param name="getParents">generates parent states
		/// (the argument should be a child of all returned states)</param>
		public BidirectionalSearch(
			ChildGenerator<T> getChildren,
			ChildGenerator<T> getParents)
			: this(getChildren, getParents, DefaultMergePaths)
		{
		}

		/// <summary>
		/// Creates a BidirectionalSearch for a directed graph with a custom
		/// function for merging the two search paths.
		/// </summary>
		/// <param name="getChildren">generates child states</param>
		/// <param name="getParents">generates parent states
		/// (the argument should be a child of all returned states)</param>
		/// <param name="mergePaths">combines two search paths into one</param>
		public BidirectionalSearch(
			ChildGenerator<T> getChildren,
			ChildGenerator<T> getParents,
			CustomPathMergeFunction<T> mergePaths)
		{
			if (getChildren == null)
				throw new ArgumentNullException("getChildren");
			if (getParents == null)
				throw new ArgumentNullException("getChildrenInverted");
			if (mergePaths == null)
				throw new ArgumentNullException("mergePaths");

			GetForwardChildren = getChildren;
			GetReverseChildren = getParents;
			MergePaths = mergePaths;
		}

		private static void DefaultMergePaths(
			PathNode<T> leafNodeFromStart,
			PathNode<T> leafNodeFromEnd)
		{
			PathNode<T> goal = leafNodeFromEnd.GetRoot();
			leafNodeFromEnd.Parent.InvertPath();
			goal.JoinPath(leafNodeFromStart);
		}

		public IEnumerable<T> FindPath(T start, T end)
		{
			if (start == null || end == null)
				return new T[] { };
			else if (start.Equals(end))
				return new T[] { start };

			PathNode<T> terminalNode = BuildPath(start, end);
			if (terminalNode == null)
				return new T[] { };
			else
				return terminalNode.GetPath();
		}

		private PathNode<T> BuildPath(T start, T end)
		{
			var startNode = new PathNode<T>(start);
			var endNode = new PathNode<T>(end);

			var forwardFrontier = new Queue<PathNode<T>>();
			var reverseFrontier = new Queue<PathNode<T>>();
			var explored = new HashSet<PathNode<T>>();

			// Pre-load the frontiers of the start and end states
			foreach (T child in GetForwardChildren(start))
			{
				var childNode = new PathNode<T>(child, startNode);
				if (child.Equals(end)) // check here if start and end are connected
					return childNode;
				else
					forwardFrontier.Enqueue(childNode);
			}

			foreach (T child in GetReverseChildren(end))
			{
				reverseFrontier.Enqueue(new PathNode<T>(child, endNode));
			}

			explored.Add(startNode);
			explored.Add(endNode);

			while (forwardFrontier.Count > 0 && reverseFrontier.Count > 0)
			{
				if (CheckForSolution(forwardFrontier, reverseFrontier))
					return endNode;

				PathNode<T> forwardNode = forwardFrontier.Dequeue();
				explored.Add(forwardNode);
				foreach (T child in GetForwardChildren(forwardNode.State))
				{
					var childNode = new PathNode<T>(child, forwardNode);
					if (!explored.Contains(childNode) && !forwardFrontier.Contains(childNode))
						forwardFrontier.Enqueue(childNode);
				}

				if (CheckForSolution(forwardFrontier, reverseFrontier))
					return endNode;

				PathNode<T> reverseNode = reverseFrontier.Dequeue();
				explored.Add(reverseNode);
				foreach (T child in GetReverseChildren(reverseNode.State))
				{
					var childNode = new PathNode<T>(child, reverseNode);
					if (!explored.Contains(childNode) && !reverseFrontier.Contains(childNode))
						reverseFrontier.Enqueue(childNode);
				}
			}

			// Path not found
			return null;
		}

		private bool CheckForSolution(
			Queue<PathNode<T>> forwardFrontier,
			Queue<PathNode<T>> reverseFrontier)
		{
			var forwardSet = new HashSet<PathNode<T>>(forwardFrontier);
			forwardSet.IntersectWith(reverseFrontier);

			if (forwardSet.Count > 0)
			{
				PathNode<T> forwardNode = forwardSet.First();
				PathNode<T> reverseNode = null;

				foreach (PathNode<T> node in reverseFrontier)
				{
					if (node.Equals(forwardNode))
					{
						reverseNode = node;
						break;
					}
				}

				if (reverseNode != null)
				{
					MergePaths(forwardNode, reverseNode);
					return true; // solution found
				}
			}

			return false; // no solution
		}
	}

}
