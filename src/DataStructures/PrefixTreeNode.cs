using System.Collections.Generic;

namespace Tools.DataStructures {

	class PrefixTreeNode : IPrefixTreeNode
	{
		public char Character { get; set; }
		public bool IsEndOfWord { get; set; }
		public int ChildrenCount
		{
			get { return _Children.Count; }
		}
		public bool IsLeaf
		{
			get { return ChildrenCount == 0; }
		}
		public IEnumerable<IPrefixTreeNode> Children
		{
			get { return _Children.Values; }
		}

		private readonly SortedList<char, PrefixTreeNode> _Children;

		/*
		 * Constructs a root PrefixTreeNode.
		 */
		public PrefixTreeNode()
			: this('\0')
		{
		}

		public PrefixTreeNode(char character)
		{
			Character = character;
			IsEndOfWord = false;
			_Children = new SortedList<char, PrefixTreeNode>();
		}

		/*
		 * Gets the child node for the given character, if it exists, and
		 * creates one otherwise. In either case, the child node is returned.
		 */
		public PrefixTreeNode GetOrAddChild(char c)
		{
			var child = GetChildImpl(c);
			if (child == null)
			{
				child = new PrefixTreeNode(c);
				_Children.Add(c, child);
			}

			return child;
		}

		public void RemoveChild(char c)
		{
			_Children.Remove(c);
		}

		public IPrefixTreeNode GetChild(char c)
		{
			return GetChildImpl(c);
		}

		public PrefixTreeNode GetChildImpl(char c)
		{
			if (_Children.TryGetValue(c, out PrefixTreeNode child))
				return child;
			else
				return null;
		}

		public IPrefixTreeNode GetDescendant(string s)
		{
			return GetDescendantImpl(s);
		}

		public PrefixTreeNode GetDescendantImpl(string s)
		{
			Validate.IsNotNull(s, "s");

			var currentNode = this;
			foreach (char c in s)
			{
				currentNode = currentNode.GetChildImpl(c);
				if (currentNode == null)
					return null;
			}

			return currentNode;
		}

		public void Accept(IVisitor<IPrefixTreeNode> visitor)
		{
			Validate.IsNotNull(visitor, "visitor");
			visitor.Visit(this);
		}

		public override bool Equals(object obj)
		{
			return obj is PrefixTreeNode other && Character == other.Character;
		}

		public override int GetHashCode()
		{
			return Character.GetHashCode();
		}

		public override string ToString()
		{
			return Character.ToString();
		}
	}

}
