/*
 * Combinatorics.cs
 * 
 * Nathan Duke
 * 8/8/2016
 * 
 * Contains the Combinatorics static class. This class exposes mathematical
 * functions for performing factorials, combinations, and permutationls.
 */

using System;

namespace CommonTools { namespace Math {

	public static class Combinatorics
	{
		// Computes the number of combinations of n choosing r (i.e. nCr) from
		// a set without replacement.
		public static ulong GetNumberOfCombinations(uint n, uint r)
		{
			if (r > n)
				throw new ArgumentException("In nCr, r cannot exceed n.");

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

		// Computes the number of permutations of n choosing r (i.e. nPr) from
		// a set without replacement.
		public static ulong GetNumberOfPermutations(uint n, uint r)
		{
			if (r > n)
				throw new ArgumentException("In nPr, r cannot exceed n.");

			if (r == n)
				return Factorial(n); // n!
			else
				return FactorialRatio(n, n - r); // n!/(n - r)!
		}

		// Returns the factorial of x (i.e. x!).
		public static ulong Factorial(uint x)
		{
			if (x <= 1)
				return 1;

			ulong value = x;
			while (--x > 1)
			{
				value *= x;
			}

			return value;
		}

		// Returns the ratio of the factorial of n to the factorial of r (i.e. n!/r!).
		public static ulong FactorialRatio(uint n, uint r)
		{
			if (r > n)
				throw new ArgumentException("FactorialRatio computes an integer ratio, so r cannot exceed n.");

			if (n == r || n == 0)
				return 1;

			ulong value = n;
			while (--n > r)
			{
				value *= n;
			}

			return value;
		}
	}

}}
