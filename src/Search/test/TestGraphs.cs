using Tools.Algorithms.Search;

namespace Test {

	public static class TestGraphs
	{
		public static ChildGenerator<T> EdgelessGraph<T>()
		{
			return state =>
			{
				return new T[] { };
			};
		}

		/*
		 * 1 -- 2 -- 3 -- ...
		 */
		public static ChildGenerator<int> OnePathGraph()
		{
			return state =>
			{
				return new int[] { state + 1 };
			};
		}

		/*
		 * 1 -- 2 -- 3 -- ... -- maxState
		 */
		public static ChildGenerator<int> FiniteGraph(int maxState)
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
		public static ChildGenerator<int> BinaryTree()
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
		public static ChildGenerator<int> OneCycleGraph(int cycleLength)
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
		public static ChildGenerator<int> TwoAsymmetricalPathsGraph()
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
	};

}
