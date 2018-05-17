using System;
using System.IO;
using System.Text;

namespace Tools.DataStructures {

	/// <summary>
	/// Prints words in a PrefixTreeDictionary to an output stream
	/// </summary>
	public class PrintWordsVisitor : IVisitor<IPrefixTreeNode>
	{
		private readonly TextWriter OutputStream;
		private readonly StringBuilder CurrentString;

		/// <summary>
		/// Constructs a PrintWordsVisitor that writes to the console.
		/// </summary>
		public PrintWordsVisitor()
			: this(Console.Out)
		{
		}

		/// <summary>
		/// Constructs a PrintWordsVisitor that writes to an output stream.
		/// </summary>
		/// <param name="outputStream">the output stream</param>
		public PrintWordsVisitor(TextWriter outputStream)
		{
			Validate.IsNotNull(outputStream, "outputStream");
			OutputStream = outputStream;
			CurrentString = new StringBuilder();
		}

		/// <summary>
		/// Prints all words in the subtree rooted at a node
		/// </summary>
		/// <param name="item">the node</param>
		public void Visit(IPrefixTreeNode item)
		{
			CurrentString.Append(item.Character);

			if (item.IsEndOfWord)
				OutputStream.WriteLine(CurrentString.ToString());

			foreach (var child in item.Children)
			{
				Visit(child);
			}

			CurrentString.Length--;
		}
	}


	/// <summary>
	/// Counts nodes in a PrefixTreeDictionary
	/// </summary>
	public class CountNodesVisitor : IVisitor<IPrefixTreeNode>
	{
		public int Count { get; private set; }

		public CountNodesVisitor()
		{
			Count = 0;
		}

		/// <summary>
		/// Counts all nodes in the subtree rooted at a node
		/// </summary>
		/// <param name="item">the node</param>
		public void Visit(IPrefixTreeNode item)
		{
			++Count;

			foreach (var child in item.Children)
			{
				Visit(child);
			}
		}
	}

}
