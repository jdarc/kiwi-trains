namespace KiwiTrains
{
	internal class Edge
	{
		public Node From { get; }

		public Node To { get; }

		public int Distance { get; }

		public Edge(Node from, Node to, int distance)
		{
			From = from;
			To = to;
			Distance = distance;
		}
	}
}
