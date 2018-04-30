namespace Tools.Algorithms.Search {

	/*
	 * Backtracking search is a memory-optimized depth-first search. Instead of keeping
	 * track of the explored set and frontier, only elements on the current search path
	 * are kept in memory. The memory savings come at the expense of potentially
	 * exploring redundant sub-paths.
	 */
	public class BacktrackingSearch<T> where T : PathNode
	{
		private GoalTest<T> IsGoal;

		public BacktrackingSearch(GoalTest<T> isGoal)
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

			return SearchHelper(start, maxSearchDistance);
		}

		private T SearchHelper(T currentNode, uint maxSearchDistance)
		{
			foreach (T child in currentNode.GetChildren())
			{
				if (!currentNode.PathContains(child))
				{
					if (IsGoal(child))
					{
						return child;
					}
					else if (child.CumulativePathLength < maxSearchDistance)
					{
						T returnValue = SearchHelper(child, maxSearchDistance);
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
