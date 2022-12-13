using System;
using Server.Dungeon;
using Server.Mobiles;

namespace Server.ContextMenus
{
    public class SetBeginnerDungeonEntry : ContextMenuEntry
    {
        private readonly PlayerMobile m_From;

        public SetBeginnerDungeonEntry(PlayerMobile from) : base(1116799, 1)
        {
            m_From = from;
        }

        public override void OnClick()
        {
            Point3D location = m_From.Location;
            Map map = m_From.Map;
            int level = 1;
            DungeonLevelModHandler.SetDungeonDifficultyParameters(
                location,
                map,
                level,
                DungeonLevelMod.DungeonDifficulty.Beginner,
                out string modName,
                out int x1,
                out int x2,
                out int y1,
                out int y2
            );
            if (!(x1 == 0 && x2 == 0 && y1 == 0 && y2 == 0) && modName.Length > 0)
            {
                DungeonLevelModHandler.SetDungeonDifficulty(
                    m_From,
                    DungeonLevelMod.DungeonDifficulty.Beginner,
                    modName,
                    x1,
                    x2,
                    y1,
                    y2
                );
            }
        }
    }
}
