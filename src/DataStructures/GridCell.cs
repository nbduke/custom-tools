namespace Tools.DataStructures {

	/// <summary>
	/// An ordered pair that uniquely locates a cell in a grid (a 2D array).
	/// </summary>
	public struct GridCell
	{
		public int Row { get; set; }
		public int Column { get; set; }

		public GridCell(int row, int column)
		{
			Row = row;
			Column = column;
		}

		public override bool Equals(object obj)
		{
			return (obj is GridCell other) && this == other;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int simpleHash = 17;
				simpleHash = simpleHash * 23 + Row;
				simpleHash = simpleHash * 23 + Column;
				return simpleHash;
			}
		}

		public override string ToString()
		{
			return $"[{Row}, {Column}]";
		}

		public static bool operator==(GridCell a, GridCell b)
		{
			return a.Row == b.Row && a.Column == b.Column;
		}

		public static bool operator!=(GridCell a, GridCell b)
		{
			return !(a == b);
		}
	}

}
