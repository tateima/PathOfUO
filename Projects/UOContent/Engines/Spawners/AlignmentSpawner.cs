/*************************************************************************
 * ModernUO                                                              *
 * Copyright 2019-2026 - ModernUO Development Team                       *
 * Email: hi@modernuo.com                                                *
 * File: RegionSpawner.cs                                                *
 *                                                                       *
 * This program is free software: you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation, either version 3 of the License, or     *
 * (at your option) any later version.                                   *
 *                                                                       *
 * You should have received a copy of the GNU General Public License     *
 * along with this program.  If not, see <http://www.gnu.org/licenses/>. *
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using ModernUO.Serialization;
using Server.Json;
using Server.Pantheon;
using Server.Regions;

namespace Server.Engines.Spawners;

[SerializationGenerator(0)]
public partial class AlignmentSpawner : Spawner
{
    [SerializableField(0, getter: "private", setter: "private")]
    private string _spawnRegionName;

    private BaseRegion _spawnRegion;

    public override Region Region => _spawnRegion;

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
        int amount,
        TimeSpan minDelay,
        TimeSpan maxDelay,
        int team = 0,
        params ReadOnlySpan<string> spawnedNames
    ) : base(amount, minDelay, maxDelay, team, spawnedNames: spawnedNames)
    {
    }

    [CommandProperty(AccessLevel.Developer)]
    public BaseRegion SpawnRegion
    {
        get => _spawnRegion;
        set
        {
            _spawnRegion = value;
            SpawnRegionName = _spawnRegion?.Name;
            _spawnRegion?.InitRectangles();
            InvalidateProperties();
        }
    }

    // RegionSpawner does not support spiral scan (disjoint rectangles make it ineffective)
    protected override bool SupportsSpiralScan => false;

    protected override Rectangle3D GetBoundsForSpawnAttempt()
    {
        if (_spawnRegion == null || _spawnRegion.TotalWeight <= 0)
        {
            return default;
        }

        // Pick a weighted random rectangle from the region
        var rand = Utility.Random(_spawnRegion.TotalWeight);

        for (var j = 0; j < _spawnRegion.RectangleWeights.Length; j++)
        {
            var curWeight = _spawnRegion.RectangleWeights[j];

            if (rand < curWeight)
            {
                return _spawnRegion.Rectangles[j];
            }

            rand -= curWeight;
        }

        return default;
    }

    protected override ReadOnlySpan<Rectangle3D> GetAllSpawnBounds() => _spawnRegion?.Rectangles;

    public override Point3D GetSpawnPosition(ISpawnable spawned, Map map)
    {
        // Check for region/map mismatch before delegating to base
        if (_spawnRegion == null || map == null || map == Map.Internal ||
            map != _spawnRegion.Map || _spawnRegion.TotalWeight <= 0)
        {
            return Location;
        }

        return base.GetSpawnPosition(spawned, map);
    }

    public override void GetSpawnerProperties(IPropertyList list)
    {
        base.GetSpawnerProperties(list);

        if (Running && _spawnRegion != null)
        {
            list.Add(1076228, $"{"region:"}\t{_spawnRegion.Name}"); // ~1_DUMMY~ ~2_DUMMY~
        }
    }

    public static List<AlignmentZone> Zones = new()
    {
        new(
            Deity.Alignment.Order,
            100,
            new Point3D(1764, 2352, 0)
        ),
        new(
            Deity.Alignment.Chaos,
            100,
            new Point3D(1602, 2366, 0)
        ),
        new(
            Deity.Alignment.Darkness,
            100,
            new Point3D(2436, 1098, 0)
        )
    };

    public double GetDistanceToSqrt(Point3D pointOne, Point3D pointTwo)
    {
        var xDelta = pointOne.X - pointTwo.X;
        var yDelta = pointOne.Y - pointTwo.Y;

        return Math.Sqrt(xDelta * xDelta + yDelta * yDelta);
    }
    public override void Spawn()
    {
        foreach (var zone in Zones)
        {
            Deity.Alignment alignment = zone.Alignment;
            double test = GetDistanceToSqrt(zone.Location, Location);
            if (Map == Map.Felucca || Map == Map.Trammel &&
                test <= zone.Distance
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

    [AfterDeserialization(false)]
    private void AfterDeserialization()
    {
        _spawnRegion = Region.Find(_spawnRegionName, Map) as BaseRegion;
        _spawnRegion?.InitRectangles();
    }
}
