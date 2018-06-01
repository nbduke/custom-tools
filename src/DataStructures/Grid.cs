using System;
using System.Collections;
using System.Collections.Generic;

namespace Tools.DataStructures {

	/// <summary>
	/// Specifies the traversal order over a grid.
	/// </summary>
	public enum GridOrder
	{
		RowMajor,
		ColumnMajor
	}

	/// <summary>
	/// Grid is a wrapper around a 2D array that provides useful functions for iterating
	/// over and searching the array.
	/// </summary>
	public class Grid<T> : IEnumerable<T>
	{
		public readonly int Rows;
		public readonly int Columns;
		public int Cells
		{
			get { return Rows * Columns; }
		}
		public bool IsSquare
		{
			get { return Rows == Columns; }
		}

		private readonly T[,] Data;

		/// <summary>
		/// Creates a grid and fills it with a collection.
		/// </summary>
		/// <param name="rows">the number of rows to create</param>
		/// <param name="columns">the number of columns to create</param>
		/// <param name="items">the collection</param>
		/// <param name="fillOrder">the order in which items should be moved from the
		/// collection to the grid</param>
		public Grid(
			int rows,
			int columns,
			IEnumerable<T> items,
			GridOrder fillOrder = GridOrder.RowMajor)
			: this(rows, columns)
		{
			Validate.IsNotNull(items, "items");

			var enumerator = items.GetEnumerator();
			VisitCellsInOrder(fillOrder,
				(cell) =>
				{
					if (enumerator.MoveNext())
						this[cell] = enumerator.Current;
				});

			if (enumerator.MoveNext())
				throw new ArgumentException("The grid is too small to hold all of the given items.");
		}

		/// <summary>
		/// Creates a grid and initializes every cell with a value.
		/// </summary>
		/// <param name="rows">the number of rows to create</param>
		/// <param name="columns">the number of columns to create</param>
		/// <param name="fillValue">the value to use</param>
		public Grid(int rows, int columns, T fillValue)
			: this(rows, columns)
		{
			VisitCellsInOrder(GridOrder.RowMajor,
				(cell) =>
				{
					this[cell] = fillValue;
				});
		}

		/// <summary>
		/// Creates an empty grid.
		/// </summary>
		/// <param name="rows">the number of rows to create</param>
		/// <param name="columns">the number of columns to create</param>
		public Grid(int rows, int columns)
		{
			Validate.IsTrue(rows >= 0 && columns >= 0,
				"rows and columns must be nonnegative integers.");

			Rows = rows;
			Columns = columns;
			Data = new T[Rows, Columns];
		}

		public T this[GridCell cell]
		{
			get { return this[cell.Row, cell.Column]; }

			set { this[cell.Row, cell.Column] = value; }
		}

		public T this[int row, int column]
		{
			get { return Data[row, column]; }

			set { Data[row, column] = value; }
		}

		/// <summary>
		/// Returns a copy of the grid flattened into a 1D array. The GridOrder parameter
		/// determines the order in which elements are moved from the grid to the array.
		/// </summary>
		/// <param name="order">the GridOrder to use</param>
		public List<T> Flatten(GridOrder order)
		{
			List<T> result = new List<T>();
			VisitCellsInOrder(order, (cell) => { result.Add(this[cell]); });
			return result;
		}

		/// <summary>
		/// Returns an enumerable over a row.
		/// </summary>
		/// <param name="rowIndex">the index of the desired row</param>
		public IEnumerable<T> RowAt(int rowIndex)
		{
			Validate.IsTrue(rowIndex >= 0 && rowIndex < Rows, "Invalid row index");

			for (int i = 0; i < Columns; ++i)
			{
				yield return this[rowIndex, i];
			}
		}

		/// <summary>
		/// Returns an enumerable over a column.
		/// </summary>
		/// <param name="columnIndex">the index of the desired column</param>
		public IEnumerable<T> ColumnAt(int columnIndex)
		{
			Validate.IsTrue(columnIndex >= 0 && columnIndex < Columns, "Invalid column index");

			for (int i = 0; i < Rows; ++i)
			{
				yield return this[i, columnIndex];
			}
		}

		/// <summary>
		/// Returns an enumerable over the items in cells that neighbor a given cell. Two
		/// cells are neighbors if they touch on any side or corner.
		/// </summary>
		/// <param name="cell">the cell whose neighbors will be returned</param>
		/// <param name="excludeDiagonals">if true, cells that touch on a corner are ignored</param>
		public IEnumerable<T> GetNeighbors(GridCell cell, bool excludeDiagonals = false)
		{
			foreach (var cellNeighbor in GetCellNeighbors(cell, excludeDiagonals))
			{
				yield return this[cellNeighbor];
			}
		}

		/// <summary>
		/// Returns an enumerable over the cells that neighbor a given cell. Two cells are
		/// neighbors if they touch on any side or corner.
		/// </summary>
		/// <param name="cell">the cell whose neighbors will be returned</param>
		/// <param name="excludeDiagonals">if true, cells that touch on a corner are ignored</param>
		public IEnumerable<GridCell> GetCellNeighbors(GridCell cell, bool excludeDiagonals = false)
		{
			for (int row = cell.Row - 1; row <= cell.Row + 1; ++row)
			{
				if (row >= 0 && row < Rows)
				{
					for (int column = cell.Column - 1; column <= cell.Column + 1; ++column)
					{
						if (column >= 0 && column < Columns &&
							!(row == cell.Row && column == cell.Column))
						{
							if (!excludeDiagonals || (row == cell.Row || column == cell.Column))
								yield return new GridCell(row, column);
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns an enumerable over the grid that iterates in row-major order.
		/// </summary>
		public IEnumerator<T> GetEnumerator()
		{
			for (int row = 0; row < Rows; ++row)
			{
				for (int column = 0; column < Columns; ++column)
				{
					yield return this[row, column];
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Applies an action to each cell in the grid.
		/// </summary>
		/// <param name="order">the order in which to iterate over cells</param>
		/// <param name="visit">the action to apply</param>
		public void VisitCellsInOrder(GridOrder order, Action<GridCell> visit)
		{
			if (order == GridOrder.RowMajor)
			{
				for (int row = 0; row < Rows; ++row)
				{
					for (int column = 0; column < Columns; ++column)
					{
						visit(new GridCell(row, column));
					}
				}
			}
			else if (order == GridOrder.ColumnMajor)
			{
				for (int column = 0; column < Columns; ++column)
				{
					for (int row = 0; row < Rows; ++row)
					{
						visit(new GridCell(row, column));
					}
				}
			}
			else
			{
				throw new ArgumentException("Unrecognized GridOrder.");
			}
		}
	}

}
