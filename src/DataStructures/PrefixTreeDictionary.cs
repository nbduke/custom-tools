using System;
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
	public class PrefixTreeDictionary : IDictionary<string>
	{
		public int Count { get; private set; }
		public int UniqueCount { get; private set; }

		private PrefixTreeNode Root { get; set; }

		public PrefixTreeDictionary()
		{
			Clear();
		}

		public void Clear()
		{
			Root = new PrefixTreeNode('\0', null); // arbitrary character
			Count = 0;
			UniqueCount = 0;
		}

		/*
		 * Inserts a word into the dictionary. Duplicate entries are accepted and
		 * will increase Count but not UniqueCount.
		 */
		public bool Insert(string entry)
		{
			Validate.IsNotNullOrEmpty(entry);

			// Create a path in the tree corresponding to the characters in entry
			PrefixTreeNode currentNode = Root;
			foreach (char c in entry)
			{
				currentNode.AddChild(c);
				currentNode = currentNode.GetChild(c);
			}

			if (!currentNode.EndOfWord)
				UniqueCount++;

			Count++;
			currentNode.WordCount++;

			return true;
		}

		public int Delete(string entry)
		{
			Validate.IsNotNullOrEmpty(entry);

			// Get the node that terminates the string, if in the dictionary
			PrefixTreeNode matchingNode = Lookup(entry);
			if (matchingNode == null)
				return 0; // word is not in dictionary

			int wordCount = matchingNode.WordCount;
			matchingNode.WordCount = 0;
			Count -= wordCount;
			UniqueCount--;

			DeleteHelper(matchingNode);

			return wordCount;
		}

		/*
		 * Traverses the path from node to Root, deleting all leaf nodes that are not the
		 * end of a word.
		 */
		private void DeleteHelper(PrefixTreeNode node)
		{
			PrefixTreeNode currentNode = node;
			while (currentNode.Parent != null && !currentNode.HasChildren && !currentNode.EndOfWord)
			{
				PrefixTreeNode parent = currentNode.Parent;
				parent.RemoveChild(currentNode.Character);
				currentNode = parent;
			}
		}

		public bool Contains(string entry)
		{
			return Lookup(entry) != null;
		}

		/*
		 * Returns the tree node that terminates the given word or null if the word
		 * is not in the dictionary.
		 */
		public PrefixTreeNode Lookup(string entry)
		{
			Validate.IsNotNullOrEmpty(entry);

			PrefixTreeNode matchingNode = PartialLookup(entry);
			if (matchingNode == null || !matchingNode.EndOfWord)
				return null;
			else
				return matchingNode;
		}

		/*
		 * Performs a prefix lookup starting at the root. Whereas Lookup only returns
		 * a node that terminates an entry in the dictionary, PartialLookup returns a
		 * node as long as the given prefix exists in the dictionary.
		 */
		public PrefixTreeNode PartialLookup(string prefix)
		{
			return PartialLookup(prefix, Root);
		}

		/*
		 * Performs a prefix lookup using startNode as the root.
		 */
		public PrefixTreeNode PartialLookup(string prefix, PrefixTreeNode startNode)
		{
			Validate.IsNotNull(prefix, "prefix");
			Validate.IsNotNull(startNode, "startNode");

			// Traverse the path from the root corresponding to the characters in prefix
			PrefixTreeNode currentNode = startNode;
			foreach (char c in prefix)
			{
				currentNode = currentNode.GetChild(c);

				if (currentNode == null)
					return null; // prefix is not in dictionary
			}

			return currentNode;
		}

		/*
		 * Returns a list of entries in the dictionary that begin with the given prefix.
		 */
		public List<string> GetWordsWithPrefix(string prefix)
		{
			List<string> result = new List<string>();
			PrefixTreeNode matchingNode = PartialLookup(prefix);

			if (matchingNode == null)
				return result;

			CollectWordsVisitor visitor = new CollectWordsVisitor(result, prefix);
			VisitNodesInSubtree(visitor, matchingNode);

			return result;
		}

		/*
		 * Applies the given visitor to all nodes in the tree.
		 */
		public void VisitAllNodes(IVisitor<PrefixTreeNode> visitor)
		{
			VisitNodesInSubtree(visitor, Root);
		}

		/*
		 * Applies the given visitor to all nodes in the subtree rooted at the given node.
		 */
		public void VisitNodesInSubtree(IVisitor<PrefixTreeNode> visitor, PrefixTreeNode subtreeRoot)
		{
			foreach (PrefixTreeNode startNode in subtreeRoot.GetChildren())
			{
				visitor.Visit(startNode);
			}
		}
	}

}
