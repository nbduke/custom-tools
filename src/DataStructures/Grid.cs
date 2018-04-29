/*
 * Grid.cs
 * 
 * Nathan Duke
 * 1/31/15
 * 
 * Contains the Grid<T> class and the GridOrder enum.
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonTools { namespace DataStructures {

	/// <summary>
	/// Specifies the traversal order over a grid (a 2D array).
	/// </summary>
	public enum GridOrder
	{
		RowMajor,
		ColumnMajor
	}

	/// <summary>
	/// A wrapper around a 2D array of an arbitrary type. Grid provides useful functions
	/// for iterating over and searching the array.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Grid<T> : IEnumerable<T>
	{
		public int Rows { get; private set; }
		public int Columns { get; private set; }
		public int Cells { get { return Rows * Columns; } }
		public bool IsSquare { get { return Rows == Columns; } }

		private T[,] Data { get; set; }

		public Grid(int rows, int columns, IEnumerable<T> items, GridOrder fillOrder = GridOrder.RowMajor)
			: this(rows, columns)
		{
			if (items == null)
				throw new ArgumentNullException("items cannot be null.");

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

		public Grid(int rows, int columns, T fillValue)
			: this(rows, columns)
		{
			VisitCellsInOrder(GridOrder.RowMajor,
				(cell) =>
				{
					this[cell] = fillValue;
				});
		}

		public Grid(int rows, int columns)
		{
			if (rows < 0 || columns < 0)
				throw new ArgumentException("rows and columns must be nonnegative integers.");

			Rows = rows;
			Columns = columns;
			Data = new T[Rows, Columns];
		}

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

		public bool Contains(T item)
		{
			foreach (T datum in Data)
			{
				if (datum.Equals(item))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Returns a copy of the grid flattened into a 1D array.
		/// </summary>
		/// <param name="order">Specifies the order in which elements are moved from the grid to the array</param>
		/// <returns></returns>
		public List<T> Flatten(GridOrder order)
		{
			List<T> result = new List<T>();
			VisitCellsInOrder(order, (cell) => { result.Add(this[cell]); });
			return result;
		}

		public IEnumerable<T> RowAt(int rowIndex)
		{
			if (rowIndex < 0 || rowIndex >= Rows)
				throw new ArgumentException("Invalid row index.");

			for (int i = 0; i < Columns; ++i)
			{
				yield return this[rowIndex, i];
			}
		}

		public IEnumerable<T> ColumnAt(int columnIndex)
		{
			if (columnIndex < 0 || columnIndex >= Columns)
				throw new ArgumentException("Invalid column index.");

			for (int i = 0; i < Rows; ++i)
			{
				yield return this[i, columnIndex];
			}
		}

		/// <summary>
		/// Returns the items in the grid neighboring the given cell. Neighbors are considered
		/// to be all items in the grid touching a corner or an edge of the item in the given cell.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="excludeDiagonals">If true, neighbors touching corners of the item will be excluded</param>
		/// <returns></returns>
		public IEnumerable<T> GetNeighbors(GridCell cell, bool excludeDiagonals = false)
		{
			foreach (var cellNeighbor in GetCellNeighbors(cell, excludeDiagonals))
			{
				yield return this[cellNeighbor];
			}
		}

		/// <summary>
		/// Returns the cell locations that neighbor the given cell. Neighbors are considered
		/// to be all cells in the grid touching a corner or an edge of the given cell.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="excludeDiagonals">If true, neighbors touching corners of the cell will be excluded</param>
		/// <returns></returns>
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
		/// Returns an IEnumerator over the grid that enumerates in row-major order.
		/// </summary>
		/// <returns></returns>
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
			return this.GetEnumerator();
		}

		/// <summary>
		/// Performs an action on each cell in the grid. Cells are visited in the
		/// specified order.
		/// </summary>
		/// <param name="order">The order in which to act on cells</param>
		/// <param name="visit">The action to perform on cells</param>
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

}}
