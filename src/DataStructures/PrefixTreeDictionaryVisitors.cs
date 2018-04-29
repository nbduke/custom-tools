/*
 * PrefixTreeDictionaryVisitors.cs
 * 
 * Nathan Duke
 * 1/31/15
 * 
 * Contains visitors for PrefixTreeDictionary.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonTools { namespace DataStructures {

	/// <summary>
	/// Prints words in a PrefixTreeDictionary.
	/// </summary>
	public class PrintWordsVisitor : IVisitor<PrefixTreeNode>
	{
		private TextWriter OutputStream { get; set; }
		private StringBuilder CurrentString { get; set; }

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


	/// <summary>
	/// Generates a list of words in a PrefixTreeDictionary.
	/// </summary>
	public class CollectWordsVisitor : IVisitor<PrefixTreeNode>
	{
		public List<string> Words { get; private set; }

		private StringBuilder CurrentString { get; set; }

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


	/// <summary>
	/// Counts all nodes in a PrefixTreeDictionary.
	/// </summary>
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

}}
