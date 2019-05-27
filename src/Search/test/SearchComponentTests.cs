using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.Algorithms.Search;

namespace Test {

	[TestClass]
	public class SearchComponentTests
	{
		[TestMethod]
		public void TestLeastWeightPathSearch()
		{
			// Arrange
			var cityMap = GetCityMap();
			var leastWeightPathSearch = new LeastWeightPathSearch<City>(
				city => cityMap[city]
			);

			// Act
			var path = leastWeightPathSearch.FindPath(City.Sibiu, City.Bucharest);

			// Assert
			City[] expectedPath =
			{
				City.Sibiu,
				City.RimnicuVilcea,
				City.Pitesti,
				City.Bucharest
			};
			CollectionAssert.AreEqual(expectedPath, path.ToList());
		}

		[TestMethod]
		public void TestBidirectionalSearch()
		{
			// Arrange
			var cityMap = GetCityMap();
			var bidirectionalSearch = new BidirectionalSearch<City>(
				city => cityMap[city].Select(t => t.Item1)
			);

			// Act
			var actualPath = bidirectionalSearch.FindPath(City.Fagaras, City.Pitesti);

			// Assert
			City[] expectedPath =
			{
				City.Fagaras,
				City.Bucharest,
				City.Pitesti
			};
			CollectionAssert.AreEqual(expectedPath, actualPath.ToList());
		}

		#region Helpers
		private enum City
		{
			Sibiu,
			Fagaras,
			RimnicuVilcea,
			Pitesti,
			Bucharest
		}

		private static Dictionary<City, Tuple<City, double>[]> GetCityMap()
		{
			return new Dictionary<City, Tuple<City, double>[]>
			{
				{
					City.Sibiu,
					new Tuple<City, double>[]
					{
						Tuple.Create(City.Fagaras, 99.0),
						Tuple.Create(City.RimnicuVilcea, 80.0)
					}
				},
				{
					City.Fagaras,
					new Tuple<City, double>[]
					{
						Tuple.Create(City.Sibiu, 99.0),
						Tuple.Create(City.Bucharest, 211.0)
					}
				},
				{
					City.RimnicuVilcea,
					new Tuple<City, double>[]
					{
						Tuple.Create(City.Sibiu, 80.0),
						Tuple.Create(City.Pitesti, 97.0)
					}
				},
				{
					City.Pitesti,
					new Tuple<City, double>[]
					{
						Tuple.Create(City.RimnicuVilcea, 97.0),
						Tuple.Create(City.Bucharest, 101.0)
					}
				}
			};
		}
		#endregion
	}
}
