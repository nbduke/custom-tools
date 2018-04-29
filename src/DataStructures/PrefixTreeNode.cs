using System.Collections.Generic;

namespace Tools.DataStructures {

	/*
	 * PrefixTreeNode is a single node in a PrefixTreeDictionary. It stores a
	 * single character, a list of child nodes, and a count of entries that
	 * terminate in this node.
	 */
	public class PrefixTreeNode
	{
		public char Character { get; set; }
		public int WordCount { get; set; }
		public bool EndOfWord
		{
			get { return WordCount > 0; }
		}
		public bool HasChildren
		{
			get { return Children.Count > 0; }
		}
		public readonly PrefixTreeNode Parent;

		private readonly SortedList<char, PrefixTreeNode> Children;

		public PrefixTreeNode(char character, PrefixTreeNode parent)
		{
			Character = character;
			WordCount = 0;
			Parent = parent;
			Children = new SortedList<char, PrefixTreeNode>();
		}

		public void AddChild(char c)
		{
			if (!Children.ContainsKey(c))
				Children.Add(c, new PrefixTreeNode(c, this));
		}

		public void RemoveChild(char c)
		{
			Children.Remove(c);
		}

		public PrefixTreeNode GetChild(char c)
		{
			try
			{
				return Children[c];
			}
			catch (KeyNotFoundException)
			{
				return null;
			}
		}

		public IEnumerable<PrefixTreeNode> GetChildren()
		{
			return Children.Values;
		}

		public override bool Equals(object obj)
		{
			PrefixTreeNode other = obj as PrefixTreeNode;
			return this == other;
		}

		public override int GetHashCode()
		{
			return Character.GetHashCode();
		}

		public override string ToString()
		{
			return Character.ToString();
		}

		public static bool operator==(PrefixTreeNode a, PrefixTreeNode b)
		{
			if (object.ReferenceEquals(a, b))
				return true;
			else if (((object)a == null) || ((object)b == null))
				return false;
			else
				return a.Character == b.Character;
		}

		public static bool operator!=(PrefixTreeNode a, PrefixTreeNode b)
		{
			return !(a == b);
		}
	}

}
