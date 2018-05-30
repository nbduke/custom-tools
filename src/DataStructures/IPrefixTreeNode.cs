using System.Collections.Generic;

namespace Tools.DataStructures {

	/// <summary>
	/// A single node in a PrefixTreeDictionary.
	/// </summary>
	public interface IPrefixTreeNode
	{
		char Character { get; }
		bool IsEndOfWord { get; }
		bool IsRoot { get; }
		bool IsLeaf { get; }
		IPrefixTreeNode Parent { get; }
		IEnumerable<IPrefixTreeNode> Children { get; }

		/// <summary>
		/// Returns the child node corresponding to a character, if it exists.
		/// </summary>
		/// <param name="c">the character</param>
		IPrefixTreeNode GetChild(char c);

		/// <summary>
		/// Returns the descendant node corresponding to the last character
		/// in a string, if it exists.
		/// </summary>
		/// <param name="s">the string</param>
		IPrefixTreeNode GetDescendant(string s);

		/// <summary>
		/// Accepts a visitor that visits this type of node.
		/// </summary>
		/// <param name="visitor">the visitor</param>
		void Accept(IVisitor<IPrefixTreeNode> visitor);
	}

}
