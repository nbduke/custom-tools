using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/*
	 * Implements a breadth-first search of a graph.
	 */
	public class BreadthFirstSearch<T> where T : PathNode
	{
		private GoalTest<T> IsGoal;

		public BreadthFirstSearch(GoalTest<T> isGoal)
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

			Queue<T> frontier = new Queue<T>();
			HashSet<T> explored = new HashSet<T>();
			frontier.Enqueue(start);

			while (frontier.Count > 0)
			{
				T currentNode = frontier.Dequeue();
				explored.Add(currentNode);
				foreach (T child in currentNode.GetChildren())
				{
					if (!explored.Contains(child) && !frontier.Contains(child))
					{
						if (IsGoal(child))
							return child;
						else if (child.CumulativePathLength < maxSearchDistance)
							frontier.Enqueue(child);
					}
				}
			}

			// Path not found
			return null;
		}
	}

}
