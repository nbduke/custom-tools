using System;
using System.Collections;
using System.Collections.Generic;

namespace Tools.Algorithms {

	/*
	 * An IndexEnumerator is an IEnumerator over indices of elements in
	 * an Arrangement.
	 */
	class IndexEnumerator : IEnumerator<SourceIndex>
	{
		private readonly int MaxIndex;
		private readonly SourceIndex Parent;
		private int CurrentIndex;

		public SourceIndex Current { get; private set; }
		object IEnumerator.Current
		{
			get { return Current; }
		}

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

}
