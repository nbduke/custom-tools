/*
 * SearchBehaviors.cs
 * 
 * Nathan Duke
 * 8/8/2016
 * 
 * Contains types allowing for injectable behaviors in the Search algorithms
 * in this library.
 */

using System.Collections.Generic;

namespace CommonTools { namespace Algorithms { namespace Search {

	// GoalOption specifies a default set of behaviors at a goal node.
	public enum GoalOption
	{
		Stop,
		Continue,
		BacktrackThenContinue
	}

	// A set of delegates that define injectable behaviors for search algorithms.
	public delegate bool GoalTest<T>(T node);
	public delegate IEnumerable<T> ChildGenerator<T>(T node);
	public delegate IEnumerator<T> ChildEnumerator<T>(T node);
	public delegate GoalOption GoalAction<T>(T node);
	public delegate void CustomPathMergeFunction<T>(T leafNodeFromStart, T leafNodeFromGoal);

}}}
