/*
 * DepthFirstSearch.cs
 * 
 * Nathan Duke
 * 8/8/2016
 * 
 * Contains the DepthFirstSearch class. This class performs a
 * standard depth-first search of a state space.
 */

using System.Collections.Generic;

namespace CommonTools { namespace Algorithms { namespace Search {

	public class DepthFirstSearch<T> where T : PathNodeBase
	{
		private GoalTest<T> IsGoal;
		private ChildGenerator<T> GetChildren;

		public DepthFirstSearch(
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

			Stack<T> frontier = new Stack<T>();
			HashSet<T> explored = new HashSet<T>();
			frontier.Push(start);

			while (frontier.Count > 0)
			{
				T currentNode = frontier.Pop();
				explored.Add(currentNode);
				foreach (T child in GetChildren(currentNode))
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

}}}
