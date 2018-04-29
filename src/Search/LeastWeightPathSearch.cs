/*
 * LeastWeightPathSearch.cs
 * 
 * Nathan Duke
 * 8/8/2016
 * 
 * Contains the LeastWeightPathSearch class.
 * 
 * Least-weight path search (a.k.a. Uniform Cost Search) explores nodes
 * in order by ascending cumulative path weight.
 */

using CommonTools.DataStructures;
using System.Collections.Generic;

namespace CommonTools { namespace Algorithms { namespace Search {

	public class LeastWeightPathSearch<T> where T : PathNodeBase
	{
		private GoalTest<T> IsGoal;
		private ChildGenerator<T> GetChildren;

		public LeastWeightPathSearch(
			GoalTest<T> isGoal,
			ChildGenerator<T> getChildren)
		{
			IsGoal = isGoal;
			GetChildren = getChildren;
		}

		public T Search(T start)
		{
			return Search(start, uint.MaxValue);
		}

		public T Search(T start, uint maxSearchDistance)
		{
			if (start == null || IsGoal(start))
				return start;
			else if (maxSearchDistance == 0)
				return null;

			BinaryHeap<T> frontier = BinaryHeap<T>.CreateMinFirstHeap();
			HashSet<T> explored = new HashSet<T>();
			frontier.Enqueue(start, start.CumulativePathWeight);

			while (frontier.Count > 0)
			{
				T currentNode = frontier.Dequeue().Value;
				if (IsGoal(currentNode))
					return currentNode;
				else if (currentNode.CumulativePathLength >= maxSearchDistance)
					continue;

				explored.Add(currentNode);
				foreach (T child in GetChildren(currentNode))
				{
					if (!explored.Contains(child))
					{
						double priorPathWeight = 0;
						if (!frontier.TryGetPriority(child, ref priorPathWeight) ||
							 priorPathWeight > child.CumulativePathWeight)
						{
							frontier.Enqueue(child, child.CumulativePathWeight);
						}
					}
				}
			}

			// Path not found
			return null;
		}
	}

}}}
