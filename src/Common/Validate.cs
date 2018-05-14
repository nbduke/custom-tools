using System;

namespace Tools {

	/// <summary>
	/// A collection of helper methods for common input validation.
	/// </summary>
	public static class Validate
	{
		public static void IsNotNull(object obj, string name)
		{
			if (obj == null)
				throw new ArgumentNullException(name);
		}
	};

}
