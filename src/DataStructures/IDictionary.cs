/*
 * IDictionary.cs
 * 
 * Nathan Duke
 * 1/31/15
 * 
 * Contains the IDictionary<T> interface.
 */

namespace CommonTools { namespace DataStructures {

	/// <summary>
	/// An interface that dictionaries should implement.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IDictionary<T>
	{
		/// <summary>
		/// The number of entries in the dictionary.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Inserts the entry into the dictionary.
		/// </summary>
		/// <param name="entry"></param>
		/// <returns>True iff the insertion was successful</returns>
		bool Insert(T entry);

		/// <summary>
		/// Deletes all occurrences of the entry from the dictionary.
		/// </summary>
		/// <param name="entry"></param>
		/// <returns>The number of entries or occurrences deleted</returns>
		int Delete(T entry);

		/// <summary>
		/// Returns true iff entry is in the dictionary.
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		bool Contains(T entry);
	}

}}
