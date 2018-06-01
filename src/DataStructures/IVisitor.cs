namespace Tools.DataStructures {

	/// <summary>
	/// A generic interface for the Visitor pattern.
	/// </summary>
	/// <typeparam name="T">the type of object to visit</typeparam>
	public interface IVisitor<T>
	{
		/// <summary>
		/// Performs the visit operation on an item.
		/// </summary>
		/// <param name="item">the item</param>
		void Visit(T item);
	}

}
