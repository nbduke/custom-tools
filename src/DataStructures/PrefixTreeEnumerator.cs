using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Tools.DataStructures
{

	/*
	 * An enumerator over words in a PrefixTreeDictionary. Current points
	 * to a word, and each call to MoveNext advances Current to the next
	 * word in lexicographic order.
	 */
	class PrefixTreeEnumerator : IEnumerator<string>
	{
		private IPrefixTreeNode CurrentNode;
		private readonly StringBuilder WordBuilder;
		private readonly Stack<IEnumerator<IPrefixTreeNode>> NodeEnumeratorStack;
		private bool CheckInitialPrefix;

		public PrefixTreeEnumerator(IPrefixTreeNode subtreeRoot, string prefix = "")
		{
			Validate.IsNotNull(subtreeRoot, "subtreeRoot");

			CurrentNode = subtreeRoot;
			WordBuilder = new StringBuilder(prefix);
			NodeEnumeratorStack = new Stack<IEnumerator<IPrefixTreeNode>>();
			NodeEnumeratorStack.Push(CurrentNode.Children.GetEnumerator());
			CheckInitialPrefix = CurrentNode.IsEndOfWord;
		}

		public string Current
		{
			get { return WordBuilder.ToString(); }
		}

		object IEnumerator.Current
		{
			get { return Current; }
		}

		public bool MoveNext()
		{
			if (CheckInitialPrefix)
			{
				CheckInitialPrefix = false;
				return true;
			}

			while (NodeEnumeratorStack.Count > 0)
			{
				var enumerator = NodeEnumeratorStack.Peek();
				bool increaseDepth = false;

				while (enumerator.MoveNext())
				{
					CurrentNode = enumerator.Current;
					WordBuilder.Append(CurrentNode.Character);
					NodeEnumeratorStack.Push(CurrentNode.Children.GetEnumerator());

					if (CurrentNode.IsEndOfWord)
					{
						// We've found the next word
						return true;
					}
					else
					{
						// We need to move on to the next node and continue searching
						increaseDepth = true;
						break;
					}
				}

				if (!increaseDepth)
				{
					NodeEnumeratorStack.Pop();

					if (WordBuilder.Length > 0)
						WordBuilder.Length -= 1;
				}
			}

			// There are no more nodes to check
			return false;
		}

		public void Reset()
		{
		}

		public void Dispose()
		{
		}
	};

}
