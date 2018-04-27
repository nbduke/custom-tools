/*
 * PathNodeBase.cs
 * 
 * Nathan Duke
 * 1/31/15
 * 
 * Contains the PathNodeBase class. This is the base class for all
 * state representations that can be used in the Search algorithms
 * in this library. It provides some of the mechanisms required by
 * each algorithm, like tracking cumulative path length and weight.
 */

using System;
using System.Collections.Generic;

namespace CommonTools { namespace Algorithms { namespace Search {

	// PathNodeBase is the base class for nodes in a generic graph search algorithm.
	// It contains a core set of methods and properties for most algorithms.
	public class PathNodeBase
	{
		public PathNodeBase Parent { get; private set; }
		public uint CumulativePathLength { get; private set; }
		public double CumulativePathWeight { get; private set; }

		public PathNodeBase() // constructs a root node
			: this(null)
		{
		}

		public PathNodeBase(PathNodeBase parent)
			: this(parent, 0)
		{
		}

		public PathNodeBase(PathNodeBase parent, double weight)
		{
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

		public virtual bool IsRoot
		{
			get
			{
				return Parent == null;
			}
		}

		public virtual PathNodeBase GetRoot()
		{
			PathNodeBase currentNode = this;
			while (!currentNode.IsRoot)
				currentNode = currentNode.Parent;

			return currentNode;
		}

		public virtual IEnumerable<PathNodeBase> GetPathToRoot()
		{
			PathNodeBase currentNode = this;
			while (currentNode != null)
			{
				yield return currentNode;
				currentNode = currentNode.Parent;
			}
		}

		public virtual bool PathContains(PathNodeBase node)
		{
			foreach (var other in GetPathToRoot())
			{
				if (node.Equals(other))
					return true;
			}

			return false;
		}

		// Reverses the child-parent relationships of all nodes on the path to the root.
		// The edge weights between nodes are preserved. If the original root has nonzero
		// weight, its weight is transferred to the new root after inversion.
		public virtual void InvertPath()
		{
			List<PathNodeBase> path = new List<PathNodeBase>(GetPathToRoot());
			List<double> edgeWeights = new List<double>();

			for (int i = 0; i < path.Count - 1; ++i)
			{
				edgeWeights.Add(path[i].CumulativePathWeight - path[i + 1].CumulativePathWeight);
			}

			Parent = null;
			CumulativePathLength = 1;
			CumulativePathWeight = path[path.Count - 1].CumulativePathWeight; // the root weight

			for (int i = 1; i < path.Count; ++i)
			{
				PathNodeBase node = path[i];
				PathNodeBase parent = path[i - 1];

				node.Parent = parent;
				node.CumulativePathLength = parent.CumulativePathLength + 1;
				node.CumulativePathWeight = parent.CumulativePathWeight + edgeWeights[i - 1];
			}
		}

		// Sets the parent of this node's root to otherNode, adjusting the cumulative path
		// lengths and weights of each node on the path to the root accordingly.
		public virtual void JoinPath(PathNodeBase otherNode)
		{
			if (otherNode == null)
				return;

			foreach (PathNodeBase node in GetPathToRoot())
			{
				node.CumulativePathLength += otherNode.CumulativePathLength;
				node.CumulativePathWeight += otherNode.CumulativePathWeight;

				if (node.IsRoot)
				{
					node.Parent = otherNode;
					break; // break or else GetPathToRoot will continue returning nodes in the joined path
				}
			}
		}
	}

}}}
