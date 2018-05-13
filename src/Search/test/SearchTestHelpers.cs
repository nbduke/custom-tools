using System.Collections.Generic;

using Tools.Algorithms.Search;

namespace Test {

	public static class SearchTestHelpers
	{
		public static IEnumerable<T> AnyChildGenerator<T>(T state)
		{
			yield return default(T);
		}

		public static bool AnyNodePredicate<T>(PathNode<T> node)
		{
			return true;
		}
	}

}
