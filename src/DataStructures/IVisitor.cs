namespace Tools.DataStructures {

	/*
	 * A generic interface for the Visitor pattern.
	 */
	public interface IVisitor<T>
	{
		/*
		 * Performs the visit operation on the given item.
		 */
		void Visit(T item);
	}

}
