/*
 * IVisitor.cs
 * 
 * Nathan Duke
 * 1/31/15
 * 
 * Contains the IVisitor<T> interface.
 */

namespace CommonTools { namespace DataStructures {

	/// <summary>
	/// An interface for the Visitor Pattern.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IVisitor<T>
	{
		/// <summary>
		/// Performs the visit operation on the given item.
		/// </summary>
		/// <param name="item"></param>
		void Visit(T item);
	}

}}
