using System;
using System.Collections;
using System.Collections.Generic;

namespace Tools.DataStructures {

	/// <summary>
	/// PrefixTreeDictionary is a collection of unique strings that groups
	/// elements by common prefixes and supports prefix-based queries.
	/// </summary>
	/// <remarks>
	/// The dictionary is represented as a tree where nodes are characters and
	/// edges represent concatenation. For example, the string "cat" would be
	/// represented by the nodes C->A->T.
	/// 
	/// Moreover, the nodes of common prefixes are shared.If "car" is also in
	/// the tree, then "cat" and "car" share the nodes C and A, and A would have
	/// two child nodes: R and T.
	/// </remarks>
	public class PrefixTreeDictionary : ICollection<string>
	{
		public int Count { get; private set; }
		public bool IsReadOnly
		{
			get { return false; }
		}

		private PrefixTreeNode Root;

		public PrefixTreeDictionary()
		{
			Clear();
		}

		/// <summary>
		/// Removes all entries from the dictionary.
		/// </summary>
		public void Clear()
		{
			Root = new PrefixTreeNode();
			Count = 0;
		}

		/// <summary>
		/// Adds a word to the dictionary.
		/// </summary>
		/// <param name="word">the word</param>
		public void Add(string word)
		{
			Validate.IsNotNullOrEmpty(word);

			// Traverse the path from the root to the node corresponding to the last
			// character in the word, adding nodes as needed
			PrefixTreeNode currentNode = Root;
			foreach (char c in word)
			{
				currentNode = currentNode.GetOrAddChild(c);
			}

			if (!currentNode.IsEndOfWord)
			{
				currentNode.IsEndOfWord = true;
				++Count;
			}
		}

		/// <summary>
		/// Removes a word from the dictionary.
		/// </summary>
		/// <param name="word">the word</param>
		/// <returns>true if the word is in the dictionary</returns>
		public bool Remove(string word)
		{
			var endOfWord = Root.GetDescendantImpl(word);
			if (endOfWord != null && endOfWord.IsEndOfWord)
			{
				endOfWord.IsEndOfWord = false;
				--Count;
				Prune(endOfWord);

				return true;
			}
			else
			{
				return false;
			}
		}

		/*
		 * Traverses the path from node to the root, removing all leaf nodes that are
		 * not the end of a word.
		 */
		private void Prune(PrefixTreeNode node)
		{
			while (!node.IsRoot && node.IsLeaf && !node.IsEndOfWord)
			{
				var parent = (PrefixTreeNode)node.Parent;
				parent.RemoveChild(node.Character);
				node = parent;
			}
		}

		/// <summary>
		/// Checks if a word is in the dictionary.
		/// </summary>
		/// <param name="word">the word</param>
		/// <returns>true if the word is in the dictionary</returns>
		public bool Contains(string word)
		{
			var endOfWord = FindNode(word);
			return endOfWord != null && endOfWord.IsEndOfWord;
		}

		/// <summary>
		/// Checks if a prefix is in the dictionary. A prefix need not be a word.
		/// </summary>
		/// <param name="prefix">the prefix</param>
		/// <returns>true if the prefix is in the dictionary</returns>
		public bool ContainsPrefix(string prefix)
		{
			return FindNode(prefix) != null;
		}

		/// <summary>
		/// Finds the node that terminates a prefix, if it exists.
		/// </summary>
		/// <param name="prefix">the prefix</param>
		public IPrefixTreeNode FindNode(string prefix)
		{
			return Root.GetDescendant(prefix);
		}

		public IEnumerator<string> GetEnumerator()
		{
			return new PrefixTreeEnumerator(Root);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void CopyTo(string[] array, int arrayIndex)
		{
			foreach (string word in this)
			{
				array[arrayIndex++] = word;
			}
		}

		/// <summary>
		/// Returns an enumerable of words that begin with a given prefix.
		/// </summary>
		/// <param name="prefix">the prefix</param>
		public IEnumerable<string> GetWordsWithPrefix(string prefix)
		{
			var endOfPrefix = FindNode(prefix);
			if (endOfPrefix != null)
			{
				var enumerator = new PrefixTreeEnumerator(endOfPrefix, prefix);
				return enumerator.Iterate();
			}
			else
			{
				return new string[] { };
			}
		}

		/// <summary>
		/// Applies a visitor object to the root of the tree.
		/// </summary>
		/// <param name="visitor">the visitor</param>
		public void VisitTree(IVisitor<IPrefixTreeNode> visitor)
		{
			Root.Accept(visitor);
		}
	}

}
