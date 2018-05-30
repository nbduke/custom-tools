using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.DataStructures;

namespace Test {

	// TODO need component tests too! Use CountingNodeVisitor to validate tree structure:
	//		- Shared prefixes
	//		- Pruning on remove

	[TestClass]
	public class PrefixTreeDictionaryUnitTests
	{
		#region Count
		[TestMethod]
		public void Count_NoWordsInTheDictionary_ReturnsZero()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			int count = dictionary.Count;

			// Assert
			Assert.AreEqual(0, count);
		}

		[TestMethod]
		public void Count_NonzeroWordsInTheDictionary_ReturnsCorrectNumberOfWords()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			int expectedCount = 12;
			for (int i = 0; i < expectedCount; ++i)
			{
				dictionary.Add(i.ToString());
			}

			// Act
			int count = dictionary.Count;

			// Assert
			Assert.AreEqual(expectedCount, count);
		}
		#endregion

		#region Add
		[TestMethod]
		public void Add_StringIsNull_ThrowsArgumentException()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			Action action = () =>
			{
				dictionary.Add(null);
			};

			Assert.ThrowsException<ArgumentException>(action);
		}

		[TestMethod]
		public void Add_StringIsEmpty_ThrowsArgumentException()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			Action action = () =>
			{
				dictionary.Add(string.Empty);
			};

			// Assert
			Assert.ThrowsException<ArgumentException>(action);
		}

		[TestMethod]
		public void Add_StringIsNotInDictionary_CountIncreasesByOne()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			dictionary.Add("any string");

			// Assert
			Assert.AreEqual(1, dictionary.Count);
		}

		[TestMethod]
		public void Add_StringIsInDictionaryButIsNotEndOfWord_CountIncreasesByOne()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "fixes";
			string prefix = anyString.Substring(0, 3);
			dictionary.Add(anyString);

			// Act
			dictionary.Add(prefix);

			// Assert
			Assert.AreEqual(2, dictionary.Count);
		}

		[TestMethod]
		public void Add_StringIsInDictionaryAndIsEndOfWord_CountDoesNotChange()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "foo";
			dictionary.Add(anyString);

			// Act
			dictionary.Add(anyString);

			// Assert
			Assert.AreEqual(1, dictionary.Count);
		}
		#endregion

		#region Remove
		[TestMethod]
		public void Remove_StringIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			Action action = () =>
			{
				dictionary.Remove(null);
			};

			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void Remove_StringIsEmpty_ReturnsFalse()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			bool result = dictionary.Remove(string.Empty);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Remove_StringIsNotInDictionary_ReturnsFalse()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			bool result = dictionary.Remove("any string");

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Remove_StringIsNotInDictionary_CountDoesNotChange()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			dictionary.Add("thing");

			// Act
			dictionary.Remove("other thing");

			// Assert
			Assert.AreEqual(1, dictionary.Count);
		}

		[TestMethod]
		public void Remove_StringIsInDictionaryButIsNotEndOfWord_ReturnsFalse()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "whether";
			string prefix = anyString.Substring(0, 4);
			dictionary.Add(anyString);

			// Act
			bool result = dictionary.Remove(prefix);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Remove_StringIsInDictionaryButIsNotEndOfWord_CountDoesNotChange()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "errors";
			string prefix = anyString.Substring(0, 5);
			dictionary.Add(anyString);

			// Act
			dictionary.Remove(prefix);

			// Assert
			Assert.AreEqual(1, dictionary.Count);
		}

		[TestMethod]
		public void Remove_StringIsInDictionaryAndIsEndOfWord_ReturnsTrue()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "hello";
			dictionary.Add(anyString);

			// Act
			bool result = dictionary.Remove(anyString);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Remove_StringIsInDictionaryAndIsEndOfWord_CountDecreasesByOne()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "asdf";
			dictionary.Add(anyString);

			// Act
			dictionary.Remove(anyString);

			// Assert
			Assert.AreEqual(0, dictionary.Count);
		}
		#endregion

		#region Contains
		[TestMethod]
		public void Contains_StringIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			Action action = () =>
			{
				dictionary.Contains(null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void Contains_StringIsEmpty_ReturnsFalse()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			bool result = dictionary.Contains(string.Empty);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Contains_StringIsNotInDictionary_ReturnsFalse()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			bool result = dictionary.Contains("something");

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Contains_StringIsInDictionaryAndIsEndOfWord_ReturnsTrue()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "lorem ipsum";
			dictionary.Add(anyString);

			// Act
			bool result = dictionary.Contains(anyString);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Contains_StringIsInDictionaryButIsNotEndOfWord_ReturnsFalse()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "racecar";
			string prefix = anyString.Substring(0, 3);
			dictionary.Add(anyString);

			// Act
			bool result = dictionary.Contains(prefix);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Contains_StringWasRemovedFromDictionary_ReturnsFalse()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "asdf";
			dictionary.Add(anyString);
			dictionary.Remove(anyString);

			// Act
			bool result = dictionary.Contains(anyString);

			// Assert
			Assert.IsFalse(result);
		}
		#endregion

		#region ContainsPrefix
		[TestMethod]
		public void ContainsPrefix_PrefixIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			Action action = () =>
			{
				dictionary.ContainsPrefix(null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void ContainsPrefix_PrefixIsEmpty_ReturnsTrue()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			bool result = dictionary.ContainsPrefix(string.Empty);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void ContainsPrefix_PrefixIsNotInDictionary_ReturnsFalse()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			bool result = dictionary.ContainsPrefix("prefix");

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void ContainsPrefix_PrefixIsAWordInTheDictionary_ReturnsTrue()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "matrix";
			dictionary.Add(anyString);

			// Act
			bool result = dictionary.ContainsPrefix(anyString);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void ContainsPrefix_PrefixIsInDictionaryButIsNotAWord_ReturnsTrue()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "4815162342";
			string prefix = anyString.Substring(0, 4);
			dictionary.Add(anyString);

			// Act
			bool result = dictionary.ContainsPrefix(prefix);

			// Assert
			Assert.IsTrue(result);
		}
		#endregion

		#region Clear
		[TestMethod]
		public void Clear_DictionaryHasWords_CountBecomesZero()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			dictionary.Add("anything");

			// Act
			dictionary.Clear();

			// Assert
			Assert.AreEqual(0, dictionary.Count);
		}

		[TestMethod]
		public void Clear_DictionaryHasWords_ContainsReturnsFalseForAnyWord()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "fizzbuzz";
			dictionary.Add(anyString);

			// Act
			dictionary.Clear();

			// Assert
			Assert.IsFalse(dictionary.Contains(anyString));
		}
		#endregion

		#region FindNode
		[TestMethod]
		public void FindNode_PrefixIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			Action action = () =>
			{
				dictionary.FindNode(null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void FindNode_PrefixIsEmpty_ReturnsRootNode()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			var node = dictionary.FindNode(string.Empty);

			// Assert
			Assert.IsTrue(node.IsRoot);
		}

		[TestMethod]
		public void FindNode_PrefixIsNotInDictionary_ReturnsNull()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			var node = dictionary.FindNode("beef");

			// Assert
			Assert.IsNull(node);
		}

		[TestMethod]
		public void FindNode_PrefixIsAWordInTheDictionary_ReturnsNodeAtEndOfWord()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "food";
			dictionary.Add(anyString);

			// Act
			var node = dictionary.FindNode(anyString);

			// Assert
			Assert.IsTrue(node.IsEndOfWord);

			string stringFromPath = ReconstructStringFromPath(node);
			Assert.AreEqual(anyString, stringFromPath);
		}

		[TestMethod]
		public void FindNode_PrefixIsInDictionaryButIsNotAWord_ReturnsNodeAtEndOfPrefix()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "leaf";
			string prefix = anyString.Substring(0, 1);
			dictionary.Add(anyString);

			// Act
			var node = dictionary.FindNode(prefix);

			// Assert
			Assert.IsFalse(node.IsEndOfWord);

			string stringFromPath = ReconstructStringFromPath(node);
			Assert.AreEqual(prefix, stringFromPath);
		}
		#endregion

		#region Enumeration
		[TestMethod]
		public void Enumeration_DictionaryIsEmpty_ReturnsEmptyEnumerable()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// ACt
			int count = 0;
			foreach(string word in dictionary)
			{
				++count;
			}

			// Assert
			Assert.AreEqual(0, count);
		}

		[TestMethod]
		public void Enumeration_DictionaryContainsTwoWordsWithDifferentPrefixes_EnumeratesBothWordsInAlphabeticalOrder()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string[] expectedWords = new string[]
			{
				"mouse",
				"cat"
			};

			foreach (string word in expectedWords)
			{
				dictionary.Add(word);
			}

			// Act
			var actualWords = new List<string>(dictionary);

			// Assert
			Array.Sort(expectedWords);
			CollectionAssert.AreEqual(expectedWords, actualWords);
		}

		[TestMethod]
		public void Enumeration_DictionaryContainsTwoWordsWithSamePrefix_EnumeratesShorterWordThenLongerWord()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string[] expectedWords = new string[]
			{
				"racecar",
				"race"
			};

			foreach (string word in expectedWords)
			{
				dictionary.Add(word);
			}

			// Act
			var actualWords = new List<string>(dictionary);

			// Assert
			Array.Sort(expectedWords);
			CollectionAssert.AreEqual(expectedWords, actualWords);
		}
		#endregion

		#region GetWordsWithPrefix
		[TestMethod]
		public void GetWordsWithPrefix_PrefixIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			Action action = () =>
			{
				dictionary.GetWordsWithPrefix(null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void GetWordsWithPrefix_PrefixIsEmpty_ReturnsEmptyEnumerable()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			var words = dictionary.GetWordsWithPrefix(string.Empty);

			// Assert
			Assert.AreEqual(0, words.Count());
		}

		[TestMethod]
		public void GetWordsWithPrefix_DictionaryIsEmpty_ReturnsEmptyEnumerable()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			var words = dictionary.GetWordsWithPrefix("any prefix");

			// Assert
			Assert.AreEqual(0, words.Count());
		}

		[TestMethod]
		public void GetWordsWithPrefix_PrefixIsNotInDictionary_ReturnsEmptyEnumerable()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			dictionary.Add("something");

			// Act
			var words = dictionary.GetWordsWithPrefix("other");

			// Assert
			Assert.AreEqual(0, words.Count());
		}

		[TestMethod]
		public void GetWordsWithPrefix_PrefixIsLongerThanAnyWordInTheDictionary_ReturnsEmptyEnumerable()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			dictionary.Add("long");

			// Act
			var words = dictionary.GetWordsWithPrefix("longer");

			// Assert
			Assert.AreEqual(0, words.Count());
		}

		[TestMethod]
		public void GetWordsWithPrefix_PrefixIsAWordInTheDictionaryWithNoChildren_ReturnsPrefix()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "word";
			dictionary.Add(anyString);

			// Act
			var words = dictionary.GetWordsWithPrefix(anyString);

			// Assert
			var expectedWords = new string[] { anyString };
			CollectionAssert.AreEqual(expectedWords, words.ToList());
		}

		[TestMethod]
		public void GetWordsWithPrefix_PrefixIsAWordInTheDictionaryWithChildren_ReturnsPrefixAndAllChildWords()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string prefix = "pro";
			string wordWithPrefix = "probably";
			dictionary.Add(prefix);
			dictionary.Add(wordWithPrefix);

			// Act
			var words = dictionary.GetWordsWithPrefix(prefix);

			// Assert
			var expectedWords = new string[] { prefix, wordWithPrefix };
			CollectionAssert.AreEqual(expectedWords, words.ToList());
		}

		[TestMethod]
		public void GetWordsWithPrefix_PrefixIsNotAWordButHasChildren_ReturnsAllChildWords()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			string anyString = "music";
			string prefix = anyString.Substring(0, 2);
			dictionary.Add(anyString);

			// Act
			var words = dictionary.GetWordsWithPrefix(prefix);

			// Assert
			var expectedWords = new string[] { anyString };
			CollectionAssert.AreEqual(expectedWords, words.ToList());
		}
		#endregion

		#region VisitTree
		[TestMethod]
		public void VisitTree_VisitorIsNull_ThrowsArgumentNullException()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();

			// Act
			Action action = () =>
			{
				dictionary.VisitTree(null);
			};

			// Assert
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void VisitTree_AnyVisitor_VisitsTheRoot()
		{
			// Arrange
			var dictionary = new PrefixTreeDictionary();
			var visitor = new FakeVisitor<IPrefixTreeNode>();

			// Act
			dictionary.VisitTree(visitor);

			// Assert
			Assert.IsTrue(visitor.LastVisited.IsRoot);
		}
		#endregion


		private static string ReconstructStringFromPath(IPrefixTreeNode node)
		{
			var nodeList = new List<IPrefixTreeNode>();
			while (!node.IsRoot)
			{
				nodeList.Add(node);
				node = node.Parent;
			}
			nodeList.Reverse();

			StringBuilder stringBuilder = new StringBuilder();
			foreach (var item in nodeList)
			{
				stringBuilder.Append(item.Character);
			}

			return stringBuilder.ToString();
		}
	}

}
