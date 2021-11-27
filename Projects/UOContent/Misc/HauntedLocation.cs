using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Server.Json;

namespace Server
{
    public class HauntedLocation
    {
        private static readonly Dictionary<string, HauntedLocation> m_Table =
            new(StringComparer.OrdinalIgnoreCase);

        [JsonPropertyName("type")] public string Type { get; set; }

        [JsonPropertyName("values")] public Point2D[] List { get; set; }

        public Point2D GetRandomHauntedLocation() => List.RandomElement();

        public static Point2D GetHauntedLocation(string type) {
            HauntedLocation n = GetHauntedLocations(type);
            if (n != null)
            {
                return n.GetRandomHauntedLocation();
            }
            return new Point2D(0,0);
        }
        public static HauntedLocation GetHauntedLocations(string type)
        {
            m_Table.TryGetValue(type, out var n);
            return n;
        }
        public static Point2D RandomLocation(string type) => GetHauntedLocation(type);

        public static void Configure()
        {
            var filePath = Path.Combine(Core.BaseDirectory, "Data/Haunted/locations.json");

            var hauntedLocations = JsonConfig.Deserialize<List<HauntedLocation>>(filePath);

            if (hauntedLocations == null)
            {
                throw new JsonException($"Failed to deserialize {filePath}.");
            }

            foreach (var hauntedLocation in hauntedLocations)
            {
                m_Table.Add(hauntedLocation.Type, hauntedLocation);
            }
        }
    }
}
