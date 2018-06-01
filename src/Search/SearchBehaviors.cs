using System;
using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/// <summary>
	/// Specifies behaviors at each node for a backtracking search algorithm.
	/// </summary>
	public enum NodeOption
	{
		Stop,
		Continue,
		BacktrackThenContinue
	}

	/*
	 * A set of delegates that define injectable behaviors for search algorithms.
	 */
	public delegate IEnumerable<T> ChildGenerator<T>(T state);
	public delegate double EdgeWeightCalculator<T>(T parent, T child);
	public delegate bool NodePredicate<T>(PathNode<T> node);
	public delegate NodeOption NodeAction<T>(PathNode<T> node);
}
