using System;
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

			var leastWeightPathSearch = new LeastWeightPathSearch<SmallMapNode>(node => node.City == CityName.Bucharest);

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

			var bidirectionalSearch = new BidirectionalSearch<SmallMapNode>();

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

		private enum CityName
		{
			Sibiu, // start
			Fagaras,
			RimnicuVilcea,
			Pitesti,
			Bucharest // goal
		}

		private class SmallMapNode : PathNode
		{
			public CityName City { get; set; }

			private static Dictionary<CityName, CityName[]> CityConnections;

			public SmallMapNode()
			{
				City = CityName.Sibiu;
			}

			public SmallMapNode(CityName city, SmallMapNode parent)
				: base(parent, CalculateDistance(city, parent.City))
			{
				City = city;
			}

			public override IEnumerable<PathNode> GetChildren()
			{
				EnsureConnections();
				foreach (var connection in CityConnections[City])
				{
					yield return new SmallMapNode(connection, this);
				}
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

			private static void EnsureConnections()
			{
				if (CityConnections == null)
				{
					CityConnections = new Dictionary<CityName, CityName[]>();
					CityConnections[CityName.Sibiu] = new CityName[]
					{
						CityName.Fagaras,
						CityName.RimnicuVilcea
					};
					CityConnections[CityName.Fagaras] = new CityName[]
					{
						CityName.Sibiu,
						CityName.Bucharest
					};
					CityConnections[CityName.RimnicuVilcea] = new CityName[]
					{
						CityName.Sibiu,
						CityName.Pitesti
					};
					CityConnections[CityName.Pitesti] = new CityName[]
					{
						CityName.RimnicuVilcea,
						CityName.Bucharest
					};
					CityConnections[CityName.Bucharest] = new CityName[]
					{
						CityName.Fagaras,
						CityName.Pitesti
					};
				}
			}
		}
	}
}
