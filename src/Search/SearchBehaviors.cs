using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/*
	 * GoalOption specifies a default set of behaviors at a goal node.
	 */
	public enum GoalOption
	{
		Stop,
		Continue,
		BacktrackThenContinue
	}

	/*
	 * A set of delegates that define injectable behaviors for search algorithms.
	 */
	public delegate bool GoalTest<T>(T node);
	public delegate IEnumerable<T> ChildGenerator<T>(T node);
	public delegate IEnumerator<T> ChildEnumerator<T>(T node);
	public delegate GoalOption GoalAction<T>(T node);
	public delegate void CustomPathMergeFunction<T>(T leafNodeFromStart, T leafNodeFromGoal);

}
