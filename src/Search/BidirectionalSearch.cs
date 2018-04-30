using System.Collections.Generic;
using System.Linq;

namespace Tools.Algorithms.Search {

	/*
	 * Bidirectional search is a breadth-first search starting from two
	 * distinct nodes simultaneously. It is guaranteed to find the shortest
	 * path between the two nodes, if one exists.
	 */
	public class BidirectionalSearch<T> where T : PathNodeBase
	{
		private ChildGenerator<T> GetForwardChildren;
		private ChildGenerator<T> GetReverseChildren;
		private CustomPathMergeFunction<T> MergePaths;

		public BidirectionalSearch(
			ChildGenerator<T> getForwardChildren,
			ChildGenerator<T> getReverseChildren)
			: this(getForwardChildren, getReverseChildren, DefaultMergePaths)
		{
		}

		private static void DefaultMergePaths(T leafNodeFromStart, T leafNodeFromGoal)
		{
			T goal = (T)leafNodeFromGoal.GetRoot();
			leafNodeFromGoal.Parent.InvertPath();
			goal.JoinPath(leafNodeFromStart);
		}

		public BidirectionalSearch(
			ChildGenerator<T> getForwardChildren,
			ChildGenerator<T> getReverseChildren,
			CustomPathMergeFunction<T> mergePaths)
		{
			GetForwardChildren = getForwardChildren;
			GetReverseChildren = getReverseChildren;
			MergePaths = mergePaths;
		}

		public T Search(T start, T goal)
		{
			if (start == null || start.Equals(goal))
				return start;

			Queue<T> forwardFrontier = new Queue<T>();
			Queue<T> reverseFrontier = new Queue<T>();
			HashSet<T> explored = new HashSet<T>();

			// Pre-load the frontiers of the start and end goal
			foreach (T child in GetForwardChildren(start))
			{
				if (child.Equals(goal)) // check here if start and goal are connected
					return child;
				else
					forwardFrontier.Enqueue(child);
			}

			foreach (T child in GetReverseChildren(goal))
			{
				reverseFrontier.Enqueue(child);
			}

			explored.Add(start);
			explored.Add(goal);

			while (forwardFrontier.Count > 0 && reverseFrontier.Count > 0)
			{
				if (CheckForSolution(forwardFrontier, reverseFrontier))
					return goal;

				T forwardNode = forwardFrontier.Dequeue();
				explored.Add(forwardNode);
				foreach (T child in GetForwardChildren(forwardNode))
				{
					if (!explored.Contains(child) && !forwardFrontier.Contains(child))
						forwardFrontier.Enqueue(child);
				}

				if (CheckForSolution(forwardFrontier, reverseFrontier))
					return goal;

				T reverseNode = reverseFrontier.Dequeue();
				explored.Add(reverseNode);
				foreach (T child in GetReverseChildren(reverseNode))
				{
					if (!explored.Contains(child) && !reverseFrontier.Contains(child))
						reverseFrontier.Enqueue(child);
				}
			}

			// Path not found
			return null;
		}

		private bool CheckForSolution(
			Queue<T> forwardFrontier,
			Queue<T> reverseFrontier)
		{
			HashSet<T> forwardSet = new HashSet<T>(forwardFrontier);
			forwardSet.IntersectWith(reverseFrontier);

			if (forwardSet.Count > 0)
			{
				T forwardNode = forwardSet.First();
				T reverseNode = null;

				foreach (T node in reverseFrontier)
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
