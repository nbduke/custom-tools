using System;

namespace Tools.Algorithms.Search {

	/*
	 * Like BacktrackingSearch, FlexibleBacktrackingSearch is a memory-optimized
	 * depth-first search. FlexibleBacktrackingSearch provides the added ability
	 * to customize how the algorithm behaves when a goal node is found.
	 * 
	 * The GoalAction given to the constructor allows the following behaviors when
	 * a goal is found:
	 *		Stop:					the algorithm terminates (identical to BacktrackingSearch)
	 *		Continue:				the algorithm extends the current search path through the goal
	 *		BacktrackAndContinue:	the algorithm backtracks to the goal's parent and continues with its next child
	 */
	public class FlexibleBacktrackingSearch<T> where T : PathNode
	{
		GoalTest<T> IsGoal;
		GoalAction<T> ProcessGoal;

		public FlexibleBacktrackingSearch(
			GoalTest<T> isGoal,
			GoalAction<T> processGoal)
		{
			IsGoal = isGoal;
			ProcessGoal = processGoal;
		}

		public void Search(T start)
		{
			Search(start, uint.MaxValue);
		}

		public void Search(T start, uint maxSearchDistance)
		{
			if (start == null || maxSearchDistance == 0)
				return;

			if (IsGoal(start))
			{
				GoalOption option = ProcessGoal(start);

				switch (option)
				{
					case GoalOption.Stop:
						return;
					case GoalOption.Continue:
						break;
					case GoalOption.BacktrackThenContinue:
						return;
					default:
						throw new ArgumentException("Unrecognized GoalOption");
				}
			}

			SearchHelper(start, maxSearchDistance);
		}

		private bool SearchHelper(T currentNode, uint maxSearchDistance)
		{
			foreach (T child in currentNode.GetChildren())
			{
				if (!currentNode.PathContains(child))
				{
					if (IsGoal(child))
					{
						GoalOption option = ProcessGoal(child);

						switch (option)
						{
							case GoalOption.Stop:
								return false;
							case GoalOption.Continue:
								break;
							case GoalOption.BacktrackThenContinue:
								continue;
							default:
								throw new ArgumentException("Unrecognized GoalOption");
						}
					}

					if (child.CumulativePathLength < maxSearchDistance)
					{
						if (!SearchHelper(child, maxSearchDistance))
							return false;
					}
				}
			}

			return true;
		}
	}

}
