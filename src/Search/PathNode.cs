using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/*
	 * PathNode represents a single node in an arbitrary path of a graph.
	 * The path is maintained by links from each node to its parent node (the
	 * preceeding node on the path). The class also keeps track of the
	 * cumulative path length and weight.
	 */
	public abstract class PathNode
	{
		public PathNode Parent { get; private set; }
		public uint CumulativePathLength { get; private set; }
		public double CumulativePathWeight { get; private set; }

		/*
		 * Constructs a root node (the first node in a path).
		 */
		public PathNode()
			: this(null)
		{
		}

		/*
		 * Constructs a node with zero weight on the incoming edge.
		 */
		public PathNode(PathNode parent)
			: this(parent, 0)
		{
		}

		/*
		 * Constructs a node with the given parent and the given weight on
		 * its incoming edge.
		 */
		public PathNode(PathNode parent, double weight)
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

		/*
		 * Returns all nodes that are reachable from this node, all of which
		 * must point to this node as their parent.
		 * 
		 * NOTE: For best performance, it's a good idea to generate children
		 * one at a time rather than all at once, for example by using a yield
		 * statement.
		 */
		public abstract IEnumerable<PathNode> GetChildren();

		public bool IsRoot
		{
			get { return Parent == null; }
		}

		public PathNode GetRoot()
		{
			PathNode currentNode = this;
			while (!currentNode.IsRoot)
				currentNode = currentNode.Parent;

			return currentNode;
		}

		public IEnumerable<PathNode> GetPath()
		{
			PathNode currentNode = this;
			while (currentNode != null)
			{
				yield return currentNode;
				currentNode = currentNode.Parent;
			}
		}

		public virtual bool PathContains(PathNode node)
		{
			foreach (var other in GetPath())
			{
				if (node.Equals(other))
					return true;
			}

			return false;
		}

		/*
		 * Reverses the child-parent relationships of all nodes on the path. The edge
		 * weights between nodes are preserved. If the original root has nonzero weight,
		 * then its weight is transferred to the new root after inversion.
		 */
		public virtual void InvertPath()
		{
			List<PathNode> path = new List<PathNode>(GetPath());
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
				PathNode node = path[i];
				PathNode parent = path[i - 1];

				node.Parent = parent;
				node.CumulativePathLength = parent.CumulativePathLength + 1;
				node.CumulativePathWeight = parent.CumulativePathWeight + edgeWeights[i - 1];
			}
		}

		/*
		 * Sets the parent of this node's root to otherNode, adjusting the cumulative path
		 * lengths and weights of each node on the path accordingly.
		 */
		public virtual void JoinPath(PathNode otherNode)
		{
			if (otherNode == null)
				return;

			foreach (PathNode node in GetPath())
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
