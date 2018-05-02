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
			City start = new City(CityName.Sibiu);
			CityName[] expectedPath = { CityName.Sibiu, CityName.RimnicuVilcea, CityName.Pitesti, CityName.Bucharest };

			var leastWeightPathSearch = new LeastWeightPathSearch<City>(
				city => city.GetNeighboringCitiesWithDistances());

			// Act
			var finalNode = leastWeightPathSearch.FindNode(start,
				node => node.State.Name == CityName.Bucharest); // find Bucharest

			if (finalNode == null)
			{
				Assert.Fail("FindNode must find the corret node");
				return;
			}

			var actualPath = finalNode.GetPath().Select(city => city.Name);

			// Assert
			Assert.IsTrue(expectedPath.SequenceEqual(actualPath), "The expected and actual sequences should match.");
		}

		[TestMethod]
		public void TestBidirectionalSearch()
		{
			// Arrange
			City start = new City(CityName.Sibiu);
			City end = new City(CityName.Bucharest);
			CityName[] expectedPath = { CityName.Sibiu, CityName.Fagaras, CityName.Bucharest };

			var bidirectionalSearch = new BidirectionalSearch<City>(
				city => city.GetNeighboringCities());

			// Act
			var actualPath = bidirectionalSearch.FindPath(start, end).Select(city => city.Name);

			// Assert
			Assert.IsTrue(expectedPath.SequenceEqual(actualPath), "The expected and actual sequences should match.");
		}

#region Helper classes
		private enum CityName
		{
			Sibiu,
			Fagaras,
			RimnicuVilcea,
			Pitesti,
			Bucharest
		}

		private class City
		{
			public CityName Name;

			private static Dictionary<CityName, CityName[]> Neighbors;

			public City(CityName name)
			{
				Name = name;
				EnsureConnections();
			}

			public IEnumerable<City> GetNeighboringCities()
			{
				foreach (var neighbor in Neighbors[Name])
				{
					yield return new City(neighbor);
				}
			}

			public IEnumerable<Tuple<City, double>> GetNeighboringCitiesWithDistances()
			{
				foreach (var neighbor in Neighbors[Name])
				{
					yield return new Tuple<City, double>(
						new City(neighbor), GetDistance(Name, neighbor));
				}
			}

			public override bool Equals(object obj)
			{
				City other = obj as City;
				return other != null && Name == other.Name;
			}

			public override int GetHashCode()
			{
				return Name.GetHashCode();
			}

			private static double GetDistance(CityName fromCity, CityName toCity)
			{
				switch (fromCity)
				{
					case CityName.Sibiu:
						return toCity == CityName.Fagaras ? 99 : 80;
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
				if (Neighbors == null)
				{
					Neighbors = new Dictionary<CityName, CityName[]>();
					Neighbors[CityName.Sibiu] = new CityName[]
					{
						CityName.Fagaras,
						CityName.RimnicuVilcea
					};
					Neighbors[CityName.Fagaras] = new CityName[]
					{
						CityName.Sibiu,
						CityName.Bucharest
					};
					Neighbors[CityName.RimnicuVilcea] = new CityName[]
					{
						CityName.Sibiu,
						CityName.Pitesti
					};
					Neighbors[CityName.Pitesti] = new CityName[]
					{
						CityName.RimnicuVilcea,
						CityName.Bucharest
					};
					Neighbors[CityName.Bucharest] = new CityName[]
					{
						CityName.Fagaras,
						CityName.Pitesti
					};
				}
			}
		}
#endregion
	}
}
