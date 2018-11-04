using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.DataStructures;

namespace Test {

	[TestClass]
	public class PrefixTreeDictionaryComponentTests
	{
		#region Add - node creation
		[TestMethod]
		public void Add_DictionaryIsEmpty_CreatesOneNodeForEachCharacter()
		{
			var dictionary = new PrefixTreeDictionary();
			string word = "bananas";
			dictionary.Add(word);

			int nodes = CountNodes(dictionary);
			Assert.AreEqual(word.Length, nodes);
		}

		[TestMethod]
		public void Add_WordIsAlreadyInDictionary_NoNodesCreated()
		{
			var dictionary = new PrefixTreeDictionary();
			string word = "beef";
			dictionary.Add(word);
			dictionary.Add(word);

			int nodes = CountNodes(dictionary);
			Assert.AreEqual(word.Length, nodes);
		}

		[TestMethod]
		public void Add_WordHasAPrefixInTheDictionary_CreatesOneNodeForEachCharacterNotInPrefix()
		{
			var dictionary = new PrefixTreeDictionary();
			string prefix = "beef";
			string word = prefix + "cake";
			dictionary.Add(prefix);
			dictionary.Add(word);

			int nodes = CountNodes(dictionary);
			Assert.AreEqual(word.Length, nodes);
		}

		[TestMethod]
		public void Add_WordDoesNotHaveAPrefixInTheDictionary_CreatesOneNodeForEachCharacter()
		{
			var dictionary = new PrefixTreeDictionary();
			string word = "abcd";
			string newWord = "efgh";
			dictionary.Add(word);
			dictionary.Add(newWord);

			int nodes = CountNodes(dictionary);
			Assert.AreEqual(word.Length + newWord.Length, nodes);
		}
		#endregion

		#region Remove - node deletion
		[TestMethod]
		public void Remove_WordIsOnlyWordInDictionary_RemovesAllNodes()
		{
			var dictionary = new PrefixTreeDictionary();
			string word = "fox";
			dictionary.Add(word);

			dictionary.Remove(word);
			int nodes = CountNodes(dictionary);
			Assert.AreEqual(0, nodes);
		}

		[TestMethod]
		public void Remove_WordIsNotInDictionaryButAPrefixOfItIs_DoesNotRemoveAnyNodes()
		{
			var dictionary = new PrefixTreeDictionary();
			string prefix = "London";
			string word = prefix + "bridge";
			dictionary.Add(prefix);

			dictionary.Remove(word);
			int nodes = CountNodes(dictionary);
			Assert.AreEqual(prefix.Length, nodes);
		}

		[TestMethod]
		public void Remove_WordHasChildren_DoesNotRemoveAnyNodes()
		{
			var dictionary = new PrefixTreeDictionary();
			string word = "gold";
			string longerWord = word + "en";
			dictionary.Add(word);
			dictionary.Add(longerWord);

			dictionary.Remove(word);
			int nodes = CountNodes(dictionary);
			Assert.AreEqual(longerWord.Length, nodes);
		}

		[TestMethod]
		public void Remove_WordHasAPrefixInTheDictionary_RemovesNodesNotInPrefix()
		{
			var dictionary = new PrefixTreeDictionary();
			string prefix = "for";
			string word = prefix + "get";
			dictionary.Add(prefix);
			dictionary.Add(word);

			dictionary.Remove(word);
			int nodes = CountNodes(dictionary);
			Assert.AreEqual(prefix.Length, nodes);
		}
		#endregion


		private static int CountNodes(PrefixTreeDictionary dictionary)
		{
			var counter = new CountNodesVisitor();
			dictionary.VisitTree(counter);
			return counter.Count;
		}
	}

}
