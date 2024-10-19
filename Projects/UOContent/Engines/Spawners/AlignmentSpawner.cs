using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Server.Json;
using Server.Pantheon;

namespace Server.Engines.Spawners
{
    public class AlignmentSpawner : Spawner
    {
        public List<DynamicJson> Zones;
        [Constructible(AccessLevel.Developer)]
        public AlignmentSpawner()
        {
        }

        [Constructible(AccessLevel.Developer)]
        public AlignmentSpawner(string spawnedName) : base(spawnedName)
        {
        }

        [Constructible(AccessLevel.Developer)]
        public AlignmentSpawner(
            int amount, int minDelay, int maxDelay, int team, int homeRange,
            params string[] spawnedNames
        ) : this(
            amount,
            TimeSpan.FromMinutes(minDelay),
            TimeSpan.FromMinutes(maxDelay),
            team,
            homeRange,
            spawnedNames
        )
        {
        }

        [Constructible(AccessLevel.Developer)]
        public AlignmentSpawner(
            int amount, TimeSpan minDelay, TimeSpan maxDelay, int team, int homeRange,
            params string[] spawnedNames
        ) : base(amount, minDelay, maxDelay, team, homeRange, spawnedNames)
        {
        }

        public AlignmentSpawner(DynamicJson json, JsonSerializerOptions options) : base(json, options)
        {
        }

        public AlignmentSpawner(Serial serial) : base(serial)
        {
        }

        public double GetDistanceToSqrt(Point3D pointOne, Point3D pointTwo)
        {
            var xDelta = pointOne.X - pointTwo.X;
            var yDelta = pointOne.Y - pointTwo.Y;

            return Math.Sqrt(xDelta * xDelta + yDelta * yDelta);
        }

        public bool InRange(Point3D pointOne, Point3D pointTwo, int distance) =>
            (pointOne.X - pointTwo.X) + (pointOne.Y - pointTwo.Y) < distance;

        public override void Spawn()
        {
            if (Zones is null)
            {
                FileInfo fileInfo = new FileInfo("Data/Spawns/alignment-zones.json");
                Zones = JsonConfig.Deserialize<List<DynamicJson>>(fileInfo.FullName);
            }
            var options = JsonConfig.GetOptions(new TextDefinitionConverterFactory());
            foreach (var zone in Zones)
            {
                Map alignmentMap = zone.GetProperty("map", options, out Map map) ? map : null;
                int distance = zone.GetProperty("distance", options, out int value) ? value : 0;
                zone.GetProperty("location", options, out Point3D location);
                Deity.Alignment alignment = Deity.AlignmentFromString(zone.Type);
                double test = GetDistanceToSqrt(location, Location);
                if (Map == alignmentMap &&
                    test <= distance
                   )
                {
                    Type[] team = alignment switch
                    {
                        Deity.Alignment.Light    => OppositionGroup.LesserHumanoidGroup,
                        Deity.Alignment.Darkness => OppositionGroup.LesserUndeadGroup,
                        Deity.Alignment.Order    => OppositionGroup.LesserReptilianGroup,
                        Deity.Alignment.Chaos    => OppositionGroup.LesserAbyssalGroup,
                        _                        => Array.Empty<Type>()
                    };
                    if (team.Length > 0)
                    {
                        foreach (var entry in Entries)
                        {
                            string newName = team[Utility.Random(team.Length)].Name;
                            while (string.Equals(newName, "KhaldunRevenant") || string.Equals(newName, "Revenant"))
                            {
                                newName = team[Utility.Random(team.Length)].Name;
                            }
                            entry.SpawnedName = team[Utility.Random(team.Length)].Name;
                        }
                    }
                }
            }

            base.Spawn();
        }
        /*
        public override bool OnDefragSpawn(ISpawnable spawned, bool remove)
        {
          // To despawn a mob that was lured 4x away from its spawner
          // TODO: Move this to a config
          if (spawned is BaseCreature c && c.Combatant == null && c.GetDistanceToSqrt( Location ) > c.RangeHome * 4)
          {
            c.Delete();
            remove = true;
          }

          return base.OnDefragSpawn(entry, spawned, remove);
        }
        */


        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadEncodedInt();
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }
    }
}
