using System;
using System.Text.Json;
using System.Text.Json.Serialization;

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
