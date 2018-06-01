using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Algorithms.Search {

	/// <summary>
	/// PathNode encapsulates a single node in a path of a graph. The path is
	/// maintained by links from each node to its parent node (the preceding
	/// node on the path). PathNode also keeps track of the cumulative path
	/// length and weight.
	/// </summary>
	/// <typeparam name="T">the type of nodes in the graph</typeparam>
	public class PathNode<T>
	{
		public readonly T State;
		public readonly PathNode<T> Parent;
		public readonly uint CumulativePathLength;
		public readonly double CumulativePathWeight;

		/// <summary>
		/// Constructs a root node (the first node in a path).
		/// </summary>
		/// <param name="state">the state associated with the node</param>
		public PathNode(T state)
			: this(state, null)
		{
		}

		/// <summary>
		/// Constructs a node with zero weight on its incoming edge.
		/// </summary>
		/// <param name="state">the state associated with the node</param>
		/// <param name="parent">the node's parent</param>
		public PathNode(T state, PathNode<T> parent)
			: this(state, parent, 0)
		{
		}

		/// <summary>
		/// Constructs a node with a parent and a weight on its incoming edge.
		/// </summary>
		/// <param name="state">the state associated with the node</param>
		/// <param name="parent">the node's parent</param>
		/// <param name="weight">the edge weight</param>
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
