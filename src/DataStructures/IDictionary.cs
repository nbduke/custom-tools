namespace Tools.DataStructures {

	/*
	 * An interface for a generic dictionary.
	 */
	public interface IDictionary<T>
	{
		/*
		 * The number of entries in the dictionary.
		 */
		int Count { get; }

		/*
		 * Inserts the entry into the dictionary. Returns true if the
		 * insertion succeeded.
		 */
		bool Insert(T entry);

		/*
		 * Deletes all occurrences of the entry from the dictionary and
		 * returns the number of items deleted.
		 */
		int Delete(T entry);

		/*
		 * Returns true if the item is in the dictionary.
		 */
		bool Contains(T entry);
	}

}
