using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Server.Json;
using Server.Items;
using System.Text.RegularExpressions;
using System.Linq;

namespace Server
{
    public class HauntedHook
    {
        private static readonly Dictionary<string, HauntedHook> m_Table =
            new(StringComparer.OrdinalIgnoreCase);

        [JsonPropertyName("type")] public string Type { get; set; }

        [JsonPropertyName("values")] public string[] List { get; set; }

        public string GetRandomHauntedHook() => List.RandomElement() ?? "";

        public static HauntedHook GetHauntedHook(string type)
        {
            m_Table.TryGetValue(type, out var n);
            return n;
        }

        public static string RandomHook(string type) => GetHauntedHook(type)?.GetRandomHauntedHook() ?? "";

        public static MatchCollection Matches(string value, bool location = true) {
            if (location)
            {
                return Regex.Matches(value, @"\@(town|dungeon|place)");
            }
            return Regex.Matches(value, @"\@(\w+)");
        }

        public static Match RandomMatch(string value) {
            MatchCollection matches = Matches(value);
            return Matches(value).RandomElement();
        }

        public static void ReplaceHookValue(ref string hook, string key, string value) {
             hook = hook.Replace(@"@" + key, value);
        }

        public static Point2D DecideLocation(ref string hook, string mapKey) {
            Point2D location = HauntedLocation.RandomLocation(mapKey + HauntedHook.RandomHook(mapKey + "town"));
            Match dynamicHookName = HauntedHook.RandomMatch(hook);
            if (dynamicHookName != null)
            {
                foreach (Group group in dynamicHookName.Groups)
                {
                    if (group.Value.Contains("@"))
                    {
                        continue;
                    }
                    string hookValue = string.Equals(group.Value, "place") ? HauntedHook.RandomHook(group.Value) : HauntedHook.RandomHook(mapKey + group.Value);
                    ReplaceHookValue(ref hook, group.Value, hookValue);
                    location = string.Equals(group.Value, "place") ? HauntedLocation.RandomLocation(mapKey + hookValue) : HauntedLocation.RandomLocation(hookValue);
                    break;
                }
            }            
            return location;            
        }
        public static string Parse(string hook) {
            foreach(Match match in Matches(hook, false)) {
                foreach(Group group in match.Groups) {
                    string replace = RandomHook(group.Value);
                    if (!string.IsNullOrEmpty(replace)) {
                        ReplaceHookValue(ref hook, group.Value, replace);
                    }
                }                
            }
            return hook + "...";
        }

        public static void Configure()
        {
            var filePath = Path.Combine(Core.BaseDirectory, "Data/Haunted/hooks.json");

            var hauntedHooks = JsonConfig.Deserialize<List<HauntedHook>>(filePath);

            if (hauntedHooks == null)
            {
                throw new JsonException($"Failed to deserialize {filePath}.");
            }

            foreach (var hauntedHook in hauntedHooks)
            {
                m_Table.Add(hauntedHook.Type, hauntedHook);
            }
        }
    }
}
