/*
 * IndexEnumerator.cs
 * 
 * Nathan Duke
 * 8/8/2016
 * 
 * Contains the IndexEnumerator class: an IEnumerator over indices
 * of elements in an Arrangement.
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonTools { namespace Algorithms {

	class IndexEnumerator : IEnumerator<SourceIndex>
	{
		private int MaxIndex { get; set; }
		private int CurrentIndex { get; set; }
		private SourceIndex Parent { get; set; }

		public SourceIndex Current { get; private set; }
		object IEnumerator.Current { get { return Current; } }

		public IndexEnumerator(int maxSourceIndex, SourceIndex parent)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");

			MaxIndex = maxSourceIndex;
			Parent = parent;
			Reset();
		}

		public bool MoveNext()
		{
			++CurrentIndex;
			Current = new SourceIndex(CurrentIndex, Parent);

			return CurrentIndex <= MaxIndex;
		}

		public void Reset()
		{
			CurrentIndex = Parent.IsIndexOfPermutation ? -1 : Parent.Index;
		}

		public void Dispose()
		{
		}
	}

}}
