/*
 * BacktrackingSearch.cs
 * 
 * Nathan Duke
 * 8/8/2016
 * 
 * Contains the BacktrackingSearch class.
 * 
 * Backtracking search is a memory-optimized depth-first search. It does not store an
 * explored set nor a frontier. Only elements on the current search path are kept in
 * memory, allowing for minimal memory consumption at the expense of potentially
 * exploring redundant sub-paths.
 */


namespace CommonTools { namespace Algorithms { namespace Search {

	public class BacktrackingSearch<T> where T : PathNodeBase
	{
		private GoalTest<T> IsGoal;
		private ChildEnumerator<T> GetChildren;

		public BacktrackingSearch(
			GoalTest<T> isGoal,
			ChildEnumerator<T> getChildren)
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

			return SearchHelper(start, maxSearchDistance);
		}

		private T SearchHelper(T currentNode, uint maxSearchDistance)
		{
			var enumerator = GetChildren(currentNode);
			while (enumerator.MoveNext())
			{
				T child = enumerator.Current;
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

}}}
