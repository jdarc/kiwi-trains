using System.Collections.Generic;
using System.Linq;

namespace KiwiTrains
{
	internal class RouteManager
	{
		private readonly Dictionary<string, Edge> _routeEdges;
		private readonly Dictionary<string, Node> _routeNodes;
		private GraphMatrix _matrix;

		public RouteManager()
		{
			_routeNodes = new Dictionary<string, Node>();
			_routeEdges = new Dictionary<string, Edge>();
		}

		public GraphMatrix Matrix
		{
			get
			{
				if (_matrix != null) return _matrix;
				_matrix = new GraphMatrix(_routeNodes.Count);
				foreach (var edge in _routeEdges.Values)
				{
					_matrix.SetElementAt(edge.From.Index, edge.To.Index, edge.Distance);
				}
				return _matrix;
			}
		}

		private static string GenerateEdgeKey(Node from, Node to) => GenerateEdgeKey(from.Index, to.Index);

		private static string GenerateEdgeKey(int from, int to) => $"{from},{to}";

		public Node[] GetNodesByName(params string[] names) => names.Select(FindNode).Where(node => node != null).ToArray();

		private void AddTown(string name)
		{
			if (!_routeNodes.ContainsKey(name)) _routeNodes.Add(name, new Node(_routeNodes.Count, name));
			_matrix = null;
		}

		private void AddRoute(string fromTown, string toTown, int distance)
		{
			var fromNode = FindNode(fromTown);
			var toNode = FindNode(toTown);

			if (fromNode == null || toNode == null) return;
			var edgeKey = GenerateEdgeKey(fromNode, toNode);

			if (_routeEdges.ContainsKey(edgeKey)) return;
			var edge = new Edge(fromNode, toNode, distance);
			_routeEdges.Add(edgeKey, edge);
			_matrix = null;
		}

		public void ParseGraphString(string graphCsv)
		{
			var fragments = graphCsv.Split(',');

			foreach (var fragment in fragments)
			{
				var normalizedFragment = fragment.Trim();
				if (normalizedFragment.Length <= 2) continue;
				var routeFrom = normalizedFragment[0].ToString();
				var routeTo = normalizedFragment[1].ToString();
				var distance = int.Parse(normalizedFragment.Substring(2));

				AddTown(routeFrom);
				AddTown(routeTo);
				AddRoute(routeFrom, routeTo, distance);
			}

			_matrix = null;
		}

		public Node FindNode(string name) => _routeNodes.ContainsKey(name) ? _routeNodes[name] : null;

		public Edge FindEdge(int from, int to)
		{
			var edgeKey = GenerateEdgeKey(from, to);
			return _routeEdges.ContainsKey(edgeKey) ? _routeEdges[edgeKey] : null;
		}

		public int CalculateRouteDistance(Node[] nodes)
		{
			if (nodes == null || nodes.Length <= 0) return -1;

			var toIndex = nodes[0].Index;
			var totalDistance = 0;
			for (var i = 1; i < nodes.Length; i++)
			{
				var fromIndex = toIndex;
				toIndex = nodes[i].Index;
				var distance = Matrix.GetElementAt(fromIndex, toIndex);
				if (distance > 0) totalDistance += distance; else return -1;
			}
			return totalDistance;
		}

		public Journey FindShortestRoute(Node from, Node to)
		{
			Journey shortestJourney = null;

			if (from != null && to != null)
			{
				var journeys = new LinkedNode(this, from).GenerateRoutes(to, 0, false, false);

				var shortestDistance = int.MaxValue;
				foreach (var journey in journeys)
				{
					var journeyDistance = journey.Distance;
					if (journeyDistance < shortestDistance)
					{
						shortestDistance = journeyDistance;
						shortestJourney = journey;
					}
				}
			}

			return shortestJourney;
		}

		public Journey[] AvailableByMaxStops(Node from, Node to, int stops, bool equalOp)
		{
			if (from != null && to != null && stops > 0)
			{
				var journeys = new LinkedNode(this, from).GenerateRoutes(to, stops - 1, equalOp, false);

				var filteredJourneys = new List<Journey>();
				foreach (var journey in journeys)
				{
					if (journey.LastNode == to)
					{
						if ((equalOp && journey.Stops == stops) || (!equalOp && journey.Stops <= stops))
						{
							filteredJourneys.Add(journey);
						}
					}
				}
				return filteredJourneys.ToArray();
			}
			return System.Array.Empty<Journey>();
		}

		public Journey[] AvailableByMaxDistance(Node from, Node to, int distance)
		{
			if (from != null && to != null && distance > 0)
			{
				var journeys = new LinkedNode(this, from).GenerateRoutes(to, 16, true, true);

				var filteredJourneys = new List<Journey>();
				foreach (var journey in journeys)
				{
					if (journey.LastNode == to && journey.Distance < distance)
					{
						filteredJourneys.Add(journey);
					}
				}
				return filteredJourneys.ToArray();
			}
			return new Journey[0];
		}
	}
}
