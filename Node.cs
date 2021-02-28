namespace KiwiTrains
{
    internal class Node
    {
        public int Index { get; }

        public string Name { get; }

        public Node(int index, string name)
        {
            Index = index;
            Name = name;
        }
    }
}