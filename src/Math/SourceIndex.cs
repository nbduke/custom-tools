using System;
using System.Collections.Generic;
using System.Linq;

using Tools.Algorithms.Search;

namespace Tools.Algorithms {

	/*
	 * SourceIndex represents the index of an element in an Arrangement. It
	 * is used to keep track of items that have been placed into a combination
	 * or permutation.
	 */
	class SourceIndex<T> : PathNode
	{
		public int Index { get; private set; }
		public bool IsIndexOfPermutation { get; private set; }

		private readonly IList<T> Source;

		public SourceIndex(int index, bool isPermutation, IList<T> source)
			: base(null)
		{
			if (source == null)
				throw new ArgumentNullException("collection");

			Index = index;
			IsIndexOfPermutation = isPermutation;
			Source = source;
		}

		public SourceIndex(int index, SourceIndex<T> parent)
			: base(parent)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");

			Index = index;
			IsIndexOfPermutation = parent.IsIndexOfPermutation;
			Source = parent.Source;
		}

		public override IEnumerable<PathNode> GetChildren()
		{
			int startOfChildIndices = IsIndexOfPermutation ? 0 : Index + 1;
			for (int i = startOfChildIndices; i < Source.Count; ++i)
			{
				if (i != Index)
					yield return new SourceIndex<T>(i, this);
			}
		}

		public override bool Equals(object node)
		{
			var nodeAsSourceIndex = node as SourceIndex<T>;
			return nodeAsSourceIndex != null && Index == nodeAsSourceIndex.Index;
		}

		public override int GetHashCode()
		{
			return Index.GetHashCode();
		}

		public override bool PathContains(PathNode node)
		{
			if (IsIndexOfPermutation)
				return base.PathContains(node);
			else
				return false; // due to the order of child expansion, combinations never need to do this check
		}

		/*
		 * Reconstructs the list of elements in the original collection whose indices are
		 * stored in the path which terminates in this SourceIndex.
		 */
		public virtual IEnumerable<T> GetOriginalsFromPath()
		{
			foreach (PathNode node in GetPath().Reverse())
			{
				int indexIntoSource = ((SourceIndex<T>)node).Index;
				yield return Source[indexIntoSource];
			}
		}
	}

}
