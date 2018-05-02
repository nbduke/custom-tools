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
		public PathNode<T> Parent { get; private set; }
		public uint CumulativePathLength { get; private set; }
		public double CumulativePathWeight { get; private set; }

		/*
		 * Constructs a root node (the first node in a path).
		 */
		public PathNode(T state)
			: this(state, null)
		{
		}

		/*
		 * Constructs a node with zero weight on the incoming edge.
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
			if (state == null)
				throw new ArgumentNullException("state");

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

		public PathNode<T> GetRoot()
		{
			PathNode<T> currentNode = this;
			while (!currentNode.IsRoot)
				currentNode = currentNode.Parent;

			return currentNode;
		}

		public IEnumerable<T> GetPath()
		{
			return GetPathToRoot().Select(node => node.State).Reverse();
		}

		private IEnumerable<PathNode<T>> GetPathToRoot()
		{
			PathNode<T> currentNode = this;
			while (currentNode != null)
			{
				yield return currentNode;
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

		public override bool Equals(object obj)
		{
			PathNode<T> other = obj as PathNode<T>;
			return other != null && State.Equals(other.State);
		}

		public override int GetHashCode()
		{
			return State.GetHashCode();
		}

		/*
		 * Reverses the child-parent relationships of all nodes on the path. The edge
		 * weights between nodes are preserved. If the original root has nonzero weight,
		 * then its weight is transferred to the new root after inversion.
		 */
		public void InvertPath()
		{
			var path = new List<PathNode<T>>(GetPathToRoot());
			var edgeWeights = new List<double>();

			for (int i = 0; i < path.Count - 1; ++i)
			{
				edgeWeights.Add(path[i].CumulativePathWeight - path[i + 1].CumulativePathWeight);
			}

			Parent = null;
			CumulativePathLength = 1;
			CumulativePathWeight = path[path.Count - 1].CumulativePathWeight; // the root weight

			for (int i = 1; i < path.Count; ++i)
			{
				PathNode<T> node = path[i];
				PathNode<T> parent = path[i - 1];

				node.Parent = parent;
				node.CumulativePathLength = parent.CumulativePathLength + 1;
				node.CumulativePathWeight = parent.CumulativePathWeight + edgeWeights[i - 1];
			}
		}

		/*
		 * Sets the parent of this node's root to otherNode, adjusting the cumulative path
		 * lengths and weights of each node on the path accordingly.
		 */
		public void JoinPath(PathNode<T> otherNode)
		{
			if (otherNode == null)
				return;

			foreach (PathNode<T> node in GetPathToRoot())
			{
				node.CumulativePathLength += otherNode.CumulativePathLength;
				node.CumulativePathWeight += otherNode.CumulativePathWeight;

				if (node.IsRoot)
				{
					node.Parent = otherNode;
					break; // break or else GetPath will continue returning nodes in the joined path
				}
			}
		}
	}

}
