using System;
using System.Collections.Generic;

namespace Tools.Math {

	/// <summary>
	/// Exposes helper functions for calculating factorials, combinations,
	/// and permutations.
	/// </summary>
	public static class Combinatorics
	{
		/// <summary>
		/// Computes nCr, the number of ways to choose r items from n
		/// without replacement.
		/// </summary>
		/// <param name="n">the total number of items</param>
		/// <param name="r">the number of items to choose</param>
		public static ulong GetNumberOfCombinations(uint n, uint r)
		{
			Validate.IsTrue(r <= n, "In nCr, r cannot exceed n.");

			if (r > n / 2)
			{
				ulong a = FactorialRatio(n, r); // n!/r!
				ulong b = Factorial(n - r); // (n - r)!
				return a / b;
			}
			else
			{
				ulong a = FactorialRatio(n, n - r); // n!/(n - r)!
				ulong b = Factorial(r); // r!
				return a / b;
			}
		}

		/// <summary>
		/// Computes nPr, the number of ways to arrange r items chosen from n
		/// without replacement.
		/// </summary>
		/// <param name="n">the total number of items</param>
		/// <param name="r">the number of items to choose</param>
		public static ulong GetNumberOfPermutations(uint n, uint r)
		{
			Validate.IsTrue(r <= n, "In nPr, r cannot exceed n.");

			if (r == n)
				return Factorial(n); // n!
			else
				return FactorialRatio(n, n - r); // n!/(n - r)!
		}

		/// <summary>
		/// Returns x!, the factorial of x.
		/// </summary>
		/// <param name="x">a nonnegative number</param>
		public static ulong Factorial(uint x)
		{
			return FactorialRatio(x, 1);
		}

		/// <summary>
		/// Returns n!/r!, the integer ratio of the factorials of n and r.
		/// </summary>
		/// <param name="n">a nonnegative number</param>
		/// <param name="r">a nonnegative number</param>
		public static ulong FactorialRatio(uint n, uint r)
		{
			if (r > n)
				return 0;
			else if (n == r || n == 0)
				return 1;

			ulong value = n;
			while (--n > r)
			{
				value *= n;
			}

			return value;
		}

		/// <summary>
		/// Returns the Cartesian product of two enumerables.
		/// </summary>
		public static IEnumerable<Tuple<T1, T2>> CartesianProduct<T1, T2>(
			IEnumerable<T1> first,
			IEnumerable<T2> second
		)
		{
			Validate.IsNotNull(first, "first");
			Validate.IsNotNull(second, "second");

			foreach (var a in first)
			{
				foreach (var b in second)
				{
					yield return new Tuple<T1, T2>(a, b);
				}
			}
		}
	}

}
