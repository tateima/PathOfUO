using System;
using System.Text.Json;
using Server.Json;

namespace Server.Dungeon;

public class DungeonLevelMod
{
    public enum DungeonDifficulty
    {
        Beginner,
        Normal,
        Hard,
        Epic
    }

    public DungeonDifficulty Difficulty { get; set; }
    public Map LocationMap { get; set; }
    public string Name { get; set; }

    public int Duration { get; set; }

    private TimerExecutionToken _timerToken;

    public int X1 { get; set; }
    public int X2 { get; set; }
    public int Y1 { get; set; }
    public int Y2 { get; set; }

    public DungeonLevelMod()
    {
    }

    public virtual void ToJson(DynamicJson json, JsonSerializerOptions options)
    {
        json.Type = GetType().Name;
        json.SetProperty("Name", options, Name);
        json.SetProperty("Duration", options, Duration);
        json.SetProperty("Difficulty", options, Difficulty.ToString());
        json.SetProperty("LocationMap", options, LocationMap.ToString());
        json.SetProperty("X1", options, X1);
        json.SetProperty("X2", options, X2);
        json.SetProperty("Y1", options, Y1);
        json.SetProperty("Y2", options, Y2);
    }

    public void Tick()
    {
        Duration--;
        if (Duration <= 0)
        {
            _timerToken.Cancel();
            DungeonLevelModHandler.RemoveMod(Name);
        }
        else
        {
            Timer.StartTimer(TimeSpan.FromMinutes(1), Tick, out _timerToken);
        }
    }
}
