﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
		/// Creates a Grid from a flat list.
		/// </summary>
		/// <param name="data">the list</param>
		/// <param name="rows">the number of rows to create</param>
		/// <param name="columns">the number of columns to create</param>
		/// <param name="order">the order to fill the grid in</param>
		public static Grid<T> Unflatten(
			IEnumerable<T> data,
			int rows,
			int columns,
			GridOrder order = GridOrder.RowMajor
		)
		{
			Validate.IsNotNull(data, "data");

			var grid = new Grid<T>(rows, columns);
			var dataEnumerator = data.GetEnumerator();

			foreach (var cell in grid.GetCellsInOrder(order))
			{
				if (dataEnumerator.MoveNext())
					grid[cell] = dataEnumerator.Current;
				else
					break;
			}

			return grid;
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

		/// <summary>
		/// Creates a grid and initializes every cell with a value.
		/// </summary>
		/// <param name="rows">the number of rows to create</param>
		/// <param name="columns">the number of columns to create</param>
		/// <param name="fillValue">the value to use</param>
		public Grid(int rows, int columns, T fillValue)
			: this(rows, columns)
		{
			foreach (var cell in GetCellsInOrder())
			{
				this[cell] = fillValue;
			}
		}

		/// <summary>
		/// Constructs a copy of another grid.
		/// </summary>
		/// <remarks>
		/// If T is a value type, this does a deep copy. Otherwise, the references
		/// in `other` are copied to this grid.
		/// </remarks>
		/// <param name="other">the other grid</param>
		public Grid(Grid<T> other)
		{
			Validate.IsNotNull(other, "other");

			Rows = other.Rows;
			Columns = other.Columns;
			Data = new T[Rows, Columns];

			foreach (var pair in other.GetItemLocationPairs())
			{
				this[pair.Key] = pair.Value;
			}
		}

		/// <summary>
		/// Gets or sets the item at a certain cell.
		/// </summary>
		/// <param name="cell">the target cell</param>
		public T this[GridCell cell]
		{
			get
			{
				return this[cell.Row, cell.Column];
			}

			set
			{
				this[cell.Row, cell.Column] = value;
			}
		}

		/// <summary>
		/// Gets or sets the item at a certain cell.
		/// </summary>
		/// <param name="row">the row of the target cell</param>
		/// <param name="column">the column of the target cell</param>
		public T this[int row, int column]
		{
			get
			{
				return Data[row, column];
			}

			set
			{
				Data[row, column] = value;
			}
		}

		/// <summary>
		/// Returns a copy of the grid flattened into a 1D list.
		/// </summary>
		/// <param name="order">the order in which to move items from the grid to the
		/// list (default is row-major)</param>
		public List<T> Flatten(GridOrder order = GridOrder.RowMajor)
		{
			return new List<T>(GetCellsInOrder(order).Select(cell => this[cell]));
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
		/// cells are neighbors if they touch on any side or corner. Items are returned
		/// in row-major order.
		/// </summary>
		/// <param name="cell">the cell whose neighbors will be returned</param>
		/// <param name="excludeDiagonals">if true, cells that touch on a corner are ignored</param>
		public IEnumerable<T> GetNeighbors(GridCell cell, bool excludeDiagonals = false)
		{
			foreach (var cellNeighbor in GetNeighborCells(cell, excludeDiagonals))
			{
				yield return this[cellNeighbor];
			}
		}

		/// <summary>
		/// Returns an enumerable over the cells that neighbor a given cell. Two cells are
		/// neighbors if they touch on any side or corner. Cells are returned in row-major
		/// order.
		/// </summary>
		/// <param name="cell">the cell whose neighbors will be returned</param>
		/// <param name="excludeDiagonals">if true, cells that touch on a corner are ignored</param>
		public IEnumerable<GridCell> GetNeighborCells(GridCell cell, bool excludeDiagonals = false)
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
		/// Returns an enumerable of the grid's contents and their locations (cells)
		/// in a particular order.
		/// </summary>
		/// <param name="order">the enumeration order (default is row-major)</param>
		public IEnumerable<KeyValuePair<GridCell, T>> GetItemLocationPairs(
			GridOrder order = GridOrder.RowMajor
		)
		{
			foreach (var cell in GetCellsInOrder(order))
			{
				yield return new KeyValuePair<GridCell, T>(cell, this[cell]);
			}
		}

		/// <summary>
		/// Returns an enumerator over the grid's contents in row-major order.
		/// </summary>
		public IEnumerator<T> GetEnumerator()
		{
			foreach (var cell in GetCellsInOrder(GridOrder.RowMajor))
			{
				yield return this[cell];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/*
		 * Returns an enumerable of cells in the order specified.
		 */
		private IEnumerable<GridCell> GetCellsInOrder(GridOrder order = GridOrder.RowMajor)
		{
			switch (order)
			{
				case GridOrder.RowMajor:
					for (int row = 0; row < Rows; ++row)
					{
						for (int column = 0; column < Columns; ++column)
						{
							yield return new GridCell(row, column);
						}
					}
					break;

				case GridOrder.ColumnMajor:
					for (int column = 0; column < Columns; ++column)
					{
						for (int row = 0; row < Rows; ++row)
						{
							yield return new GridCell(row, column);
						}
					}
					break;

				default:
					throw new ArgumentException("Unknown GridOrder");
			}
		}
	}

}
