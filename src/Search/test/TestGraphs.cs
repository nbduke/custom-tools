using System;
using System.Collections.Generic;
using System.Linq;

namespace Test {

	public static class EdgeWeighter
	{
		public static Func<T, IEnumerable<Tuple<T, double>>> ConstantWeight<T>(
			Func<T, IEnumerable<T>> graph,
			double weight = 1
		)
		{
			return state =>
			{
				return graph(state).Select(c => Tuple.Create(c, weight));
			};
		}

		public static Func<T, IEnumerable<Tuple<T, double>>> VariableWeight<T>(
			Func<T, IEnumerable<T>> graph,
			Func<T, double> getWeight
		)
		{
			return state =>
			{
				return graph(state).Select(c => Tuple.Create(c, getWeight(c)));
			};
		}
	}

	public static class TestGraphs
	{
		public static Func<T, IEnumerable<T>> EdgelessGraph<T>()
		{
			return state =>
			{
				return new T[] { };
			};
		}

		/*
		 * 1 -- 2 -- 3 -- ...
		 */
		public static Func<int, IEnumerable<int>> OnePathGraph()
		{
			return state =>
			{
				return new int[] { state + 1 };
			};
		}

		/*
		 * 1 -- 2 -- 3 -- ... -- maxState
		 */
		public static Func<int, IEnumerable<int>> FiniteGraph(int maxState)
		{
			return state =>
			{
				if (state < maxState)
					return new int[] { state + 1 };
				else
					return new int[] { };
			};
		}

		/*
		 *		1
		 *	  2   3
		 *	 4 5 6 7
		 */
		public static Func<int, IEnumerable<int>> BinaryTree()
		{
			return state =>
			{
				return new int[] { state * 2, state * 2 + 1 };
			};
		}

		/*
		 * 1 ------ 4
		 *  \	   /
		 *   2 -- 3
		 */
		public static Func<int, IEnumerable<int>> OneCycleGraph(int cycleLength)
		{
			return state =>
			{
				return new int[] { (state + 1) % cycleLength };
			};
		}

		/*
		 *   2 -- 4 -- 6
		 *  /			\
		 * 1			 7   Not a cyle (graph is directed from left to right)
		 *  \			/
		 *   3 ------- 5
		 */
		public static Func<int, IEnumerable<int>> TwoAsymmetricalPathsGraph()
		{
			return state =>
			{
				switch (state)
				{
					case 1:
						return new int[] { 3, 2 }; // order to make DFS interesting
					case 2:
						return new int[] { 4 };
					case 3:
						return new int[] { 5 };
					case 4:
						return new int[] { 6 };
					case 5:
					case 6:
						return new int[] { 7 };
					default:
						return new int[] { };
				}
			};
		}

		/*
		 * Same as above, but the reverse child generator.
		 */
		public static Func<int, IEnumerable<int>> ReverseTwoAsymmetricalPathsGraph()
		{
			return state =>
			{
				switch (state)
				{
					case 7:
						return new int[] { 5, 6 };
					case 6:
						return new int[] { 4 };
					case 5:
						return new int[] { 3 };
					case 4:
						return new int[] { 2 };
					case 3:
						return new int[] { 1 };
					case 2:
						return new int[] { 1 };
					default:
						return new int[] { };
				}
			};
		}
	};

}
