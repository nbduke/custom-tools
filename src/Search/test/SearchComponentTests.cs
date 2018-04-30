using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.Algorithms.Search;

namespace Test
{
	[TestClass]
	public class SearchComponentTests
	{
		[TestMethod]
		public void TestLeastWeightPathSearch()
		{
			// Arrange
			SmallMapNode start = new SmallMapNode();
			CityName[] expectedPath = { CityName.Bucharest, CityName.Pitesti, CityName.RimnicuVilcea, CityName.Sibiu };

			var leastWeightPathSearch = new LeastWeightPathSearch<SmallMapNode>(IsGoal, GetChildren);

			// Act
			SmallMapNode goal = leastWeightPathSearch.Search(start);
			if (goal == null)
			{
				Assert.Fail("A solution should have been found.");
				return;
			}

			var actualPath = goal.GetPath().Select((node) => ((SmallMapNode)node).City);

			// Assert
			Assert.IsTrue(expectedPath.SequenceEqual(actualPath), "The expected and actual sequences should match.");
		}

		[TestMethod]
		public void TestBidirectionalSearch()
		{
			// Arrange
			SmallMapNode start = new SmallMapNode();
			SmallMapNode goal = new SmallMapNode();
			goal.City = CityName.Bucharest;

			CityName[] expectedPath = { CityName.Bucharest, CityName.Fagaras, CityName.Sibiu };

			var bidirectionalSearch = new BidirectionalSearch<SmallMapNode>(GetChildren, GetReverseChildren);

			// Act
			SmallMapNode searchGoal = bidirectionalSearch.Search(start, goal);
			if (searchGoal == null)
			{
				Assert.Fail("A solution should have been found.");
				return;
			}

			var actualPath = searchGoal.GetPath().Select((node) => ((SmallMapNode)node).City);

			// Assert
			Assert.IsTrue(expectedPath.SequenceEqual(actualPath), "The expected and actual sequences should match.");
		}


		private static bool IsGoal(SmallMapNode node)
		{
			return node.City == CityName.Bucharest;
		}

		private static IEnumerable<SmallMapNode> GetChildren(SmallMapNode node)
		{
			switch (node.City)
			{
				case CityName.Sibiu:
					yield return new SmallMapNode(CityName.Fagaras, node);
					yield return new SmallMapNode(CityName.RimnicuVilcea, node);
					break;
				case CityName.Fagaras:
					yield return new SmallMapNode(CityName.Bucharest, node);
					break;
				case CityName.RimnicuVilcea:
					yield return new SmallMapNode(CityName.Pitesti, node);
					break;
				case CityName.Pitesti:
					yield return new SmallMapNode(CityName.Bucharest, node);
					break;
				default:
					yield break;
			}
		}

		private static IEnumerable<SmallMapNode> GetReverseChildren(SmallMapNode node)
		{
			switch (node.City)
			{
				case CityName.Fagaras:
					yield return new SmallMapNode(CityName.Sibiu, node);
					break;
				case CityName.RimnicuVilcea:
					yield return new SmallMapNode(CityName.Sibiu, node);
					break;
				case CityName.Pitesti:
					yield return new SmallMapNode(CityName.RimnicuVilcea, node);
					break;
				case CityName.Bucharest:
					yield return new SmallMapNode(CityName.Fagaras, node);
					yield return new SmallMapNode(CityName.Pitesti, node);
					break;
				default:
					yield break;
			}
		}

		private enum CityName
		{
			Sibiu, // start
			Fagaras,
			RimnicuVilcea,
			Pitesti,
			Bucharest // goal
		}

		private class SmallMapNode : PathNodeBase
		{
			public CityName City { get; set; }

			public SmallMapNode()
			{
				City = CityName.Sibiu;
			}

			public SmallMapNode(CityName city, SmallMapNode parent)
				: base(parent, CalculateDistance(city, parent.City))
			{
				City = city;
			}

			public override bool Equals(object obj)
			{
				SmallMapNode other = obj as SmallMapNode;
				return other != null && City == other.City;
			}

			public override int GetHashCode()
			{
				return City.GetHashCode();
			}

			private static double CalculateDistance(CityName city, CityName parentCity)
			{
				switch (parentCity)
				{
					case CityName.Sibiu:
						return city == CityName.Fagaras ? 99 : 80;
					case CityName.Fagaras:
						return 211;
					case CityName.RimnicuVilcea:
						return 97;
					case CityName.Pitesti:
						return 101;
					default:
						return 0;
				}
			}
		}
	}
}
