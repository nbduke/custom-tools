/*
 * BreadthFirstSearch.cs
 * 
 * Nathan Duke
 * 8/8/2016
 * 
 * Contains the BreadthFirstSearch class. This class performs a
 * standard breadth-first search of a state space.
 */

using System.Collections.Generic;

namespace CommonTools { namespace Algorithms { namespace Search {

	public class BreadthFirstSearch<T> where T : PathNodeBase
	{
		private GoalTest<T> IsGoal;
		private ChildGenerator<T> GetChildren;

		public BreadthFirstSearch(
			GoalTest<T> isGoal,
			ChildGenerator<T> getChildren)
		{
			IsGoal = isGoal;
			GetChildren = getChildren;
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
				foreach (T child in GetChildren(currentNode))
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

}}}
