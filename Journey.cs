using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KiwiTrains
{
    internal class Journey
    {
        private readonly List<Edge> _routes;

        public Journey() => _routes = new List<Edge>();

        public int Distance => _routes.Sum(edge => edge.Distance);

        public int Stops => _routes.Count;

        public Node StartNode => _routes[0].From;

        public Node LastNode => _routes[^1].To;

        public void AddRoute(Edge edge) => _routes.Add(edge);

        public Edge[] GetRoutes() => _routes.ToArray();

        public void Clear() => _routes.Clear();

        public void Reverse() => _routes.Reverse();

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (_routes.Count <= 0) return sb.ToString();
            sb.Append(_routes[0].From.Name);
            foreach (var route in _routes)
            {
                sb.Append('-');
                sb.Append(route.To.Name);
            }

            sb.Append(" (distance = ");
            sb.Append(Distance);
            sb.Append(" )");
            return sb.ToString();
        }
    }
}