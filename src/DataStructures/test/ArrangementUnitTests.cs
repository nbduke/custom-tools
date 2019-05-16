using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Tools.DataStructures;

namespace Test
{
	[TestClass]
	public class ArrangementUnitTests
	{
		#region Constructor
		[TestMethod]
		public void Constructor_WithNullCollection_ThrowsArgumentNullException()
		{
			// Act
			Action action = () =>
			{
				var arrangement = new Arrangement<int>(null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}
		#endregion

		#region Count
		[TestMethod]
		public void Count_AtAnyTime_ReturnsNumberOfItemsInCollection()
		{
			// Arrange
			var arrangement = new Arrangement<double>(new double[] { 0, 0.1, 0.01 });

			// Act
			int count = arrangement.Count;

			// Assert
			Assert.AreEqual(3, count);
		}
		#endregion

		#region GetPairs
		[TestMethod]
		public void GetPairs_CollectionIsEmpty_ReturnsEmptyEnumerable()
		{
			// Arrange
			var arrangement = new Arrangement<char>("");

			// Act
			var pairs = arrangement.GetPairs();

			// Assert
			Assert.AreEqual(0, pairs.Count());
		}

		[TestMethod]
		public void GetPairs_CollectionIsNonempty_ReturnsEnumerableOfCombinationsOfSizeTwo()
		{
			// Arrange
			var arrangement = new Arrangement<char>("ABCD");
			Tuple<char, char>[] expectedPairs =
			{
				Tuple.Create('A', 'B'),
				Tuple.Create('A', 'C'),
				Tuple.Create('A', 'D'),
				Tuple.Create('B', 'C'),
				Tuple.Create('B', 'D'),
				Tuple.Create('C', 'D')
			};

			// Act
			var pairs = arrangement.GetPairs();

			// Assert
			CollectionAssert.AreEqual(expectedPairs, pairs.ToList());
		}
		#endregion

		#region GetOrderedPairs
		[TestMethod]
		public void GetOrderedPairs_CollectionIsEmpty_ReturnsEmptyEnumerable()
		{
			// Arrange
			var arrangement = new Arrangement<char>("");

			// Act
			var pairs = arrangement.GetOrderedPairs();

			// Assert
			Assert.AreEqual(0, pairs.Count());
		}

		[TestMethod]
		public void GetOrderedPairs_CollectionIsNonempty_ReturnsEnumerableOfPermutationsOfSizeTwo()
		{
			// Arrange
			var arrangement = new Arrangement<char>("BEE");
			Tuple<char, char>[] expectedPairs =
			{
				Tuple.Create('B', 'E'),
				Tuple.Create('B', 'E'),
				Tuple.Create('E', 'B'),
				Tuple.Create('E', 'E'),
				Tuple.Create('E', 'B'),
				Tuple.Create('E', 'E')
			};

			// Act
			var pairs = arrangement.GetOrderedPairs();

			// Assert
			CollectionAssert.AreEqual(expectedPairs, pairs.ToList());
		}
		#endregion

		#region GetCombinations
		[TestMethod]
		public void GetCombinations_MaximumSizeIsLargerThanCollection_ThrowsArgumentException()
		{
			// Arrange
			var arrangement = new Arrangement<char>("A");

			// Act
			Action action = () =>
			{
				var combinations = arrangement.GetCombinations(2);
			};

			// Assert
			Assert.ThrowsException<ArgumentException>(action);
		}

		[TestMethod]
		public void GetCombinations_MinimumSizeIsLargerThanMaximumSize_ThrowsArgumentException()
		{
			// Arrange
			var arrangement = new Arrangement<char>("foo");

			// Act
			Action action = () =>
			{
				var combinations = arrangement.GetCombinations(2, 1);
			};

			// Assert
			Assert.ThrowsException<ArgumentException>(action);
		}

		[TestMethod]
		public void GetCombinations_CollectionIsEmpty_ReturnsEmptyEnumerable()
		{
			// Arrange
			var arrangement = new Arrangement<char>("");

			// Act
			var combinations = arrangement.GetCombinations(0);

			// Assert
			Assert.AreEqual(0, combinations.Count());
		}

		[TestMethod]
		public void GetCombinations_CollectionIsNonemptyButGivenSizeIsZero_ReturnsEmptyEnumerable()
		{
			// Arrange
			var arrangement = new Arrangement<char>("Hello");

			// Act
			var combinations = arrangement.GetCombinations(0);

			// Assert
			Assert.AreEqual(0, combinations.Count());
		}

		[TestMethod]
		public void GetCombinations_SizeIsOne_ReturnsElementsFromCollectionOneAtATime()
		{
			// Arrange
			string collection = "beef";
			var arrangement = new Arrangement<char>(collection);

			// Act
			var combinations = arrangement.GetCombinations(1).ToList();

			// Assert
			Assert.AreEqual(collection.Length, combinations.Count);
			for (int i = 0; i < combinations.Count && i < collection.Length; ++i)
			{
				Assert.AreEqual(1, combinations[i].Count);
				Assert.AreEqual(collection[i], combinations[i][0]);
			}
		}

		[TestMethod]
		public void GetCombinations_SizeIsAllOfCollection_ReturnsCollectionAllAtOnce()
		{
			// Arrange
			string collection = "dog";
			var arrangement = new Arrangement<char>(collection);

			// Act
			var combinations = arrangement.GetCombinations((uint)collection.Length).ToList();

			// Assert
			Assert.AreEqual(1, combinations.Count);
			CollectionAssert.AreEqual(collection.ToCharArray(), combinations[0]);
		}

		[TestMethod]
		public void GetCombinations_OneSizeSpecified_ReturnsAllCombinationsOfThatSize()
		{
			// Arrange
			var arrangement = new Arrangement<int>(new int[] { 1, 2, 4, 8 });
			var expectedCombinations = new int[][]
			{
				new int[] { 1, 2, 4 },
				new int[] { 1, 2, 8 },
				new int[] { 1, 4, 8 },
				new int[] { 2, 4, 8 }
			};

			// Act
			var combinations = arrangement.GetCombinations(3).ToList();

			// Assert
			Assert.AreEqual(expectedCombinations.Length, combinations.Count);
			for (int i = 0; i < combinations.Count && i < expectedCombinations.Length; ++i)
			{
				CollectionAssert.AreEqual(expectedCombinations[i], combinations[i]);
			}
		}

		[TestMethod]
		public void GetCombinations_MinimumAndMaximumSizesSpecified_ReturnsAllCombinationsBetweenThoseSizes()
		{
			// Arrange
			var arrangement = new Arrangement<char>("WXYZ");
			var expectedCombinations = new string[]
			{
				"WX",
				"WXY",
				"WXZ",
				"WY",
				"WYZ",
				"WZ",
				"XY",
				"XYZ",
				"XZ",
				"YZ"
			};

			// Act
			var combinations = arrangement.GetCombinations(2, 3).ToList();

			// Assert
			Assert.AreEqual(expectedCombinations.Length, combinations.Count);
			for (int i = 0; i < combinations.Count && i < expectedCombinations.Length; ++i)
			{
				CollectionAssert.AreEqual(expectedCombinations[i].ToCharArray(), combinations[i]);
			}
		}

		[TestMethod]
		public void GetCombinations_NumberOfCombinationsIsVeryLarge_ReturnsDelayedExecutionEnumerable()
		{
			// Arrange
			string collection = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
			var arrangement = new Arrangement<char>(collection);

			// Act
			var combinations = arrangement.GetCombinations(27); // there are ~10^15 of these

			// Assert
			// If we got here, GetCombinations didn't try to generate all the permutations at once.
			// Just verify that the first few are right.
			var enumerator = combinations.GetEnumerator();
			enumerator.MoveNext();
			CollectionAssert.AreEqual(collection.Substring(0, 27).ToCharArray(), enumerator.Current);

			string secondCombination = "Lorem ipsum dolor sit amet ";
			enumerator.MoveNext();
			CollectionAssert.AreEqual(secondCombination.ToCharArray(), enumerator.Current);
		}
		#endregion

		#region GetPermutations
		[TestMethod]
		public void GetPermutations_MaximumSizeIsLargerThanCollection_ThrowsArgumentException()
		{
			// Arrange
			var arrangement = new Arrangement<char>("CATS");

			// Act
			Action action = () =>
			{
				var permutations = arrangement.GetPermutations(5);
			};

			// Assert
			Assert.ThrowsException<ArgumentException>(action);
		}

		[TestMethod]
		public void GetPermutations_MinimumSizeIsLargerThanMaximumSize_ThrowsArgumentException()
		{
			// Arrange
			var arrangement = new Arrangement<char>("foo");

			// Act
			Action action = () =>
			{
				var permutations = arrangement.GetPermutations(1, 0);
			};

			// Assert
			Assert.ThrowsException<ArgumentException>(action);
		}

		[TestMethod]
		public void GetPermutations_CollectionIsEmpty_ReturnsEmptyEnumerable()
		{
			// Arrange
			var arrangement = new Arrangement<char>("");

			// Act
			var combinations = arrangement.GetPermutations(0);

			// Assert
			Assert.AreEqual(0, combinations.Count());
		}

		[TestMethod]
		public void GetPermutations_CollectionIsNonemptyButGivenSizeIsZero_ReturnsEmptyEnumerable()
		{
			// Arrange
			var arrangement = new Arrangement<char>("any");

			// Act
			var permutations = arrangement.GetPermutations(0);

			// Assert
			Assert.AreEqual(0, permutations.Count());
		}

		[TestMethod]
		public void GetPermutations_SizeIsOne_ReturnsElementsFromCollectionOneAtATime()
		{
			// Arrange
			int[] collection = new int[] { 1, 1, 2, 3, 5 };
			var arrangement = new Arrangement<int>(collection);

			// Act
			var permutations = arrangement.GetPermutations(1).ToList();

			// Assert
			Assert.AreEqual(collection.Length, permutations.Count);
			for (int i = 0; i < permutations.Count && i < collection.Length; ++i)
			{
				Assert.AreEqual(1, permutations[i].Count);
				Assert.AreEqual(collection[i], permutations[i][0]);
			}
		}

		[TestMethod]
		public void GetPermutations_NoArgumentGiven_ReturnsAllPermutationsOfEntireCollection()
		{
			// Arrange
			var arrangement = new Arrangement<char>("cat");
			var expectedPermutations = new string[]
			{
				"cat",
				"cta",
				"act",
				"atc",
				"tca",
				"tac"
			};

			// Act
			var permutations = arrangement.GetPermutations().ToList();

			// Assert
			Assert.AreEqual(expectedPermutations.Length, permutations.Count);
			for (int i = 0; i < permutations.Count && i < expectedPermutations.Length; ++i)
			{
				CollectionAssert.AreEqual(expectedPermutations[i].ToCharArray(), permutations[i]);
			}
		}

		[TestMethod]
		public void GetPermutations_SizeIsAllOfCollection_ReturnsAllPermutationsOfEntireCollection()
		{
			// Arrange
			var arrangement = new Arrangement<char>("dog");
			var expectedPermutations = new string[]
			{
				"dog",
				"dgo",
				"odg",
				"ogd",
				"gdo",
				"god"
			};

			// Act
			var permutations = arrangement.GetPermutations(3).ToList();

			// Assert
			Assert.AreEqual(expectedPermutations.Length, permutations.Count);
			for (int i = 0; i < permutations.Count && i < expectedPermutations.Length; ++i)
			{
				CollectionAssert.AreEqual(expectedPermutations[i].ToCharArray(), permutations[i]);
			}
		}

		[TestMethod]
		public void GetPermutations_OneSizeSpecified_ReturnsAllPermutationsOfThatSize()
		{
			// Arrange
			var arrangement = new Arrangement<char>("start");
			var expectedPermutations = new string[]
			{
				"st", "sa", "sr", "st",
				"ts", "ta", "tr", "tt",
				"as", "at", "ar", "at",
				"rs", "rt", "ra", "rt",
				"ts", "tt", "ta", "tr"
			};

			// Act
			var permutations = arrangement.GetPermutations(2).ToList();

			// Assert
			Assert.AreEqual(expectedPermutations.Length, permutations.Count);
			for (int i = 0; i < permutations.Count && i < expectedPermutations.Length; ++i)
			{
				CollectionAssert.AreEqual(expectedPermutations[i].ToCharArray(), permutations[i]);
			}
		}

		[TestMethod]
		public void GetPermutations_MinimumAndMaximumSizesSpecified_ReturnsAllPermutationsBetweenThoseSizes()
		{
			// Arrange
			var arrangement = new Arrangement<char>("ABCD");
			var expectedPermutations = new string[]
			{
				"A", "AB", "AC", "AD",
				"B", "BA", "BC", "BD",
				"C", "CA", "CB", "CD",
				"D", "DA", "DB", "DC"
			};

			// Act
			var permutations = arrangement.GetPermutations(1, 2).ToList();

			// Assert
			Assert.AreEqual(expectedPermutations.Length, permutations.Count);
			for (int i = 0; i < permutations.Count && i < expectedPermutations.Length; ++i)
			{
				CollectionAssert.AreEqual(expectedPermutations[i].ToCharArray(), permutations[i]);
			}
		}

		[TestMethod]
		public void GetPermutations_NumberOfPermutationsIsVeryLarge_ReturnsDelayedExecutionEnumerable()
		{
			// Arrange
			string collection = "This is a pretty long collection"; // there are ~10^35 permutations of this
			var arrangement = new Arrangement<char>(collection);

			// Act
			var permutations = arrangement.GetPermutations();

			// Assert
			// If we got here, GetPermutations didn't try to generate all the permutations at once.
			// Just verify that the first few are right.
			var enumerator = permutations.GetEnumerator();
			enumerator.MoveNext();
			CollectionAssert.AreEqual(collection.ToCharArray(), enumerator.Current);

			string secondPermutation = "This is a pretty long collectino";
			enumerator.MoveNext();
			CollectionAssert.AreEqual(secondPermutation.ToCharArray(), enumerator.Current);

			string thirdPermutation = "This is a pretty long collectoin";
			enumerator.MoveNext();
			CollectionAssert.AreEqual(thirdPermutation.ToCharArray(), enumerator.Current);
		}
		#endregion

		#region Product
		[TestMethod]
		public void Product_WithNullArrangement_ThrowsNullArgumentException()
		{
			// Arrange
			var arrangement = new Arrangement<char>("foo");

			// Act
			Action action = () =>
			{
				arrangement.Product<int>(null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void Product_WithEmptyArrangement_ReturnsEmptyEnumerable()
		{
			// Arrange
			var arrangement = new Arrangement<char>("bar");
			var empty = new Arrangement<char>("");

			// Act
			var product = arrangement.Product(empty);

			// Assert
			Assert.AreEqual(0, product.Count());
		}

		[TestMethod]
		public void Product_WithNonemptyArrangement_ReturnsAllPairsFromTheTwoSets()
		{
			// Arrange
			var arrangement = new Arrangement<int>(new int[] { 0, 2 });
			var other = new Arrangement<char>("abc");

			// Act
			var product = arrangement.Product(other);

			// Assert
			var expectedProduct = new Tuple<int, char>[]
			{
				Tuple.Create(0, 'a'),
				Tuple.Create(0, 'b'),
				Tuple.Create(0, 'c'),
				Tuple.Create(2, 'a'),
				Tuple.Create(2, 'b'),
				Tuple.Create(2, 'c')
			};
			CollectionAssert.AreEqual(expectedProduct, product.ToList());
		}
		#endregion
	}
}
