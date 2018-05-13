﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Algorithms.Search {

	/*
	 * Bidirectional search is a breadth-first search starting from two nodes
	 * simultaneously. One path proceeds forward, like all other search
	 * algorithms; the other proceeds backward and must be reversed once a
	 * common node is found in both paths.
	 * 
	 * This algorithm is guaranteed to find the shortest path between the two
	 * nodes, if one exists.
	 */
	public class BidirectionalSearch<T>
	{
		private readonly ChildGenerator<T> GetForwardChildren;
		private readonly ChildGenerator<T> GetReverseChildren;
		private readonly Action<IEnumerable<T>> RepairReversePath;

		/// <summary>
		/// Creates a BidirectionalSearch for an undirected graph.
		/// </summary>
		/// <param name="getChildrenUndirected">the ChildGenerator for both search paths</param>
		/// <param name="repairReversePath">an optional function that repairs links between
		/// states in the reverse path. The function will be given the path in the final order,
		/// starting with the node found in both paths</param>
		public BidirectionalSearch(
			ChildGenerator<T> getChildrenUndirected,
			Action<IEnumerable<T>> repairReversePath = null)
			: this(getChildrenUndirected, getChildrenUndirected, repairReversePath)
		{
		}

		/// <summary>
		/// Creates a BidirectionalSearch for a directed graph.
		/// </summary>
		/// <param name="getForwardChildren">the ChildGenerator for the forward path</param>
		/// <param name="getReverseChildren">the ChildGenerator for the reverse path.
		/// When the path is reversed at the end, states returned by this function will
		/// become parents of the state passed in.</param>
		/// <param name="repairReversePath">an optional function that repairs links between
		/// states in the reverse path. The function will be given the path in the final order,
		/// starting with the node found in both paths</param>
		public BidirectionalSearch(
			ChildGenerator<T> getForwardChildren,
			ChildGenerator<T> getReverseChildren,
			Action<IEnumerable<T>> repairReversePath = null)
		{
			if (getForwardChildren == null)
				throw new ArgumentNullException("getForwardChildren");
			if (getReverseChildren == null)
				throw new ArgumentNullException("getReverseChildren");

			GetForwardChildren = getForwardChildren;
			GetReverseChildren = getReverseChildren;
			RepairReversePath = repairReversePath;
		}

		public IEnumerable<T> FindPath(T start, T end)
		{
			if (start == null)
				throw new ArgumentNullException("start");
			if (end == null)
				throw new ArgumentNullException("end");

			if (start.Equals(end))
				return new T[] { start };

			var forwardFrontier = new Queue<PathNode<T>>();
			var reverseFrontier = new Queue<PathNode<T>>();
			var explored = new HashSet<PathNode<T>>();

			forwardFrontier.Enqueue(new PathNode<T>(start));
			reverseFrontier.Enqueue(new PathNode<T>(end));

			while (forwardFrontier.Count > 0 && reverseFrontier.Count > 0)
			{
				forwardFrontier = GetNextLayer(forwardFrontier, explored, GetForwardChildren);
				var match = FindMatchingNodes(forwardFrontier, reverseFrontier);
				if (match != null)
					return ConstructSolution(match);

				reverseFrontier = GetNextLayer(reverseFrontier, explored, GetReverseChildren);
				match = FindMatchingNodes(forwardFrontier, reverseFrontier);
				if (match != null)
					return ConstructSolution(match);
			}

			// Path not found
			return new T[] { };
		}

		private static Queue<PathNode<T>> GetNextLayer(
			Queue<PathNode<T>> currentLayer,
			HashSet<PathNode<T>> explored,
			ChildGenerator<T> getChildren)
		{
			var nextLayer = new Queue<PathNode<T>>();
			foreach (var currentNode in currentLayer)
			{
				explored.Add(currentNode);

				foreach (var child in getChildren(currentNode.State))
				{
					var childNode = new PathNode<T>(child, currentNode);
					if (!explored.Contains(childNode) &&
						!currentLayer.Contains(childNode) &&
						!nextLayer.Contains(childNode))
					{
						nextLayer.Enqueue(childNode);
					}
				}
			}

			return nextLayer;
		}

		private static Tuple<PathNode<T>, PathNode<T>> FindMatchingNodes(
			Queue<PathNode<T>> forwardFrontier,
			Queue<PathNode<T>> reverseFrontier)
		{
			var forwardSet = new HashSet<PathNode<T>>(forwardFrontier);
			forwardSet.IntersectWith(reverseFrontier);

			if (forwardSet.Count > 0)
			{
				PathNode<T> forwardNode = forwardSet.First();
				foreach (var reverseNode in reverseFrontier)
				{
					if (reverseNode.Equals(forwardNode))
						return Tuple.Create(forwardNode, reverseNode);
				}
			}

			return null; // no common nodes
		}

		private IEnumerable<T> ConstructSolution(Tuple<PathNode<T>, PathNode<T>> matchingNodes)
		{
			var nodeOnForwardPath = matchingNodes.Item1;
			var nodeOnReversePath = matchingNodes.Item2;
			var pathFromStart = nodeOnForwardPath.GetPath();
			var pathToEnd = nodeOnReversePath.GetPathToRoot();

			if (RepairReversePath != null)
				RepairReversePath(pathToEnd);

			return pathFromStart.Concat(pathToEnd.Skip(1)); // skip the common node
		}
	}

}
