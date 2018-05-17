using System;
using System.Collections;
using System.Collections.Generic;

namespace Tools.DataStructures {

	/*
	 * PrefixTreeDictionary is a collection of strings that groups elements
	 * by common prefixes. This allows for prefix-based queries. The dictionary
	 * is represented as a tree whose nodes are characters and whose edges
	 * represent concatenation.
	 * 
	 * For example, the string "cat" would be represented by the nodes C->A->T.
	 * 
	 * Moreover, the nodes of common prefixes are shared. If "car" is also in
	 * the tree, then "cat" and "car" share the nodes C and A, and A would have
	 * two child nodes: R and T.
	 */
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

		public void Clear()
		{
			Root = new PrefixTreeNode();
			Count = 0;
		}

		public void Add(string entry)
		{
			Validate.IsNotNullOrEmpty(entry);

			// Traverse the path from the root to the node corresponding to the last
			// character in entry, adding nodes as needed
			PrefixTreeNode currentNode = Root;
			foreach (char c in entry)
			{
				currentNode = currentNode.GetOrAddChild(c);
			}

			if (!currentNode.IsEndOfWord)
			{
				currentNode.IsEndOfWord = true;
				++Count;
			}
		}

		public bool Remove(string entry)
		{
			Validate.IsNotNullOrEmpty(entry);

			var endOfEntry = Root.GetDescendantImpl(entry);
			if (endOfEntry != null && endOfEntry.IsEndOfWord)
			{
				endOfEntry.IsEndOfWord = false;
				--Count;
				Prune(endOfEntry);

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

		public bool Contains(string entry)
		{
			var endOfEntry = FindNode(entry);
			return endOfEntry != null && endOfEntry.IsEndOfWord;
		}

		/// <summary>
		/// Finds the node that terminates the given prefix, if it exists
		/// </summary>
		/// <param name="prefix">the prefix string</param>
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

		/// <summary>
		/// Throws NotImplementedException.
		/// </summary>
		public void CopyTo(string[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns an enumerable of words that begin with a given prefix
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
