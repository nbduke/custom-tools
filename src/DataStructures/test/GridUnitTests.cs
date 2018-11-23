using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tools.DataStructures;

namespace Test
{
	[TestClass]
	public class GridUnitTests
	{
		#region Basic constructor
		[TestMethod]
		public void Constructor_WithNegativeRows_ThrowsArgumentException()
		{
			// Act
			Action action = () =>
			{
				var grid = new Grid<int>(-1, 10);
			};

			// Assert
			Assert.ThrowsException<ArgumentException>(action);
		}

		[TestMethod]
		public void Constructor_WithNegativeColumns_ThrowsArgumentException()
		{
			// Act
			Action action = () =>
			{
				var grid = new Grid<int>(10, -1);
			};

			// Assert
			Assert.ThrowsException<ArgumentException>(action);
		}
		#endregion

		#region Fill constructor
		[TestMethod]
		public void Constructor_WithAnyRowsAndColumnsAndFillValue_FillsGridWithValue()
		{
			// Arrange
			string fillValue = "whoa!";

			// Act
			var grid = new Grid<string>(3, 4, fillValue);

			// Assert
			foreach (string item in grid)
			{
				Assert.AreEqual(fillValue, item);
			}
		}
		#endregion

		#region Copy constructor
		[TestMethod]
		public void Consructor_WithAnotherGrid_CreatesCopyOfGrid()
		{
			// Arrange
			var original = new Grid<string>(2, 6, "foo");

			// Act
			var copy = new Grid<string>(original);

			// Assert
			Assert.IsTrue(original.SequenceEqual(copy));
		}
		#endregion

		#region Rows
		[TestMethod]
		public void Rows_AtAnyTime_ReturnsValueGivenToConstructor()
		{
			// Arrange
			int expectedRows = 3;
			var grid = new Grid<int>(expectedRows, 2);

			// Act
			int rows = grid.Rows;

			// Assert
			Assert.AreEqual(expectedRows, rows);
		}
		#endregion

		#region Columns
		[TestMethod]
		public void Columns_AtAnyTime_ReturnsValueGivenToConstructor()
		{
			// Arrange
			int expectedColumns = 4;
			var grid = new Grid<int>(1, expectedColumns);

			// Act
			int columns = grid.Columns;

			// Assert
			Assert.AreEqual(expectedColumns, columns);
		}
		#endregion

		#region Cells
		[TestMethod]
		public void Cells_AtAnyTime_ReturnsProductOfRowsAndColumns()
		{
			// Arrange
			int rows = 5;
			int columns = 6;
			var grid = new Grid<int>(rows, columns);

			// Act
			int cells = grid.Cells;

			// Assert
			Assert.AreEqual(rows * columns, cells);
		}
		#endregion

		#region IsSquare
		[TestMethod]
		public void IsSquare_RowsAndColumnsAreEqual_ReturnsTrue()
		{
			// Arrange
			int rows = 5;
			int columns = rows;
			var grid = new Grid<int>(rows, columns);

			// Act
			bool isSquare = grid.IsSquare;

			// Assert
			Assert.IsTrue(isSquare);
		}

		[TestMethod]
		public void IsSquare_RowsAndColumnsAreNotEqual_ReturnsFalse()
		{
			// Arrange
			int rows = 3;
			int columns = 1;
			var grid = new Grid<int>(rows, columns);

			// Act
			bool isSquare = grid.IsSquare;

			// Assert
			Assert.IsFalse(isSquare);
		}
		#endregion

		#region Getters and setters
		[TestMethod]
		public void GetAndSet_WithGridCell_ReadsAndWritesExpectedLocation()
		{
			// Arrange
			var grid = new Grid<int>(10, 10);
			GridCell location = new GridCell(3, 9);
			int expectedItem = 50;

			// Act
			grid[location] = expectedItem;
			int item = grid[location];

			// Assert
			Assert.AreEqual(expectedItem, item);
		}

		[TestMethod]
		public void GetAndSet_WithRowAndColumnIndices_ReadsAndWritesExpectedLocation()
		{
			// Arrange
			var grid = new Grid<string>(4, 4);
			int row = 2;
			int column = 2;
			string expectedItem = "foo";

			// Act
			grid[row, column] = expectedItem;
			string item = grid[row, column];

			// Assert
			Assert.AreEqual(expectedItem, item);
		}
		#endregion

