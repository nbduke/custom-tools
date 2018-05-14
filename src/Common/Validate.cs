using System;

namespace Tools {

	/// <summary>
	/// A collection of helper methods for common input validation.
	/// </summary>
	public static class Validate
	{
		public static void IsNotNull(object obj, string paramName)
		{
			if (obj == null)
				throw new ArgumentNullException(paramName);
		}

		public static void IsNotNullOrEmpty(string s)
		{
			IsNotNullOrEmpty(s, "The string cannot be empty");
		}

		public static void IsNotNullOrEmpty(string s, string message)
		{
			if (string.IsNullOrEmpty(s))
				throw new ArgumentException(message);
		}

		public static void IsTrue(bool condition, string message)
		{
			if (!condition)
				throw new ArgumentException(message);
		}
	};

}
