using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tools.DataStructures {

	/// <summary>
	/// Tracks counts of unique items.
	/// </summary>
	/// <typeparam name="T">the type of items to count</typeparam>
	public class Counter<T> : IEnumerable<KeyValuePair<T, int>>
	{
		private Dictionary<T, int> Counts;

		public Counter()
		{
			Counts = new Dictionary<T, int>();
		}

		/// <summary>
		/// Returns the sum of all counts in the Counter.
		/// </summary>
		public int Sum
		{
			get
			{
				return Counts.Values.Sum();
			}
		}

		/// <summary>
		/// Returns the number of unique items in the Counter.
		/// </summary>
		public int UniqueCount
		{
			get
			{
				return Counts.Count;
			}
		}

		/// <summary>
		/// Gets or sets the count of an item.
		/// </summary>
		/// <param name="item">the item</param>
		public int this[T item]
		{
			get
			{
				if (Counts.TryGetValue(item, out int count))
					return count;
				else
					return 0;
			}

			set
			{
				Validate.IsTrue(value >= 0, "The count cannot be negative.");
				Counts[item] = value;
			}
		}

		/// <summary>
		/// Increases the count of an item by one. If the item is not in the
		/// counter, it is inserted with a count of one.
		/// </summary>
		/// <param name="item">the item</param>
		public void Increment(T item)
		{
			this[item]++;
		}

		/// <summary>
		/// Decreases the count of an item by one, if the item is present and
		/// has a count greater than zero. Otherwise, no change occurs to the
		/// counter.
		/// </summary>
		/// <param name="item">the item</param>
		/// <returns>true if the count was changed</returns>
		public bool TryDecrement(T item)
		{
			if (Counts.TryGetValue(item, out int count) && count > 0)
			{
				Counts[item]--;
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Clears all entries from the counter.
		/// </summary>
		public void Clear()
		{
			Counts.Clear();
		}

		/// <summary>
		/// Returns an IEnumerator over pairs of items and their counts.
		/// </summary>
		public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
		{
			return Counts.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

}