		#region Flatten
		[TestMethod]
		public void Flatten_OrderIsRowMajor_ReturnsListWithItemsChosenInRowMajorOrder()
		{
			// Arrange
			var grid = new Grid<int>(3, 3);
			var expectedList = new List<int>();

			int count = 0;
			for (int i = 0; i < 3; ++i)
			{
				for (int j = 0; j < 3; ++j)
				{
					expectedList.Add(count);
					grid[i, j] = count++;
				}
			}

			// Act
			var list = grid.Flatten(GridOrder.RowMajor);

			// Assert
			CollectionAssert.AreEqual(expectedList, list);
		}

		[TestMethod]
		public void Flatten_OrderIsColumnMajor_ReturnsListWithItemsChosenInColumnMajorOrder()
		{
			// Arrange
			var grid = new Grid<int>(3, 3);
			var expectedList = new List<int>();

			int count = 0;
			for (int i = 0; i < 3; ++i)
			{
				for (int j = 0; j < 3; ++j)
				{
					expectedList.Add(count);
					grid[j, i] = count++;
				}
			}

			// Act
			var list = grid.Flatten(GridOrder.ColumnMajor);

			// Assert
			CollectionAssert.AreEqual(expectedList, list);
		}
		#endregion

		#region RowAt
		[TestMethod]
		public void RowAt_RowIndexIsNegative_ThrowsArgumentException()
		{
			// Arrange
			var grid = new Grid<int>(1, 2);

			// Act
			var row = grid.RowAt(-1);

			// Assert
			Assert.ThrowsException<ArgumentException>(() => { row.ToList(); });
		}

		[TestMethod]
		public void RowAt_RowIndexIsGreaterThanGreatestPossibleIndex_ThrowsArgumentException()
		{
			// Arrange
			int rows = 7;
			var grid = new Grid<int>(rows, 3);

			// Act
			var row = grid.RowAt(-1);

			// Assert
			Assert.ThrowsException<ArgumentException>(() => { row.ToList(); });
		}

		[TestMethod]
		public void RowAt_ValidRowIndex_ReturnsEnumeratorOverThatRow()
		{
			// Arrange
			var grid = new Grid<int>(4, 4);
			var expectedRow = new List<int>();
			int rowIndex = 3;
			int count = 0;

			for (int i = 0; i < 4; ++i)
			{
				expectedRow.Add(count);
				grid[rowIndex, i] = count++;
			}

			// Act
			var row = grid.RowAt(rowIndex);

			// Assert
			CollectionAssert.AreEqual(expectedRow, row.ToList());
		}
		#endregion

		#region ColumnAt
		[TestMethod]
		public void ColumnAt_ColumnIndexIsNegative_ThrowsArgumentException()
		{
			// Arrange
			var grid = new Grid<int>(2, 5);

			// Act
			var column = grid.ColumnAt(-1);

			// Assert
			Assert.ThrowsException<ArgumentException>(() => { column.ToList(); });
		}

		[TestMethod]
		public void ColumnAt_ColumnIndexIsGreaterThanGreatestPossibleIndex_ThrowsArgumentException()
		{
			// Arrange
			int columns = 1;
			var grid = new Grid<int>(3, columns);

			// Act
			var column = grid.ColumnAt(columns);

			// Assert
			Assert.ThrowsException<ArgumentException>(() => { column.ToList(); });
		}

		[TestMethod]
		public void ColumnAt_ValidColumnIndex_ReturnsEnumeratorOverThatColumn()
		{
			// Arrange
			var grid = new Grid<int>(2, 3);
			var expectedColumn = new List<int>();
			int columnIndex = 0;
			int count = 0;

			for (int i = 0; i < 2; ++i)
			{
				expectedColumn.Add(count);
				grid[i, columnIndex] = count++;
			}

			// Act
			var column = grid.ColumnAt(columnIndex);

			// Assert
			CollectionAssert.AreEqual(expectedColumn, column.ToList());
		}
		#endregion

		#region GetNeighbors
		[TestMethod]
		public void GetNeighbors_CellIsInCenterOfGridAndDiagonalsIncluded_ReturnsAllNeighbors()
		{
			// Arrange
			var grid = new Grid<int>(3, 3);
			FillIntegerGrid(grid);
			int[] expectedNeighbors = new int[]
			{
				0, 1, 2,
				3,    5,
				6, 7, 8
			};

			// Act
			var neighbors = grid.GetNeighbors(new GridCell(1, 1));

			// Assert
			CollectionAssert.AreEqual(expectedNeighbors, neighbors.ToList());
		}

		[TestMethod]
		public void GetNeighbors_CellIsInCenterOfGridAndDiagonalsExcluded_ReturnsNonDiagonalNeighbors()
		{
			// Arrange
			var grid = new Grid<int>(3, 3);
			FillIntegerGrid(grid);
			int[] expectedNeighbors = new int[]
			{
				   1,
				3,    5,
				   7
			};

			// Act
			var neighbors = grid.GetNeighbors(new GridCell(1, 1), true);

			// Assert
			CollectionAssert.AreEqual(expectedNeighbors, neighbors.ToList());
		}

