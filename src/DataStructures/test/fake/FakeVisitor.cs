using Tools.DataStructures;

namespace Test {

	class FakeVisitor<T> : IVisitor<T>
	{
		public T LastVisited;

		public void Visit(T item)
		{
			LastVisited = item;
		}
	};

}
