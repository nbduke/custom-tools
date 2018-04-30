using Tools.DataStructures;
using System.Collections.Generic;

namespace Tools.Algorithms.Search {

	/*
	 * Least-weight path search (a.k.a. uniform cost search) explores nodes
	 * in order by ascending cumulative path weight. It can find the optimal
	 * (i.e. least-total-weight) path between two nodes in a graph with no
	 * negative cycles.
	 */
	public class LeastWeightPathSearch<T> where T : PathNode
	{
		private GoalTest<T> IsGoal;

		public LeastWeightPathSearch(GoalTest<T> isGoal)
		{
			IsGoal = isGoal;
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
				foreach (T child in currentNode.GetChildren())
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

}
