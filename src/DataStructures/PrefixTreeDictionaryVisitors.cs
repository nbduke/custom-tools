using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tools.DataStructures {

	/*
	 * Prints words in a PrefixTreeDictionary.
	 */
	public class PrintWordsVisitor : IVisitor<PrefixTreeNode>
	{
		private readonly TextWriter OutputStream;
		private readonly StringBuilder CurrentString;

		public PrintWordsVisitor()
			: this(Console.Out)
		{
		}

		public PrintWordsVisitor(TextWriter outputStream)
		{
			OutputStream = outputStream;
			CurrentString = new StringBuilder();
		}

		public void Visit(PrefixTreeNode item)
		{
			CurrentString.Append(item.Character);

			if (item.EndOfWord)
				OutputStream.WriteLine(CurrentString.ToString());

			foreach (var child in item.GetChildren())
			{
				Visit(child);
			}

			CurrentString.Length--;
		}
	}


	/*
	 * Generates a list of words in a PrefixTreeDictionary.
	 */
	public class CollectWordsVisitor : IVisitor<PrefixTreeNode>
	{
		public readonly List<string> Words;

		private readonly StringBuilder CurrentString;

		public CollectWordsVisitor()
			: this(new List<string>())
		{
		}

		public CollectWordsVisitor(List<string> collection)
			: this(collection, string.Empty)
		{
		}

		public CollectWordsVisitor(List<string> collection, string wordsWithPrefix)
		{
			Words = collection;
			CurrentString = new StringBuilder(wordsWithPrefix);
		}

		public void Visit(PrefixTreeNode item)
		{
			CurrentString.Append(item.Character);

			if (item.EndOfWord)
				Words.Add(CurrentString.ToString());

			foreach (var child in item.GetChildren())
			{
				Visit(child);
			}

			CurrentString.Length--;
		}
	}


	/*
	 * Counts all nodes in a PrefixTreeDictionary.
	 */
	public class CountNodesVisitor : IVisitor<PrefixTreeNode>
	{
		public int Count { get; private set; }

		public CountNodesVisitor()
		{
			Count = 0;
		}

		public void Visit(PrefixTreeNode item)
		{
			++Count;

			foreach (var child in item.GetChildren())
			{
				Visit(child);
			}
		}
	}

}
