using System;
using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/*
	 * NodeOption specifies behaviors at each node for a backtracking search
	 * algorithm.
	 */
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
	public delegate IEnumerable<Tuple<T, double>> WeightedChildGenerator<T>(T state);
	public delegate bool NodePredicate<T>(PathNode<T> node);
	public delegate NodeOption NodeAction<T>(PathNode<T> node);
}
