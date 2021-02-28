using System;

namespace KiwiTrains
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
			var routeManager = new RouteManager();
			routeManager.ParseGraphString("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");

			var result1 = routeManager.CalculateRouteDistance(routeManager.GetNodesByName("A-B-C".Split('-')));
			Console.WriteLine(@"Output #1: {0}", result1 > 0 ? result1.ToString() : "NO SUCH ROUTE");

			var result2 = routeManager.CalculateRouteDistance(routeManager.GetNodesByName("A-D".Split('-')));
			Console.WriteLine(@"Output #2: {0}", result2 > 0 ? result2.ToString() : "NO SUCH ROUTE");

			var result3 = routeManager.CalculateRouteDistance(routeManager.GetNodesByName("A-D-C".Split('-')));
			Console.WriteLine(@"Output #3: {0}", result3 > 0 ? result3.ToString() : "NO SUCH ROUTE");

			var result4 = routeManager.CalculateRouteDistance(routeManager.GetNodesByName("A-E-B-C-D".Split('-')));
			Console.WriteLine(@"Output #4: {0}", result4 > 0 ? result4.ToString() : "NO SUCH ROUTE");

			var result5 = routeManager.CalculateRouteDistance(routeManager.GetNodesByName("A-E-D".Split('-')));
			Console.WriteLine(@"Output #5: {0}", result5 > 0 ? result5.ToString() : "NO SUCH ROUTE");

			var result6 = routeManager.AvailableByMaxStops(routeManager.FindNode("C"), routeManager.FindNode("C"), 3, false).Length;
			Console.WriteLine(@"Output #6: {0}", result6.ToString());

			var result7 = routeManager.AvailableByMaxStops(routeManager.FindNode("A"), routeManager.FindNode("C"), 4, true).Length;
			Console.WriteLine(@"Output #7: {0}", result7.ToString());

			var result8 = routeManager.FindShortestRoute(routeManager.FindNode("A"), routeManager.FindNode("C")).Distance;
			Console.WriteLine(@"Output #8: {0}", result8.ToString());

			var result9 = routeManager.FindShortestRoute(routeManager.FindNode("B"), routeManager.FindNode("B")).Distance;
			Console.WriteLine(@"Output #9: {0}", result9.ToString());

			var result10 = routeManager.AvailableByMaxDistance(routeManager.FindNode("C"), routeManager.FindNode("C"), 30).Length;
			Console.WriteLine(@"Output #10: {0}", result10.ToString());

        }
    }
}