		[TestMethod]
		public void GetNeighbors_CellIsInCornerAndDiagonalsIncluded_ReturnsThreeNeighbors()
		{
			// Arrange
			var grid = new Grid<int>(3, 3);
			FillIntegerGrid(grid);
			int[] expectedNeighbors = new int[]
			{
				   1,
				3, 4
			};

			// Act
			var neighbors = grid.GetNeighbors(new GridCell(0, 0));

			// Assert
			CollectionAssert.AreEqual(expectedNeighbors, neighbors.ToList());
		}

		[TestMethod]
		public void GetNeighbors_CellIsInCornerAndDiagonalsExcluded_ReturnsTwoNeighbors()
		{
			// Arrange
			var grid = new Grid<int>(3, 3);
			FillIntegerGrid(grid);
			int[] expectedNeighbors = new int[]
			{
				   1,
				3
			};

			// Act
			var neighbors = grid.GetNeighbors(new GridCell(0, 0), true);

			// Assert
			CollectionAssert.AreEqual(expectedNeighbors, neighbors.ToList());
		}

		[TestMethod]
		public void GetNeighbors_CellIsOnSideAndDiagonalsIncluded_ReturnsFiveNeighbors()
		{
			// Arrange
			var grid = new Grid<int>(3, 3);
			FillIntegerGrid(grid);
			int[] expectedNeighbors = new int[]
			{
				0, 1,
				   4,
				6, 7
			};

			// Act
			var neighbors = grid.GetNeighbors(new GridCell(1, 0));

			// Assert
			CollectionAssert.AreEqual(expectedNeighbors, neighbors.ToList());
		}

		[TestMethod]
		public void GetNeighbors_CellIsOnSideAndDiagonalsExcluded_ReturnsThreeNeighbors()
		{
			// Arrange
			var grid = new Grid<int>(3, 3);
			FillIntegerGrid(grid);
			int[] expectedNeighbors = new int[]
			{
				0,
				   4,
				6
			};

			// Act
			var neighbors = grid.GetNeighbors(new GridCell(1, 0), true);

			// Assert
			CollectionAssert.AreEqual(expectedNeighbors, neighbors.ToList());
		}
		#endregion

		#region Enumeration
		[TestMethod]
		public void Enumeration_AtAnyTime_EnumeratesItemsInRowMajorOrder()
		{
			// Arrange
			var grid = new Grid<int>(5, 5);
			FillIntegerGrid(grid);
			var expectedItems = grid.Flatten(GridOrder.RowMajor);

			// Act
			var items = new List<int>(grid);

			// Assert
			CollectionAssert.AreEqual(expectedItems, items);
		}
		#endregion

		#region GetItemLocationPairs
		[TestMethod]
		public void GetItemLocationPairs_OrderIsRowMajor_ReturnsItemsAndTheirCellsInRowMajorOrder()
		{
			// Arrange
			var grid = new Grid<int>(2, 2);
			FillIntegerGrid(grid);

			// Act & Assert
			int row = 0;
			int col = 0;
			foreach (var pair in grid.GetItemLocationPairs(GridOrder.RowMajor))
			{
				var expectedCell = new GridCell(row, col);
				var expectedValue = grid[expectedCell];

				Assert.AreEqual(expectedCell, pair.Key);
				Assert.AreEqual(expectedValue, pair.Value);

				++col;
				if (col == grid.Columns)
				{
					col = 0;
					++row;
				}
			}
		}

		[TestMethod]
		public void GetItemLocationPairs_OrderIsColumnMajor_ReturnsItemsAndTheirCellsInColumnMajorOrder()
		{
			// Arrange
			var grid = new Grid<int>(4, 3);
			FillIntegerGrid(grid);

			// Act & Assert
			int row = 0;
			int col = 0;
			foreach (var pair in grid.GetItemLocationPairs(GridOrder.ColumnMajor))
			{
				var expectedCell = new GridCell(row, col);
				var expectedValue = grid[expectedCell];

				Assert.AreEqual(expectedCell, pair.Key);
				Assert.AreEqual(expectedValue, pair.Value);

				++row;
				if (row == grid.Rows)
				{
					row = 0;
					++col;
				}
			}
		}
		#endregion

		private void FillIntegerGrid(Grid<int> grid)
		{
			int count = 0;

			for (int row = 0; row < grid.Rows; ++row)
			{
				for (int col = 0; col < grid.Columns; ++col)
				{
					grid[row, col] = count++;
				}
			}
		}
	}
}