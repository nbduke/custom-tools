using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Algorithms.Search {

	/*
	 * PathNode encapsulates a single node in a path of a graph whose nodes
	 * are represented by the template type T. The path is maintained by
	 * links from each node to its parent node (the preceding node on the
	 * path). PathNode also keeps track of information such as the
	 * cumulative path length and weight.
	 */
	public class PathNode<T>
	{
		public readonly T State;
		public readonly PathNode<T> Parent;
		public readonly uint CumulativePathLength;
		public readonly double CumulativePathWeight;

		/*
		 * Constructs a root node (the first node in a path).
		 */
		public PathNode(T state)
			: this(state, null)
		{
		}

		/*
		 * Constructs a node with zero weight on its incoming edge.
		 */
		public PathNode(T state, PathNode<T> parent)
			: this(state, parent, 0)
		{
		}

		/*
		 * Constructs a node with the given parent and the given weight on
		 * its incoming edge.
		 */
		public PathNode(T state, PathNode<T> parent, double weight)
		{
			Validate.IsNotNull(state, "state");

			State = state;
			Parent = parent;

			if (IsRoot)
			{
				CumulativePathLength = 1;
				CumulativePathWeight = weight;
			}
			else
			{
				CumulativePathLength = Parent.CumulativePathLength + 1;
				CumulativePathWeight = Parent.CumulativePathWeight + weight;
			}
		}

		public bool IsRoot
		{
			get { return Parent == null; }
		}

		public IEnumerable<T> GetPath()
		{
			return GetPathToRoot().Reverse();
		}

		public IEnumerable<T> GetPathToRoot()
		{
			PathNode<T> currentNode = this;
			while (currentNode != null)
			{
				yield return currentNode.State;
				currentNode = currentNode.Parent;
			}
		}

		public bool PathContains(T state)
		{
			foreach (var otherState in GetPath())
			{
				if (otherState.Equals(state))
					return true;
			}

			return false;
		}

		public bool Equals(PathNode<T> node)
		{
			return node != null && State.Equals(node.State);
		}

		public bool Equals(T state)
		{
			return State.Equals(state);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as PathNode<T>);
		}

		public override int GetHashCode()
		{
			return State.GetHashCode();
		}
	}

}
