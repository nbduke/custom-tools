using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/*
	 * Implements a depth-first search of a graph.
	 */
	public class DepthFirstSearch<T> where T : PathNode
	{
		private GoalTest<T> IsGoal;

		public DepthFirstSearch(GoalTest<T> isGoal)
		{
			IsGoal = isGoal;
		}

		public T Search(T start)
		{
			return Search(start, uint.MaxValue);
		}

		public T Search(T start, uint maxSearchDistance)
		{
			if (start == null || IsGoal(start))
				return start;
			else if (maxSearchDistance == 0)
				return null;

			Stack<T> frontier = new Stack<T>();
			HashSet<T> explored = new HashSet<T>();
			frontier.Push(start);

			while (frontier.Count > 0)
			{
				T currentNode = frontier.Pop();
				explored.Add(currentNode);
				foreach (T child in currentNode.GetChildren())
				{
					if (!explored.Contains(child) && !frontier.Contains(child))
					{
						if (IsGoal(child))
							return child;
						else if (child.CumulativePathLength < maxSearchDistance)
							frontier.Push(child);
					}
				}
			}

			// Path not found
			return null;
		}
	}

}
