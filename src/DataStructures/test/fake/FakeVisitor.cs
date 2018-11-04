using System.Collections.Generic;
using Tools.DataStructures;

namespace Test {

	class FakeVisitor<T> : IVisitor<T>
	{
		public Stack<T> Visited = new Stack<T>();

		public void Visit(T item)
		{
			Visited.Push(item);
		}
	};

}
