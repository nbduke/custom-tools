using System;
using System.Collections.Generic;

using Tools.Algorithms.Search;

namespace Test {

	public static class SearchTestHelpers
	{
		public static IEnumerable<T> AnyChildGenerator<T>(T state)
		{
			yield return default(T);
		}

		public static IEnumerable<Tuple<T, double>> AnyWeightedChildGenerator<T>(T state)
		{
			yield return Tuple.Create(default(T), 0.5);
		}

		public static bool AnyPredicate<T>(T state)
		{
			return true;
		}
	}

}
