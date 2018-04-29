using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.Algorithms;
using Tools.DataStructures;

namespace Test
{
	[TestClass]
	public class ArrangementComponentTests
	{
		#region Test methods
		[TestMethod]
		public void TestGetPairs()
		{
			char[] sourceSet = "ABCD".ToCharArray();
			var arrangement = new Arrangement<char>(sourceSet);

			char[][] expectedPairs =
			{
				new char[] { 'A', 'B' },
				new char[] { 'A', 'C' },
				new char[] { 'A', 'D' },
				new char[] { 'B', 'C' },
				new char[] { 'B', 'D' },
				new char[] { 'C', 'D' }
			};

			// Act
			var actualPairs = arrangement.GetPairs();

			// Assert
			Assert.AreEqual(expectedPairs.Length, actualPairs.Count());

			int currentIndex = 0;
			foreach (var pair in actualPairs)
			{
				CollectionAssert.AreEqual(expectedPairs[currentIndex], pair.ToArray());
				++currentIndex;
			}
		}

		[TestMethod]
		public void TestZeroSizedCombinations()
		{
			// Arrange
			char[] sourceSet = "ABCD".ToCharArray();
			var arrangement = new Arrangement<char>(sourceSet);
			uint fixedSize = 0;
			uint expectedNumberOfCombinations = 0;

			// Act
			var combinations = arrangement.GetCombinations(fixedSize);

			// Assert
			VerifySizesMatch(expectedNumberOfCombinations, fixedSize, fixedSize, combinations);
		}

		[TestMethod]
		public void TestFixedSizeCombinations()
		{
			// Arrange
			char[] sourceSet = "ABCD".ToCharArray();
			var arrangement = new Arrangement<char>(sourceSet);
			uint fixedSize = 3;

			char[][] expected =
			{
				new char[] { 'A', 'B', 'C' },
				new char[] { 'A', 'B', 'D' },
				new char[] { 'A', 'C', 'D' },
				new char[] { 'B', 'C', 'D' }
			};

			// Act
			var actual = arrangement.GetCombinations(fixedSize);

			// Assert
			Assert.AreEqual(expected.Length, actual.Count);
			for (int i = 0; i < expected.Length; ++i)
			{
				CollectionAssert.AreEqual(expected[i], actual[i]);
			}
		}

		[TestMethod]
		public void TestVariableSizeCombinations()
		{
			// Arrange
			char[] sourceSet = "ABCD".ToCharArray();
			var arrangement = new Arrangement<char>(sourceSet);
			uint minimumSize = 1;
			uint maximumSize = 2;

			char[][] expected =
			{
				new char[] { 'A' },
				new char[] { 'A', 'B' },
				new char[] { 'A', 'C' },
				new char[] { 'A', 'D' },
				new char[] { 'B' },
				new char[] { 'B', 'C' },
				new char[] { 'B', 'D' },
				new char[] { 'C' },
				new char[] { 'C', 'D' },
				new char[] { 'D' }
			};

			// Act
			var actual = arrangement.GetCombinations(minimumSize, maximumSize);

			// Assert
			Assert.AreEqual(expected.Length, actual.Count);
			for (int i = 0; i < expected.Length; ++i)
			{
				CollectionAssert.AreEqual(expected[i], actual[i]);
			}
		}

		[TestMethod]
		public void TestZeroSizedPermutations()
		{
			// Arrange
			char[] sourceSet = "ABCD".ToCharArray();
			var arrangement = new Arrangement<char>(sourceSet);
			uint fixedSize = 0;
			uint expectedNumberOfPermutations = 0;

			// Act
			var combinations = arrangement.GetPermutations(fixedSize);

			// Assert
			VerifySizesMatch(expectedNumberOfPermutations, fixedSize, fixedSize, combinations);
		}

		[TestMethod]
		public void TestSmallFixedSizePermutations()
		{
			// Arrange
			char[] sourceSet = "ABCD".ToCharArray();
			var arrangement = new Arrangement<char>(sourceSet);
			uint fixedSize = 2;

			char[][] expected =
			{
				new char[] { 'A', 'B' },
				new char[] { 'A', 'C' },
				new char[] { 'A', 'D' },
				new char[] { 'B', 'A' },
				new char[] { 'B', 'C' },
				new char[] { 'B', 'D' },
				new char[] { 'C', 'A' },
				new char[] { 'C', 'B' },
				new char[] { 'C', 'D' },
				new char[] { 'D', 'A' },
				new char[] { 'D', 'B' },
				new char[] { 'D', 'C' }
			};

			// Act
			var actual = arrangement.GetPermutations(fixedSize);

			// Assert
			Assert.AreEqual(expected.Length, actual.Count);
			for (int i = 0; i < expected.Length; ++i)
			{
				CollectionAssert.AreEqual(expected[i], actual[i]);
			}
		}

		[TestMethod]
		public void TestSmallVariableSizePermutations()
		{
			// Arrange
			char[] sourceSet = "ABCD".ToCharArray();
			var arrangement = new Arrangement<char>(sourceSet);
			uint minimumSize = 1;
			uint maximumSize = 2;

			char[][] expected =
			{
				new char[] { 'A' },
				new char[] { 'A', 'B' },
				new char[] { 'A', 'C' },
				new char[] { 'A', 'D' },
				new char[] { 'B' },
				new char[] { 'B', 'A' },
				new char[] { 'B', 'C' },
				new char[] { 'B', 'D' },
				new char[] { 'C' },
				new char[] { 'C', 'A' },
				new char[] { 'C', 'B' },
				new char[] { 'C', 'D' },
				new char[] { 'D' },
				new char[] { 'D', 'A' },
				new char[] { 'D', 'B' },
				new char[] { 'D', 'C' }
			};

			// Act
			var actual = arrangement.GetPermutations(minimumSize, maximumSize);

			// Assert
			Assert.AreEqual(expected.Length, actual.Count);
			for (int i = 0; i < expected.Length; ++i)
			{
				CollectionAssert.AreEqual(expected[i], actual[i]);
			}
		}

		[TestMethod]
		public void TestSizeOfLargePermutation()
		{
			// Arrange
			int[] input = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var arrangement = new Arrangement<int>(input);
			uint fixedSize = 5;
			uint expectedNumberOfPermutations = 30240;

			// Act
			var permutations = arrangement.GetPermutations(fixedSize);

			// Assert
			VerifySizesMatch(expectedNumberOfPermutations, fixedSize, fixedSize, permutations);
		}
		#endregion

		#region Helper methods
		private void VerifySizesMatch<T>(
			uint expectedCombinationCount,
			uint expectedMinimumCombinationSize,
			uint expectedMaximumCombinationSize,
			List<List<T>> combinations)
		{
			Assert.AreEqual(expectedCombinationCount, (uint)combinations.Count);

			foreach (var combination in combinations)
			{
				Assert.IsTrue((uint)combination.Count >= expectedMinimumCombinationSize &&
							  (uint)combination.Count <= expectedMaximumCombinationSize);
			}
		}
		#endregion
	}
}
