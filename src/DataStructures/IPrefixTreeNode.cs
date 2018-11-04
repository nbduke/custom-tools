using System.Collections.Generic;

namespace Tools.DataStructures {

	/// <summary>
	/// A single node in a PrefixTreeDictionary.
	/// </summary>
	public interface IPrefixTreeNode
	{
		char Character { get; }
		bool IsEndOfWord { get; }
		bool IsLeaf { get; }
		IEnumerable<IPrefixTreeNode> Children { get; }

		/// <summary>
		/// Returns the child node corresponding to a character, if it exists.
		/// </summary>
		/// <param name="c">the character</param>
		IPrefixTreeNode GetChild(char c);

		/// <summary>
		/// Returns the descendant node on the path whose characters correspond
		/// to a string, if it exists. The first character of the string is
		/// considered to be a child of this node.
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
