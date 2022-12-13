using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Server.Json;
using Server.Network;

namespace Server.Dungeon;

public class DungeonLevelModHandler
{
    public static readonly string DungeonDifficultyRejectedMessage =
        "You can only change the difficulty for some dungeons on level one.";

    public static readonly string DungeonDifficultyNotNormal =
        "You cannot change the difficulty of this dungeon at this time.";

    public static readonly string DungeonDifficultyAlreadySetMessage =
        "This dungeon level is already set to this difficulty.";

    public static readonly string DungeonDifficultyChangedMessage =
        "You have changed the dungeon difficulty of this level for an hour. Which will take place in three minutes.";
    public static ConcurrentDictionary<string, DungeonLevelMod> DungeonLevelMods { get; set; }

    public static string DataPath = Path.Combine(Core.BaseDirectory, $"Data/DungeonMods/DungeonLevelMods.json");
    public static void AddMod(string name, DungeonLevelMod dungeonLevelMod)
    {
        DungeonLevelMods ??= new ConcurrentDictionary<string, DungeonLevelMod>();
        DungeonLevelMods.AddOrUpdate(name, dungeonLevelMod, (t, bt) => dungeonLevelMod);
    }
    public static void RemoveMod(string name)
    {
        if (DungeonLevelMods is not null)
        {
            DungeonLevelMod oldMod;
            DungeonLevelMods.Remove(name, out oldMod);
            if (oldMod is not null)
            {
                ResetDungeonSpawners(
                    oldMod.LocationMap,
                    new Point2D(oldMod.X2, oldMod.Y1),
                    new Point2D(oldMod.X1, oldMod.Y2)
                );
            }
        }
    }

    public static DungeonLevelMod? GetMod(string modName)
    {
        if (modName.Length > 0)
        {
            DungeonLevelMod dungeonLevelMod;
            if (DungeonLevelMods.TryGetValue(
                    modName,
                    out dungeonLevelMod
                ))
            {
                return dungeonLevelMod;
            }
        }

        return null;
    }

    public static void Load()
    {
        var options = JsonConfig.GetOptions(new TextDefinitionConverterFactory());

        var modRecords = JsonConfig.Deserialize<List<DungeonLevelMod>>(DataPath, options);

        if (modRecords == null)
        {
            throw new JsonException($"Failed to deserialize {DataPath}.");
        }
        foreach (var modRecord in modRecords)
        {
            modRecord.Duration += 1;
            AddMod(modRecord.Name, modRecord);
            modRecord.Tick();
        }
    }

    public static void Save()
    {
        NetState.FlushAll();
        var options = JsonConfig.GetOptions(new TextDefinitionConverterFactory());

        var modRecords = new List<DynamicJson>(DungeonLevelMods.Count);
        foreach (var (_, value) in DungeonLevelMods)
        {
            var dynamicJson = DynamicJson.Create(value.GetType());
            value.ToJson(dynamicJson, options);
            modRecords.Add(dynamicJson);
        }
        if (modRecords.Count == 0)
        {
            return;
        }
        JsonConfig.Serialize(DataPath, modRecords, options);
    }

    public static void SetDungeonDifficultyParameters(Point3D location, Map map, int level, DungeonLevelMod.DungeonDifficulty dungeonDifficulty, out string modName, out int x1, out int x2, out int y1, out int y2)
    {
        modName = GetModNameFromLocation(location, map, level);
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (modName)
        {
            case "Covetous_":
                GetCovetousLevelXY(level, out x1, out x2, out y1, out y2);
                break;
            case "Deceit_":
                GetDeceitLevelXY(level, out x1, out x2, out y1, out y2);
                break;
            case "BlightedGrove_":
                x1 = BlightedGroveX1;
                x2 = BlightedGroveX2;
                y1 = BlightedGroveY1;
                y2 = BlightedGroveY2;
                break;
            case "Despise_":
                GetDespiseLevelXY(level, out x1, out x2, out y1, out y2);
                break;
            case "Hythloth_":
                GetHythlothLevelXY(level, out x1, out x2, out y1, out y2);
                break;
            case "Shame_":
                GetShameLevelXY(level, out x1, out x2, out y1, out y2);
                break;
            case "Wrong_":
                GetWrongLevelXY(level, out x1, out x2, out y1, out y2);
                break;
            case "OrcCaves_":
                GetOrcCavesLevelXY(level, out x1, out x2, out y1, out y2);
                break;
            case "Ankh_":
                GetAnkhLevelXY(level, out x1, out x2, out y1, out y2);
                break;
            case "Rock_":
                GetRockLevelXY(level, out x1, out x2, out y1, out y2);
                break;
            case "Wisp_":
                GetWispLevelXY(level, out x1, out x2, out y1, out y2);
                break;
        }

        if (dungeonDifficulty is not DungeonLevelMod.DungeonDifficulty.Beginner)
        {
            switch (modName)
            {
                case "Destard_":
                    x1 = DestardLevelOneX1;
                    x2 = DestardLevelOneX2;
                    y1 = DestardLevelOneY1;
                    y2 = DestardLevelOneY2;
                    break;
                case "Exodus_":
                    x1 = ExodusLevelOneX1;
                    x2 = ExodusLevelOneX2;
                    y1 = ExodusLevelOneY1;
                    y2 = ExodusLevelOneY2;
                    break;
                case "Fire_":
                    x1 = FireLevelOneX1;
                    x2 = FireLevelOneX2;
                    y1 = FireLevelOneY1;
                    y2 = FireLevelOneY2;
                    break;
                case "TerathanKeep_":
                    GetIceDungeonLevelXY(level, out x1, out x2, out y1, out y2);
                    break;
                case "PaintedCaves_":
                    x1 = PaintedCavesX1;
                    x2 = PaintedCavesX2;
                    y1 = PaintedCavesY1;
                    y2 = PaintedCavesY2;
                    break;
                case "PalaceOfParoxysmus_":
                    x1 = PalaceOfParoxysmusX1;
                    x2 = PalaceOfParoxysmusX2;
                    y1 = PalaceOfParoxysmusY1;
                    y2 = PalaceOfParoxysmusY2;
                    break;
                case "Doom_":
                    GetDoomLevelXY(level, out x1, out x2, out y1, out y2);
                    break;
                case "Blood_":
                    x1 = BloodLevelOneX1;
                    x2 = BloodLevelOneX2;
                    y1 = BloodLevelOneY1;
                    y2 = BloodLevelOneY2;
                    break;
                case "Sorcerers_":
                    GetSorcerersLevelXY(level, out x1, out x2, out y1, out y2);
                    break;
                case "Spectre_":
                    x1 = SpectreLevelOneX1;
                    x2 = SpectreLevelOneX2;
                    y1 = SpectreLevelOneY1;
                    y2 = SpectreLevelOneY2;
                    break;
                case "Underworld_":
                    x1 = UnderworldX1;
                    x2 = UnderworldX2;
                    y1 = UnderworldY1;
                    y2 = UnderworldY2;
                    break;
                case "TombOfKings_":
                    x1 = TombOfKingsLevelOneX1;
                    x2 = TombOfKingsLevelOneX2;
                    y1 = TombOfKingsLevelOneY1;
                    y2 = TombOfKingsLevelOneY2;
                    break;
                case "StygianAbyss_":
                    x1 = StygianAbyssLevelOneX1;
                    x2 = StygianAbyssLevelOneX2;
                    y1 = StygianAbyssLevelOneY1;
                    y2 = StygianAbyssLevelOneY2;
                    break;
            }
            modName = $"{modName}_{level.ToString()}";
        }
    }
    public static bool HasDungeonDifficulty(string modName, Map map, DungeonLevelMod.DungeonDifficulty dungeonDifficulty)
    {
        DungeonLevelMod dungeonLevelMod = GetMod(modName);
        if (dungeonLevelMod is not null)
        {
            return dungeonLevelMod.Difficulty == dungeonDifficulty && map == dungeonLevelMod.LocationMap;
        }
        return false;
    }

