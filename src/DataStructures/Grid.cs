using System;
using System.Collections;
using System.Collections.Generic;

namespace Tools.DataStructures {

	/*
	 * Specifies the traversal order over a grid.
	 */
	public enum GridOrder
	{
		RowMajor,
		ColumnMajor
	}

	/*
	 * Grid is a wrapper around a 2D array of any type. It provides useful functions for
	 * iterating over and searching the array.
	 */
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

		public Grid(
			int rows,
			int columns,
			IEnumerable<T> items,
			GridOrder fillOrder = GridOrder.RowMajor)
			: this(rows, columns)
		{
			if (items == null)
				throw new ArgumentNullException("items");

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
			get { return this[cell.Row, cell.Column]; }

			set { this[cell.Row, cell.Column] = value; }
		}

		public T this[int row, int column]
		{
			get { return Data[row, column]; }

			set { Data[row, column] = value; }
		}

		/*
		 * Returns a copy of the grid flattened into a 1D array. The GridOrder
		 * determines the order in which elements are moved from the grid to the array.
		 */
		public List<T> Flatten(GridOrder order)
		{
			List<T> result = new List<T>();
			VisitCellsInOrder(order, (cell) => { result.Add(this[cell]); });
			return result;
		}

		/*
		 * Returns an enumerable over the row at the given index.
		 */
		public IEnumerable<T> RowAt(int rowIndex)
		{
			if (rowIndex < 0 || rowIndex >= Rows)
				throw new ArgumentException("Invalid row index.");

			for (int i = 0; i < Columns; ++i)
			{
				yield return this[rowIndex, i];
			}
		}

		/*
		 * Returns an enumerable over the column at the given index
		 */
		public IEnumerable<T> ColumnAt(int columnIndex)
		{
			if (columnIndex < 0 || columnIndex >= Columns)
				throw new ArgumentException("Invalid column index.");

			for (int i = 0; i < Rows; ++i)
			{
				yield return this[i, columnIndex];
			}
		}

		/*
		 * Returns an enumerable over the items that neighbor the given cell. A cell is a
		 * neighbor if it touches the given cell on any side or corner. If excludeDiagonals
		 * is true, cells touching the corners are excluded.
		 */
		public IEnumerable<T> GetNeighbors(GridCell cell, bool excludeDiagonals = false)
		{
			foreach (var cellNeighbor in GetCellNeighbors(cell, excludeDiagonals))
			{
				yield return this[cellNeighbor];
			}
		}

		/*
		 * Returns an enumerable over the cells that neighbor the given cell.
		 */
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

		/*
		 * Returns an enumerable over the grid that iterates in row-major order.
		 */
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

		/*
		 * Applies an action to each cell in the grid, iterating according to the
		 * given GridOrder.
		 */
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
