/*
 * SourceIndex.cs
 * 
 * Nathan Duke
 * 8/8/2016
 * 
 * Contains the SourceIndex class. SourceIndex represents the index of
 * an element in an Arrangement. This is used to keep track of items
 * that have been placed into a combination or permutation.
 */

using System;
using System.Collections.Generic;
using System.Linq;

using CommonTools.Algorithms.Search;

namespace CommonTools { namespace Algorithms {

	class SourceIndex : PathNodeBase
	{
		public int Index { get; private set; }
		public bool IsIndexOfPermutation { get; private set; }

		public SourceIndex(int index, bool isPermutation)
			: base(null)
		{
			Index = index;
			IsIndexOfPermutation = isPermutation;
		}

		public SourceIndex(int index, SourceIndex parent)
			: base(parent)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");

			Index = index;
			IsIndexOfPermutation = parent.IsIndexOfPermutation;
		}

		public override bool Equals(object node)
		{
			var nodeAsSourceIndex = node as SourceIndex;
			return nodeAsSourceIndex != null && Index == nodeAsSourceIndex.Index;
		}

		public override int GetHashCode()
		{
			return Index.GetHashCode();
		}

		public override bool PathContains(PathNodeBase node)
		{
			if (IsIndexOfPermutation)
				return base.PathContains(node);
			else
				return false; // due to the order of child expansion, combinations never need to do this check
		}

		// Reconstructs the list of elements in the original collection whose indices are
		// stored in the path which terminates in this SourceIndex.
		public virtual IEnumerable<T> GetOriginals<T>(IList<T> sourceSet)
		{
			foreach (PathNodeBase node in GetPathToRoot().Reverse())
			{
				int indexIntoSource = ((SourceIndex)node).Index;
				yield return sourceSet[indexIntoSource];
			}
		}
	}

}}