    public static bool IsInDungeonDifficulty(Point3D location, Map map, DungeonLevelMod.DungeonDifficulty dungeonDifficulty, int minLevel, int maxLevel)
    {
        for (int level = minLevel; level < maxLevel; level++)
        {
            if (HasDungeonDifficulty(
                    $"{GetModNameFromLocation(location, map, level)}_{level.ToString()}",
                    map,
                    dungeonDifficulty
                ))
            {
                return true;
            }
        }
        return false;
    }
    public static bool IsInDungeonDifficulty(Point3D location, Map map, DungeonLevelMod.DungeonDifficulty dungeonDifficulty, int level) => HasDungeonDifficulty($"{GetModNameFromLocation(location, map, level)}_{level.ToString()}", map, dungeonDifficulty);

    public static void ResetDungeonSpawners(Map map, Point2D pointOne, Point2D pointTwo)
    {
        IPooledEnumerable<Item> items = map.GetItemsInBounds(
            new Rectangle2D(pointOne, pointTwo)
        );
        foreach (var item in items)
        {
            if (item is ISpawner spawner)
            {
                spawner.Respawn();
            }
        }
    }
    public static bool IsInShame(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInShameLevelOne(location, map),
            2 => IsInShameLevelTwo(location, map),
            3 => IsInShameLevelThree(location, map),
            4 => IsInShameLevelFour(location, map),
            _ => false
        };
    }

    public static void GetShameLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = ShameLevelOneX1;
                x2 = ShameLevelOneX2;
                y1 = ShameLevelOneY1;
                y2 = ShameLevelOneY2;
                break;
            case 2:
                x1 = ShameLevelTwoX1;
                x2 = ShameLevelTwoX2;
                y1 = ShameLevelTwoY1;
                y2 = ShameLevelTwoY2;
                break;
            case 3:
                x1 = ShameLevelThreeX1;
                x2 = ShameLevelThreeX2;
                y1 = ShameLevelThreeY1;
                y2 = ShameLevelThreeY2;
                break;
            case 4:
                x1 = ShameLevelFourX1;
                x2 = ShameLevelFourX2;
                y1 = ShameLevelFourY1;
                y2 = ShameLevelFourY2;
                break;
        }
    }

    public static bool IsInHythloth(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInHythlothLevelOne(location, map),
            2 => IsInHythlothLevelTwo(location, map),
            3 => IsInHythlothLevelThree(location, map),
            4 => IsInHythlothLevelFour(location, map),
            _ => false
        };
    }

    public static void GetHythlothLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = HythlothLevelOneX1;
                x2 = HythlothLevelOneX2;
                y1 = HythlothLevelOneY1;
                y2 = HythlothLevelOneY2;
                break;
            case 2:
                x1 = HythlothLevelTwoX1;
                x2 = HythlothLevelTwoX2;
                y1 = HythlothLevelTwoY1;
                y2 = HythlothLevelTwoY2;
                break;
            case 3:
                x1 = HythlothLevelThreeX1;
                x2 = HythlothLevelThreeX2;
                y1 = HythlothLevelThreeY1;
                y2 = HythlothLevelThreeY2;
                break;
            case 4:
                x1 = HythlothLevelFourX1;
                x2 = HythlothLevelFourX2;
                y1 = HythlothLevelFourY1;
                y2 = HythlothLevelFourY2;
                break;
        }
    }
    public static bool IsInDespise(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInDespiseLevelOne(location, map),
            2 => IsInDespiseLevelTwo(location, map),
            3 => IsInDespiseLevelThree(location, map),
            _ => false
        };
    }
    public static void GetDespiseLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = DespiseLevelOneX1;
                x2 = DespiseLevelOneX2;
                y1 = DespiseLevelOneY1;
                y2 = DespiseLevelOneY2;
                break;
            case 2:
                x1 = DespiseLevelTwoX1;
                x2 = DespiseLevelTwoX1;
                y1 = DespiseLevelTwoX1;
                y2 = DespiseLevelTwoX1;
                break;
            case 3:
                x1 = DespiseLevelThreeX1;
                x2 = DespiseLevelThreeX2;
                y1 = DespiseLevelThreeY1;
                y2 = DespiseLevelThreeY2;
                break;
        }
    }

    public static bool IsInDeceit(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInDeceitLevelOne(location, map),
            2 => IsInDeceitLevelTwo(location, map),
            3 => IsInDeceitLevelThree(location, map),
            4 => IsInDeceitLevelFour(location, map),
            _ => false
        };
    }
    public static void GetDeceitLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = DeceitLevelOneX1;
                x2 = DeceitLevelOneX2;
                y1 = DeceitLevelOneY1;
                y2 = DeceitLevelOneY2;
                break;
            case 2:
                x1 = DeceitLevelTwoX1;
                x2 = DeceitLevelTwoX1;
                y1 = DeceitLevelTwoX1;
                y2 = DeceitLevelTwoX1;
                break;
            case 3:
                x1 = DeceitLevelThreeX1;
                x2 = DeceitLevelThreeX2;
                y1 = DeceitLevelThreeY1;
                y2 = DeceitLevelThreeY2;
                break;
            case 4:
                x1 = DeceitLevelFourX1;
                x2 = DeceitLevelFourX2;
                y1 = DeceitLevelFourY1;
                y2 = DeceitLevelFourY2;
                break;
        }
    }
    public static bool IsInWrong(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInWrongLevelOne(location, map),
            2 => IsInWrongLevelTwo(location, map),
            3 => IsInWrongLevelThree(location, map),
            _ => false
        };
    }

    public static void GetWrongLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = WrongLevelOneX1;
                x2 = WrongLevelOneX2;
                y1 = WrongLevelOneY1;
                y2 = WrongLevelOneY2;
                break;
            case 2:
                x1 = WrongLevelTwoX1;
                x2 = WrongLevelTwoX2;
                y1 = WrongLevelTwoY1;
                y2 = WrongLevelTwoY2;
                break;
            case 3:
                x1 = WrongLevelThreeX1;
                x2 = WrongLevelThreeX2;
                y1 = WrongLevelThreeY1;
                y2 = WrongLevelThreeY2;
                break;
        }
    }

    public static bool IsInIceDungeon(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInIceLevelOne(location, map),
            2 => IsInIceLevelTwo(location, map),
            3 => IsInIceLevelThree(location, map),
            _ => false
        };
    }

    public static void GetIceDungeonLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = IceLevelOneX1;
                x2 = IceLevelOneX2;
                y1 = IceLevelOneY1;
                y2 = IceLevelOneY2;
                break;
            case 2:
                x1 = IceLevelTwoX1;
                x2 = IceLevelTwoX2;
                y1 = IceLevelTwoY1;
                y2 = IceLevelTwoY2;
                break;
            case 3:
                x1 = IceLevelThreeX1;
                x2 = IceLevelThreeX2;
                y1 = IceLevelThreeY1;
                y2 = IceLevelThreeY2;
                break;
        }
    }

    public static bool IsInOrcCaves(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInOrcCavesLevelOne(location, map),
            2 => IsInOrcCavesLevelTwo(location, map),
            3 => IsInOrcCavesLevelThree(location, map),
            _ => false
        };
    }

    public static void GetOrcCavesLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = OrcCavesLevelOneX1;
                x2 = OrcCavesLevelOneX2;
                y1 = OrcCavesLevelOneY1;
                y2 = OrcCavesLevelOneY2;
                break;
            case 2:
                x1 = OrcCavesLevelTwoX1;
                x2 = OrcCavesLevelTwoX2;
                y1 = OrcCavesLevelTwoY1;
                y2 = OrcCavesLevelTwoY2;
                break;
            case 3:
                x1 = OrcCavesLevelThreeX1;
                x2 = OrcCavesLevelThreeX2;
                y1 = OrcCavesLevelThreeY1;
                y2 = OrcCavesLevelThreeY2;
                break;
        }
    }
    public static bool IsInAnkh(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInAnkhLevelOne(location, map),
            2 => IsInAnkhLevelTwo(location, map),
            3 => IsInAnkhLevelThree(location, map),
            _ => false
        };
    }

    public static void GetAnkhLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = AnkhLevelOneX1;
                x2 = AnkhLevelOneX2;
                y1 = AnkhLevelOneY1;
                y2 = AnkhLevelOneY2;
                break;
            case 2:
                x1 = AnkhLevelTwoX1;
                x2 = AnkhLevelTwoX2;
                y1 = AnkhLevelTwoY1;
                y2 = AnkhLevelTwoY2;
                break;
            case 3:
                x1 = AnkhLevelThreeX1;
                x2 = AnkhLevelThreeX2;
                y1 = AnkhLevelThreeY1;
                y2 = AnkhLevelThreeY2;
                break;
        }
    }
    public static bool IsInRock(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInRockLevelOne(location, map),
            2 => IsInRockLevelTwo(location, map),
            _ => false
        };
    }

    public static void GetRockLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = RockLevelOneX1;
                x2 = RockLevelOneX2;
                y1 = RockLevelOneY1;
                y2 = RockLevelOneY2;
                break;
            case 2:
                x1 = RockLevelTwoX1;
                x2 = RockLevelTwoX2;
                y1 = RockLevelTwoY1;
                y2 = RockLevelTwoY2;
                break;
        }
    }
    public static bool IsInSorcerers(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInSorcerersLevelOne(location, map),
            2 => IsInSorcerersLevelTwo(location, map),
            4 => IsInSorcerersLevelFour(location, map),
            5 => IsInSorcerersLevelFive(location, map),
            _ => false
        };
    }

    public static void GetSorcerersLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = SorcerersLevelOneX1;
                x2 = SorcerersLevelOneX2;
                y1 = SorcerersLevelOneY1;
                y2 = SorcerersLevelOneY2;
                break;
            case 2:
                x1 = SorcerersLevelTwoX1;
                x2 = SorcerersLevelTwoX2;
                y1 = SorcerersLevelTwoY1;
                y2 = SorcerersLevelTwoY2;
                break;
            case 3:
                x1 = SorcerersLevelFourX1;
                x2 = SorcerersLevelFourX2;
                y1 = SorcerersLevelFourY1;
                y2 = SorcerersLevelFourY2;
                break;
            case 4:
                x1 = SorcerersLevelFiveX1;
                x2 = SorcerersLevelFiveX2;
                y1 = SorcerersLevelFiveY1;
                y2 = SorcerersLevelFiveY2;
                break;
        }
    }
    public static bool IsInWisp(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInWispLevelOne(location, map),
            2 => IsInWispLevelThree(location, map),
            3 => IsInWispLevelFive(location, map),
            4 => IsInWispLevelEight(location, map),
            _ => false
        };
    }

    public static void GetWispLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = WispLevelOneX1;
                x2 = WispLevelOneX2;
                y1 = WispLevelOneY1;
                y2 = WispLevelOneY2;
                break;
            case 2:
                x1 = WispLevelThreeX1;
                x2 = WispLevelThreeX2;
                y1 = WispLevelThreeY1;
                y2 = WispLevelThreeY2;
                break;
            case 3:
                x1 = WispLevelFiveX1;
                x2 = WispLevelFiveX2;
                y1 = WispLevelFiveY1;
                y2 = WispLevelFiveY2;
                break;
            case 4:
                x1 = WispLevelEightX1;
                x2 = WispLevelEightX2;
                y1 = WispLevelEightY1;
                y2 = WispLevelEightY2;
                break;
        }
    }
    public static bool IsInDoom(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInDoomLevelOne(location, map),
            2 => IsInDoomLevelTwo(location, map),
            _ => false
        };
    }

    public static void GetDoomLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = DoomLevelOneX1;
                x2 = DoomLevelOneX2;
                y1 = DoomLevelOneY1;
                y2 = DoomLevelOneY2;
                break;
            case 2:
                x1 = DoomLevelTwoX1;
                x2 = DoomLevelTwoX2;
                y1 = DoomLevelTwoY1;
                y2 = DoomLevelTwoY2;
                break;
        }
    }
    public static bool IsInCovetous(Point3D location, Map map, int level)
    {
        return level switch
        {
            1 => IsInCovetousLevelOne(location, map),
            2 => IsInCovetousLevelTwo(location, map),
            3 => IsInCovetousLevelThree(location, map),
            4 => IsInCovetousLevelFour(location, map),
            _ => false
        };
    }

    public static void GetCovetousLevelXY(int level, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;
        switch (level)
        {
            case 1:
                x1 = CovetousLevelOneX1;
                x2 = CovetousLevelOneX2;
                y1 = CovetousLevelOneY1;
                y2 = CovetousLevelOneY2;
                break;
            case 2:
                x1 = CovetousLevelTwoX1;
                x2 = CovetousLevelTwoX2;
                y1 = CovetousLevelTwoY1;
                y2 = CovetousLevelTwoY2;
                break;
            case 3:
                x1 = CovetousLevelThreeX1;
                x2 = CovetousLevelThreeX2;
                y1 = CovetousLevelThreeY1;
                y2 = CovetousLevelThreeY2;
                break;
            case 4:
                x1 = CovetousLevelFourX1;
                x2 = CovetousLevelFourX2;
                y1 = CovetousLevelFourY1;
                y2 = CovetousLevelFourY2;
                break;
        }
    }
    public static void SetDungeonDifficulty(
        Mobile player,
        DungeonLevelMod.DungeonDifficulty dungeonDifficulty,
        string modName,
        int x1,
        int x2,
        int y1,
        int y2
    )
    {
        DungeonLevelMod existingMod = GetMod(modName);
        Map map = player.Map;
        string message = DungeonDifficultyRejectedMessage;
        Type mapType = typeof(Map);
        PropertyInfo mapPropertyInfo = mapType.GetProperty(map.ToString());
        if (mapPropertyInfo is not null)
        {
            if (existingMod?.Difficulty == dungeonDifficulty && existingMod.LocationMap == map)
            {
                message = DungeonDifficultyAlreadySetMessage;
            } else if (existingMod is null || existingMod.Difficulty == DungeonLevelMod.DungeonDifficulty.Normal)
            {
                DungeonLevelMod newMod = new DungeonLevelMod
                {
                    Name = modName,
                    Difficulty = dungeonDifficulty,
                    Duration = 60,
                    LocationMap = map,
                    X1 = x1,
                    X2 = x2,
                    Y1 = y1,
                    Y2 = y2
                };
                AddMod(modName, newMod);
                Point2D pointOne = new Point2D(x2, y1);
                Point2D pointTwo = new Point2D(x1, y2);
                Timer.StartTimer(TimeSpan.FromMinutes(3), () => { ResetDungeonSpawners(map, pointOne, pointTwo); });
                Timer.StartTimer(TimeSpan.FromMinutes(3), newMod.Tick);
                NotifyPlayersInDungeon(map, pointOne, pointTwo, dungeonDifficulty);
                message = DungeonDifficultyChangedMessage;
            }
            else
            {
                message = DungeonDifficultyNotNormal;
            }
        }
        player.PrivateOverheadMessage(MessageType.Regular, player.SpeechHue, true, message, player.NetState);
    }

    public static void NotifyPlayersInDungeon(Map map, Point2D pointOne, Point2D pointTwo, DungeonLevelMod.DungeonDifficulty difficulty)
    {
        IPooledEnumerable<Mobile> mobiles = map.GetMobilesInBounds(
            new Rectangle2D(pointOne, pointTwo)
        );
        foreach (var mobile in mobiles)
        {
            if (mobile.Player)
            {
                mobile.PrivateOverheadMessage(MessageType.Regular, mobile.SpeechHue, true, $"This dungeon level will be changed to ${difficulty.ToString()} difficulty in 3 minutes.", mobile.NetState);
            }
        }
    }
    public static string GetModNameFromLocation(Point3D location, Map map, int level)
    {
        if (IsInCovetous(location, map, level))
        {
            return "Covetous_";
        }
        if (IsInDespise(location, map, level))
        {
            return "Despise_";
        }
        if (IsInBlightedGrove(location, map))
        {
            return "BlightedGrove_";
        }
        if (IsInDeceit(location, map, level))
        {
            return "Deceit_";
        }
        if (IsInHythloth(location, map, level))
        {
            return "Hythloth_";
        }
        if (IsInShame(location, map, level))
        {
            return "Shame_";
        }
        if (IsInWrong(location, map, level))
        {
            return "Wrong_";
        }
        if (IsInOrcCaves(location, map, level))
        {
            return "OrcCaves_";
        }
        if (IsInAnkh(location, map, level))
        {
            return "Ankh_";
        }
        if (IsInRock(location, map, level))
        {
            return "Rock_";
        }
        return IsInWisp(location, map, level) ? "Wisp_" : "";
    }

    public static bool IsInLevelOneDungeon(Point3D location, Map map) => IsInCovetousLevelOne(location, map)
                                                                         || IsInBlightedGrove(location, map)
                                                                         || IsInDeceitLevelOne(location, map)
                                                                         || IsInDespiseLevelOne(location, map)
                                                                         || IsInHythlothLevelOne(location, map)
                                                                         || IsInShameLevelOne(location, map)
                                                                         || IsInWrongLevelOne(location, map)
                                                                         || IsInOrcCavesLevelOne(location, map)
                                                                         || IsInAnkhLevelOne(location, map)
                                                                         || IsInRockLevelOne(location, map)
                                                                         || IsInWispLevelOne(location, map);
    public static bool IsInLevelTwoDungeon(Point3D location, Map map) => IsInCovetousLevelTwo(location, map)
                                                                         || IsInDeceitLevelTwo(location, map)
                                                                         || IsInDespiseLevelTwo(location, map)
                                                                         || IsInHythlothLevelTwo(location, map)
                                                                         || IsInShameLevelTwo(location, map)
                                                                         || IsInDestardLevelOne(location, map)
                                                                         || IsInWrongLevelTwo(location, map)
                                                                         || IsInFireLevelOne(location, map)
                                                                         || IsInIceLevelOne(location, map)
                                                                         || IsInOrcCavesLevelTwo(location, map)
                                                                         || IsInAnkhLevelTwo(location, map)
                                                                         || IsInSorcerersLevelOne(location, map)
                                                                         || IsInWispLevelThree(location, map);
    public static bool IsInLevelThreeDungeon(Point3D location, Map map) => IsInCovetousLevelThree(location, map)
                                                                           || IsInPaintedCaves(location, map)
                                                                           || IsInDeceitLevelThree(location, map)
                                                                           || IsInDespiseLevelThree(location, map)
                                                                           || IsInHythlothLevelThree(location, map)
                                                                           || IsInShameLevelThree(location, map)
                                                                           || IsInWrongLevelThree(location, map)
                                                                           || IsInFireLevelTwo(location, map)
                                                                           || IsInDestardLevelTwo(location, map)
                                                                           || IsInIceLevelTwo(location, map)
                                                                           || IsInOrcCavesLevelThree(location, map)
                                                                           || IsInDoomLevelOne(location, map)
                                                                           || IsInAnkhLevelThree(location, map)
                                                                           || IsInExodusLevelOne(location, map)
                                                                           || IsInRockLevelTwo(location, map)
                                                                           || IsInSorcerersLevelTwo(location, map)
                                                                           || IsInSpectreLevelOne(location, map)
                                                                           || IsInWispLevelFive(location, map)
                                                                           || IsInUnderworld(location, map)
                                                                           || IsInTombOfKings(location, map);
    public static bool IsInLevelFourDungeon(Point3D location, Map map) => IsInCovetousLevelFour(location, map)
                                                                          || IsInDeceitLevelFour(location, map)
                                                                          || IsInDestardLevelThree(location, map)
                                                                          || IsInHythlothLevelFour(location, map)
                                                                          || IsInShameLevelFour(location, map)
                                                                          || IsInTerathanKeepLevelOne(location, map)
                                                                          || IsInIceLevelThree(location, map)
                                                                          || IsInPalaceOfParoxysmus(location, map)
                                                                          || IsInDoomLevelTwo(location, map)
                                                                          || IsInBloodLevelOne(location, map)
                                                                          || IsInSorcerersLevelFour(location, map)
                                                                          || IsInStygianAbyss(location, map);

    public static bool IsInLevelFiveDungeon(Point3D location, Map map) => IsInSorcerersLevelFive(location, map)
                                                                          || IsInWispLevelEight(location, map);

    public const int CovetousLevelOneX1 = 5505;
    public const int CovetousLevelOneY1 = 1838;
    public const int CovetousLevelOneX2 = 5386;
    public const int CovetousLevelOneY2 = 1953;
    public static bool IsInCovetousLevelOne(Point3D location, Map map) => location.X is < CovetousLevelOneX1 and > CovetousLevelOneX2 &&
                                                                          location.Y is < CovetousLevelOneY2 and > CovetousLevelOneY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int CovetousLevelTwoX1 = 5614;
    public const int CovetousLevelTwoY1 = 1997;
    public const int CovetousLevelTwoX2 = 5385;
    public const int CovetousLevelTwoY2 = 2040;
    public static bool IsInCovetousLevelTwo(Point3D location, Map map) => location.X is < CovetousLevelTwoX1 and > CovetousLevelTwoX2 &&
                                                                          location.Y is < CovetousLevelTwoY2 and > CovetousLevelTwoY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int CovetousLevelThreeX1 = 5620;
    public const int CovetousLevelThreeY1 = 1810;
    public const int CovetousLevelThreeX2 = 5531;
    public const int CovetousLevelThreeY2 = 1936;
    public static bool IsInCovetousLevelThree(Point3D location, Map map) => location.X is < CovetousLevelThreeX1 and > CovetousLevelThreeX2 &&
                                                                            location.Y is < CovetousLevelThreeY2 and > CovetousLevelThreeY1 && (map == Map.Trammel || map == Map.Felucca);

    public const int CovetousLevelFourX1 = 5487;
    public const int CovetousLevelFourY1 = 1784;
    public const int CovetousLevelFourX2 = 5393;
    public const int CovetousLevelFourY2 = 1836;
    public static bool IsInCovetousLevelFour(Point3D location, Map map) => location.X is < CovetousLevelFourX1 and > CovetousLevelFourX2 &&
                                                                           location.Y is < CovetousLevelFourY2 and > CovetousLevelFourY1 && (map == Map.Trammel || map == Map.Felucca);

    public const int BlightedGroveX1 = 6600;
    public const int BlightedGroveY1 = 810;
    public const int BlightedGroveX2 = 6438;
    public const int BlightedGroveY2 = 978;
    public static bool IsInBlightedGrove(Point3D location, Map map) => location.X is < BlightedGroveX1 and > BlightedGroveX2 &&
                                                                       location.Y is < BlightedGroveY2 and > BlightedGroveY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int DeceitLevelOneX1 = 5231;
    public const int DeceitLevelOneY1 = 530;
    public const int DeceitLevelOneX2 = 5127;
    public const int DeceitLevelOneY2 = 635;
    public static bool IsInDeceitLevelOne(Point3D location, Map map) => location.X is < DeceitLevelOneX1 and > DeceitLevelOneX2 &&
                                                                        location.Y is < DeceitLevelOneY2 and > DeceitLevelOneY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int DeceitLevelTwoX1 = 5351;
    public const int DeceitLevelTwoY1 = 529;
    public const int DeceitLevelTwoX2 = 5271;
    public const int DeceitLevelTwoY2 = 638;
    public static bool IsInDeceitLevelTwo(Point3D location, Map map) => location.X is < DeceitLevelTwoX1 and > DeceitLevelTwoX2 &&
                                                                        location.Y is < DeceitLevelTwoY2 and > DeceitLevelTwoY1 && (map == Map.Trammel || map == Map.Felucca);

    public const int DeceitLevelThreeX1 = 5235;
    public const int DeceitLevelThreeY1 = 638;
    public const int DeceitLevelThreeX2 = 5214;
    public const int DeceitLevelThreeY2 = 768;
    public static bool IsInDeceitLevelThree(Point3D location, Map map) => location.X is < DeceitLevelThreeX1 and > DeceitLevelThreeX2 &&
                                                                          location.Y is < DeceitLevelThreeY2 and > DeceitLevelThreeY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int DeceitLevelFourX1 = 5353;
    public const int DeceitLevelFourY1 = 644;
    public const int DeceitLevelFourX2 = 5251;
    public const int DeceitLevelFourY2 = 766;

    public static bool IsInDeceitLevelFour(Point3D location, Map map) => location.X is < DeceitLevelFourX1 and > DeceitLevelFourX2 &&
                                                                         location.Y is < DeceitLevelFourY2 and > DeceitLevelFourY1 && (map == Map.Trammel || map == Map.Felucca);

    public const int DespiseLevelOneX1 = 5524;
    public const int DespiseLevelOneY1 = 512;
    public const int DespiseLevelOneX2 = 5370;
    public const int DespiseLevelOneY2 = 642;
    public static bool IsInDespiseLevelOne(Point3D location, Map map) => location.X is < DespiseLevelOneX1 and > DespiseLevelOneX2 &&
                                                                         location.Y is < DespiseLevelOneY2 and > DespiseLevelOneY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int DespiseLevelTwoX1 = 5523;
    public const int DespiseLevelTwoY1 = 637;
    public const int DespiseLevelTwoX2 = 5373;
    public const int DespiseLevelTwoY2 = 770;
    public static bool IsInDespiseLevelTwo(Point3D location, Map map) => location.X is < DespiseLevelTwoX1 and > DespiseLevelTwoX2 &&
                                                                         location.Y is < DespiseLevelTwoY2 and > DespiseLevelTwoY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int DespiseLevelThreeX1 = 5630;
    public const int DespiseLevelThreeY1 = 762;
    public const int DespiseLevelThreeX2 = 5364;
    public const int DespiseLevelThreeY2 = 1019;
    public static bool IsInDespiseLevelThree(Point3D location, Map map) => location.X is < DespiseLevelThreeX1 and > DespiseLevelThreeX2 &&
                                                                           location.Y is < DespiseLevelThreeY2 and > DespiseLevelThreeY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int DestardLevelOneX1 = 5372;
    public const int DestardLevelOneY1 = 761;
    public const int DestardLevelOneX2 = 5122;
    public const int DestardLevelOneY2 = 1027;

    public static bool IsInDestard(Point3D location, Map map) => location.X is < DestardLevelOneX1 and > DestardLevelOneX2 &&
                                                                 location.Y is < DestardLevelOneY2 and > DestardLevelOneY1 &&
                                                                 (map == Map.Trammel || map == Map.Felucca);
    public static bool IsInDestardLevelOne(Point3D location, Map map) => location.X is < DestardLevelOneX1 and > DestardLevelOneX2 &&
                                                                         location.Y is < DestardLevelOneY2 and > DestardLevelOneY1 &&
                                                                         !IsInDestardLevelTwo(location,map) && !IsInDestardLevelThree(location, map) &&
                                                                         (map == Map.Trammel || map == Map.Felucca);
    public const int DestardLevelTwoX1 = 5183;
    public const int DestardLevelTwoY1 = 780;
    public const int DestardLevelTwoX2 = 5120;
    public const int DestardLevelTwoY2 = 890;
    public static bool IsInDestardLevelTwo(Point3D location, Map map) => location.X is < DestardLevelTwoX1 and > DestardLevelTwoX2 &&
                                                                         location.Y is < DestardLevelTwoY2 and > DestardLevelTwoY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int DestardLevelThreeX1 = 5154;
    public const int DestardLevelThreeY1 = 954;
    public const int DestardLevelThreeX2 = 5127;
    public const int DestardLevelThreeY2 = 1008;
    public const int DestardLevelThreeX1_2 = 5195;
    public const int DestardLevelThreeY1_2 = 984;
    public const int DestardLevelThreeX2_2 = 5147;
    public const int DestardLevelThreeY2_2 = 1021;
    public static bool IsInDestardLevelThree(Point3D location, Map map) => (location.X is < DestardLevelThreeX1 and > DestardLevelThreeX2 &&
                                                                            location.Y is < DestardLevelThreeY2 and > DestardLevelThreeY1 ||
                                                                            location.X is < DestardLevelThreeX1_2 and > DestardLevelThreeX2_2 &&
                                                                            location.Y is < DestardLevelThreeY2_2 and > DestardLevelThreeY1_2 )&& (map == Map.Trammel || map == Map.Felucca);
    public const int HythlothLevelOneX1 = 5998;
    public const int HythlothLevelOneY1 = 17;
    public const int HythlothLevelOneX2 = 5898;
    public const int HythlothLevelOneY2 = 121;
    public static bool IsInHythlothLevelOne(Point3D location, Map map) => location.X is < HythlothLevelOneX1 and > HythlothLevelOneX2 &&
                                                                          location.Y is < HythlothLevelOneY2 and > HythlothLevelOneY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int HythlothLevelTwoX1 = 6004;
    public const int HythlothLevelTwoY1 = 136;
    public const int HythlothLevelTwoX2 = 5906;
    public const int HythlothLevelTwoY2 = 245;
    public static bool IsInHythlothLevelTwo(Point3D location, Map map) => location.X is < HythlothLevelTwoX1 and > HythlothLevelTwoX2 &&
                                                                          location.Y is < HythlothLevelTwoY2 and > HythlothLevelTwoY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int HythlothLevelThreeX1 = 6131;
    public const int HythlothLevelThreeY1 = 140;
    public const int HythlothLevelThreeX2 = 6020;
    public const int HythlothLevelThreeY2 = 241;
    public static bool IsInHythlothLevelThree(Point3D location, Map map) => location.X is < HythlothLevelThreeX1 and > HythlothLevelThreeX2 &&
                                                                            location.Y is < HythlothLevelThreeY2 and > HythlothLevelThreeY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int HythlothLevelFourX1 = 6120;
    public const int HythlothLevelFourY1 = 24;
    public const int HythlothLevelFourX2 = 6046;
    public const int HythlothLevelFourY2 = 110;

    public static bool IsInHythlothLevelFour(Point3D location, Map map) => location.X is < HythlothLevelFourX1 and > HythlothLevelFourX2 &&
                                                                           location.Y is < HythlothLevelFourY2 and > HythlothLevelFourY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int ShameLevelOneX1 = 5503;
    public const int ShameLevelOneY1 = 0;
    public const int ShameLevelOneX2 = 5374;
    public const int ShameLevelOneY2 = 135;
    public static bool IsInShameLevelOne(Point3D location, Map map) => location.X is < ShameLevelOneX1 and > ShameLevelOneX2 &&
                                                                       location.Y is < ShameLevelOneY2 and > ShameLevelOneY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int ShameLevelTwoX1 = 5632;
    public const int ShameLevelTwoY1 = 0;
    public const int ShameLevelTwoX2 = 5507;
    public const int ShameLevelTwoY2 = 124;
    public static bool IsInShameLevelTwo(Point3D location, Map map) => location.X is < ShameLevelTwoX1 and > ShameLevelTwoX2 &&
                                                                       location.Y is < ShameLevelTwoY2 and > ShameLevelTwoY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int ShameLevelThreeX1 = 5630;
    public const int ShameLevelThreeY1 = 130;
    public const int ShameLevelThreeX2 = 5375;
    public const int ShameLevelThreeY2 = 257;
    public static bool IsInShameLevelThree(Point3D location, Map map) => location.X is < ShameLevelThreeX1 and > ShameLevelThreeX2 &&
                                                                         location.Y is < ShameLevelThreeY2 and > ShameLevelThreeY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int ShameLevelFourX1 = 5892;
    public const int ShameLevelFourY1 = 12;
    public const int ShameLevelFourX2 = 5629;
    public const int ShameLevelFourY2 = 125;
    public static bool IsInShameLevelFour(Point3D location, Map map) => location.X is < ShameLevelFourX1 and > ShameLevelFourX2 &&
                                                                        location.Y is < ShameLevelFourY2 and > ShameLevelFourY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int WrongLevelOneX1 = 5888;
    public const int WrongLevelOneY1 = 509;
    public const int WrongLevelOneX2 = 5771;
    public const int WrongLevelOneY2 = 635;
    public static bool IsInWrongLevelOne(Point3D location, Map map) => location.X is < WrongLevelOneX1 and > WrongLevelOneX2 &&
                                                                       location.Y is < WrongLevelOneY2 and > WrongLevelOneY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int WrongLevelTwoX1 = 5749;
    public const int WrongLevelTwoY1 = 505;
    public const int WrongLevelTwoX2 = 5624;
    public const int WrongLevelTwoY2 = 596;
    public static bool IsInWrongLevelTwo(Point3D location, Map map) => location.X is < WrongLevelTwoX1 and > WrongLevelTwoX2 &&
                                                                       location.Y is < WrongLevelTwoY2 and > WrongLevelTwoY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int WrongLevelThreeX1 = 5730;
    public const int WrongLevelThreeY1 = 616;
    public const int WrongLevelThreeX2 = 5683;
    public const int WrongLevelThreeY2 = 673;
    public static bool IsInWrongLevelThree(Point3D location, Map map) => location.X is < WrongLevelThreeX1 and > WrongLevelThreeX2 &&
                                                                         location.Y is < WrongLevelThreeY2 and > WrongLevelThreeY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int TerathanKeepLevelOneX1 = 5639;
    public const int TerathanKeepLevelOneY1 = 1528;
    public const int TerathanKeepLevelOneX2 = 5126;
    public const int TerathanKeepLevelOneY2 = 1800;
    public static bool IsInTerathanKeepLevelOne(Point3D location, Map map) => location.X is < TerathanKeepLevelOneX1 and > TerathanKeepLevelOneX2 &&
                                                                              location.Y is < TerathanKeepLevelOneY2 and > TerathanKeepLevelOneY1 && (map == Map.Trammel || map == Map.Felucca);
    public static bool IsInFireDungeon(Point3D location, Map map) => location.X is < FireLevelOneX1 and FireLevelOneX2 &&
                                                                     location.Y is < FireLevelOneY2 and > FireLevelOneY1
                                                                     && (map == Map.Trammel || map == Map.Felucca);
    public const int FireLevelOneX1 = 5887;
    public const int FireLevelOneY1 = 1285;
    public const int FireLevelOneX2 = 5615;
    public const int FireLevelOneY2 = 1515;
    public static bool IsInFireLevelOne(Point3D location, Map map) => location.X is < FireLevelOneX1 and FireLevelOneX2 &&
                                                                      location.Y is < FireLevelOneY2 and > FireLevelOneY1 &&
                                                                      !IsInFireLevelTwo(location, map) && (map == Map.Trammel || map == Map.Felucca);
    public const int FireLevelTwoX1 = 5760;
    public const int FireLevelTwoY1 = 1286;
    public const int FireLevelTwoX2 = 5624;
    public const int FireLevelTwoY2 = 1403;
    public const int FireLevelTwoX1_2 = 5662;
    public const int FireLevelTwoY1_2 = 1394;
    public const int FireLevelTwoX2_2 = 5628;
    public const int FireLevelTwoY2_2 = 1462;
    public static bool IsInFireLevelTwo(Point3D location, Map map) => (location.X is < FireLevelTwoX1 and > FireLevelTwoX2 &&
                                                                       location.Y is < FireLevelTwoY2 and > FireLevelTwoY1 ||
                                                                       location.X is < FireLevelTwoX1_2 and > FireLevelTwoX2_2 &&
                                                                       location.Y is < FireLevelTwoY2_2 and > FireLevelTwoY1_2) && (map == Map.Trammel || map == Map.Felucca);
    public const int IceLevelOneX1 = 5897;
    public const int IceLevelOneY1 = 131;
    public const int IceLevelOneX2 = 5659;
    public const int IceLevelOneY2 = 274;
    public static bool IsInIceLevelOne(Point3D location, Map map) => location.X is < IceLevelOneX1 and > IceLevelOneX2 &&
                                                                     location.Y is < IceLevelOneY2 and > IceLevelOneY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int IceLevelTwoX1 = 5867;
    public const int IceLevelTwoY1 = 312;
    public const int IceLevelTwoX2 = 5794;
    public const int IceLevelTwoY2 = 391;
    public static bool IsInIceLevelTwo(Point3D location, Map map) => location.X is < IceLevelTwoX1 and > IceLevelTwoX2 &&
                                                                     location.Y is < IceLevelTwoY2 and > IceLevelTwoY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int IceLevelThreeX1 = 5713;
    public const int IceLevelThreeY1 = 291;
    public const int IceLevelThreeX2 = 5650;
    public const int IceLevelThreeY2 = 343;
    public static bool IsInIceLevelThree(Point3D location, Map map) => location.X is < IceLevelThreeX1 and > IceLevelThreeX2 &&
                                                                       location.Y is < IceLevelThreeY2 and > IceLevelThreeY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int OrcCavesLevelOneX1 = 5165;
    public const int OrcCavesLevelOneY1 = 1949;
    public const int OrcCavesLevelOneX2 = 5127;
    public const int OrcCavesLevelOneY2 = 2026;
    public static bool IsInOrcCavesLevelOne(Point3D location, Map map) => location.X is < OrcCavesLevelOneX1 and > OrcCavesLevelOneX2 &&
                                                                          location.Y is < OrcCavesLevelOneY2 and > OrcCavesLevelOneY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int OrcCavesLevelTwoX1 = 5378;
    public const int OrcCavesLevelTwoY1 = 1275;
    public const int OrcCavesLevelTwoX2 = 5284;
    public const int OrcCavesLevelTwoY2 = 1391;
    public static bool IsInOrcCavesLevelTwo(Point3D location, Map map) => location.X is < OrcCavesLevelTwoX1 and > OrcCavesLevelTwoX2 &&
                                                                          location.Y is < OrcCavesLevelTwoY2 and > OrcCavesLevelTwoY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int OrcCavesLevelThreeX1 = 5366;
    public const int OrcCavesLevelThreeY1 = 1950;
    public const int OrcCavesLevelThreeX2 = 5258;
    public const int OrcCavesLevelThreeY2 = 2044;
    public static bool IsInOrcCavesLevelThree(Point3D location, Map map) => location.X is < OrcCavesLevelThreeX1 and > OrcCavesLevelThreeX2 &&
                                                                            location.Y is < OrcCavesLevelThreeY2 and > OrcCavesLevelThreeY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int PaintedCavesX1 = 6321;
    public const int PaintedCavesY1 = 856;
    public const int PaintedCavesX2 = 6241;
    public const int PaintedCavesY2 = 927;
    public static bool IsInPaintedCaves(Point3D location, Map map) => location.X is < PaintedCavesX1 and > PaintedCavesX2 &&
                                                                      location.Y is < PaintedCavesY2 and > PaintedCavesY1 && (map == Map.Trammel || map == Map.Felucca);
    public const int PalaceOfParoxysmusX1 = 6577;
    public const int PalaceOfParoxysmusY1 = 304;
    public const int PalaceOfParoxysmusX2 = 6198;
    public const int PalaceOfParoxysmusY2 = 675;
    public static bool IsInPalaceOfParoxysmus(Point3D location, Map map) => location.X is < PalaceOfParoxysmusX1 and > PalaceOfParoxysmusX2 &&
                                                                            location.Y is < PalaceOfParoxysmusY2 and PalaceOfParoxysmusY1 && (map == Map.Trammel || map == Map.Felucca);

    public const int DoomLevelOneX1 = 509;
    public const int DoomLevelOneY1 = 0;
    public const int DoomLevelOneX2 = 250;
    public const int DoomLevelOneY2 = 255;

    public static bool IsInDoomLevelOne(Point3D location, Map map) => location.X is < DoomLevelOneX1 and > DoomLevelOneX2 &&
                                                                      location.Y is < DoomLevelOneY2 and > DoomLevelOneY1 && map == Map.Malas;
    public const int DoomLevelTwoX1 = 510;
    public const int DoomLevelTwoY1 = 295;
    public const int DoomLevelTwoX2 = 321;
    public const int DoomLevelTwoY2 = 554;
    public static bool IsInDoomLevelTwo(Point3D location, Map map) => location.X is < DoomLevelTwoX1 and > DoomLevelTwoX2 &&
                                                                      location.Y is < DoomLevelTwoY2 and > DoomLevelTwoY1 && map == Map.Malas;
    public const int AnkhLevelOneX1 = 178;
    public const int AnkhLevelOneY1 = 1226;
    public const int AnkhLevelOneX2 = 0;
    public const int AnkhLevelOneY2 = 1551;
    public static bool IsInAnkhLevelOne(Point3D location, Map map) => location.X is < AnkhLevelOneX1 and > AnkhLevelOneX2 &&
                                                                      location.Y is < AnkhLevelOneY2 and > AnkhLevelOneY1 && map == Map.Ilshenar;
    public const int AnkhLevelTwoX1 = 196;
    public const int AnkhLevelTwoY1 = 799;
    public const int AnkhLevelTwoX2 = 0;
    public const int AnkhLevelTwoY2 = 1225;
    public static bool IsInAnkhLevelTwo(Point3D location, Map map) => location.X is < AnkhLevelTwoX1 and > AnkhLevelTwoX2 &&
                                                                      location.Y is < AnkhLevelTwoY2 and > AnkhLevelTwoY1 && map == Map.Ilshenar;
    public const int AnkhLevelThreeX1 = 555;
    public const int AnkhLevelThreeY1 = 1475;
    public const int AnkhLevelThreeX2 = 378;
    public const int AnkhLevelThreeY2 = 1598;
    public static bool IsInAnkhLevelThree(Point3D location, Map map) => location.X is < AnkhLevelThreeX1 and > AnkhLevelThreeX2 &&
                                                                        location.Y is < AnkhLevelThreeY2 and > AnkhLevelThreeY1 && map == Map.Ilshenar;
    public const int BloodLevelOneX1 = 2201;
    public const int BloodLevelOneY1 = 814;
    public const int BloodLevelOneX2 = 2048;
    public const int BloodLevelOneY2 = 1062;
    public static bool IsInBloodLevelOne(Point3D location, Map map) => location.X is < BloodLevelOneX1 and > BloodLevelOneX2 &&
                                                                       location.Y is < BloodLevelOneY2 and > BloodLevelOneY1 && map == Map.Ilshenar;
    public const int ExodusLevelOneX1 = 2081;
    public const int ExodusLevelOneY1 = 27;
    public const int ExodusLevelOneX2 = 1830;
    public const int ExodusLevelOneY2 = 212;
    public static bool IsInExodusLevelOne(Point3D location, Map map) => location.X is < ExodusLevelOneX1 and > ExodusLevelOneX2 &&
                                                                        location.Y is < ExodusLevelOneY2 and > ExodusLevelOneY1 && map == Map.Ilshenar;
    public const int RockLevelOneX1 = 2194;
    public const int RockLevelOneY1 = 289;
    public const int RockLevelOneX2 = 2161;
    public const int RockLevelOneY2 = 341;
    public static bool IsInRockLevelOne(Point3D location, Map map) => location.X is < RockLevelOneX1 and > RockLevelOneX2 &&
                                                                      location.Y is < RockLevelOneY2 and > RockLevelOneY1 && map == Map.Ilshenar;
    public const int RockLevelTwoX1 = 2242;
    public const int RockLevelTwoY1 = 5;
    public const int RockLevelTwoX2 = 2086;
    public const int RockLevelTwoY2 = 187;
    public static bool IsInRockLevelTwo(Point3D location, Map map) => location.X is < RockLevelTwoX1 and > RockLevelTwoX2 &&
                                                                      location.Y is < RockLevelTwoY2 and > RockLevelTwoY1 && map == Map.Ilshenar;
    public const int SorcerersLevelOneX1 = 476;
    public const int SorcerersLevelOneY1 = 0;
    public const int SorcerersLevelOneX2 = 365;
    public const int SorcerersLevelOneY2 = 124;
    public static bool IsInSorcerersLevelOne(Point3D location, Map map) => location.X is < SorcerersLevelOneX1 and > SorcerersLevelOneX2 &&
                                                                           location.Y is < SorcerersLevelOneY2 and > SorcerersLevelOneY1 && map == Map.Ilshenar;
    public const int SorcerersLevelTwoX1 = 362;
    public const int SorcerersLevelTwoY1 = 19;
    public const int SorcerersLevelTwoX2 = 197;
    public const int SorcerersLevelTwoY2 = 111;
    public static bool IsInSorcerersLevelTwo(Point3D location, Map map) => location.X is < SorcerersLevelTwoX1 and > SorcerersLevelTwoX2 &&
                                                                           location.Y is < SorcerersLevelTwoY2 and > SorcerersLevelTwoY1 && map == Map.Ilshenar;
    public const int SorcerersLevelFourX1 = 179;
    public const int SorcerersLevelFourY1 = 0;
    public const int SorcerersLevelFourX2 = 54;
    public const int SorcerersLevelFourY2 = 139;

    public static bool IsInSorcerersLevelFour(Point3D location, Map map) => location.X is < SorcerersLevelFourX1 and SorcerersLevelFourX2 &&
                                                                            location.Y is < SorcerersLevelFourY2 and > SorcerersLevelFourY1 &&
                                                                            map == Map.Ilshenar;
    public const int SorcerersLevelFiveX1 = 252;
    public const int SorcerersLevelFiveY1 = 109;
    public const int SorcerersLevelFiveX2 = 222;
    public const int SorcerersLevelFiveY2 = 148;

    public static bool IsInSorcerersLevelFive(Point3D location, Map map) => location.X is < SorcerersLevelFiveX1 and > SorcerersLevelFiveX2 &&
                                                                            location.Y is < SorcerersLevelFiveY2 and > SorcerersLevelFiveY1 &&
                                                                            map == Map.Ilshenar;
    public const int SpectreLevelOneX1 = 2022;
    public const int SpectreLevelOneY1 = 1007;
    public const int SpectreLevelOneX2 = 1938;
    public const int SpectreLevelOneY2 = 1112;
    public static bool IsInSpectreLevelOne(Point3D location, Map map) => location.X is < SpectreLevelOneX1 and > SpectreLevelOneX2 &&
                                                                         location.Y is < SpectreLevelOneY2 and > SpectreLevelOneY1 &&
                                                                         map == Map.Ilshenar;
    public const int WispLevelOneX1 = 738;
    public const int WispLevelOneY1 = 1477;
    public const int WispLevelOneX2 = 622;
    public const int WispLevelOneY2 = 1576;
    public static bool IsInWispLevelOne(Point3D location, Map map) => location.X is < WispLevelOneX1 and > WispLevelOneX2 &&
                                                                      location.Y is < WispLevelOneY2 and > WispLevelOneY1 &&
                                                                      map == Map.Ilshenar;
    public const int WispLevelThreeX1 = 917;
    public const int WispLevelThreeY1 = 1438;
    public const int WispLevelThreeX2 = 811;
    public const int WispLevelThreeY2 = 1600;
    public static bool IsInWispLevelThree(Point3D location, Map map) => location.X is < WispLevelThreeX1 and > WispLevelThreeX2 &&
                                                                        location.Y is < WispLevelThreeY2 and > WispLevelThreeY1 &&
                                                                        map == Map.Ilshenar;
    public const int WispLevelFiveX1 = 1024;
    public const int WispLevelFiveY1 = 1447;
    public const int WispLevelFiveX2 = 915;
    public const int WispLevelFiveY2 = 1588;
    public static bool IsInWispLevelFive(Point3D location, Map map) => location.X is < WispLevelFiveX1 and > WispLevelFiveX2 &&
                                                                       location.Y is < WispLevelFiveY2 and > WispLevelFiveY1 &&
                                                                       map == Map.Ilshenar;
    public const int WispLevelEightX1 = 796;
    public const int WispLevelEightY1 = 1458;
    public const int WispLevelEightX2 = 749;
    public const int WispLevelEightY2 = 1508;

    public static bool IsInWispLevelEight(Point3D location, Map map) => location.X is < WispLevelEightX1 and > WispLevelEightX2 &&
                                                                        location.Y is < WispLevelEightY2 and > WispLevelEightY1 &&
                                                                        map == Map.Ilshenar;
    public const int UnderworldX1 = 1252;
    public const int UnderworldY1 = 949;
    public const int UnderworldX2 = 993;
    public const int UnderworldY2 = 1223;
    public static bool IsInUnderworld(Point3D location, Map map) => location.X is < UnderworldX1 and > UnderworldX2 &&
                                                                    location.Y is < UnderworldY2 and > UnderworldY1 &&
                                                                    map == Map.TerMur;
    public const int TombOfKingsLevelOneX1 = 68;
    public const int TombOfKingsLevelOneY1 = 11;
    public const int TombOfKingsLevelOneX2 = 5;
    public const int TombOfKingsLevelOneY2 = 252;
    public static bool IsInTombOfKings(Point3D location, Map map) => location.X is < TombOfKingsLevelOneX1 and > TombOfKingsLevelOneX2 &&
                                                                     location.Y is < TombOfKingsLevelOneY2 and > TombOfKingsLevelOneY1 &&
                                                                     map == Map.TerMur;
    public const int StygianAbyssLevelOneX1 = 1137;
    public const int StygianAbyssLevelOneY1 = 17;
    public const int StygianAbyssLevelOneX2 = 301;
    public const int StygianAbyssLevelOneY2 = 1082;
    // public const int StygianAbyssLevelOneX1 = 1137;
    // public const int StygianAbyssLevelOneY1 = 17;
    // public const int StygianAbyssLevelOneX2 = 454;
    // public const int StygianAbyssLevelOneY2 = 804;
    public static bool IsInStygianAbyss(Point3D location, Map map) => (location.X is < StygianAbyssLevelOneX1 and > StygianAbyssLevelOneX2 &&
                                                                       location.Y is < StygianAbyssLevelOneY2 and > StygianAbyssLevelOneY1
                                                                          // ||
                                                                          // location.X is < 747 and > 301 &&
                                                                          // location.Y is < 1082 and > 336
                                                                      ) &&
                                                                      map == Map.TerMur;
}
