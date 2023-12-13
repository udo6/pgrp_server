using AltV.Net;
using Newtonsoft.Json;

namespace Game.Streamer
{
	public static class MarkerStreamer
	{
		public static readonly List<Core.Models.Marker> Markers = new();

		public static int AddMarker(Core.Models.Marker obj)
		{
			Markers.Add(obj);
			Alt.EmitAllClients("Client:MarkerStreamer:AddMarker", JsonConvert.SerializeObject(obj));
			return obj.Id;
		}

		public static void RemoveMarker(int id)
		{
			var obj = Markers.FirstOrDefault(x => x.Id == id);
			if (obj == null) return;

			Markers.Remove(obj);
			Alt.EmitAllClients("Client:MarkerStreamer:RemoveMarker", id);
		}

		public static void RemoveMarkers(List<int> ids)
		{
			Console.WriteLine(10);
			foreach(var marker in Markers.ToList())
			{
				if (!ids.Any(x => marker.Id == x)) continue;

				Markers.Remove(marker);
			}
			Console.WriteLine(11);

			Alt.EmitAllClients("Client:MarkerStreamer:RemoveMarkers", JsonConvert.SerializeObject(ids));
			Console.WriteLine(12);
		}
	}
}
