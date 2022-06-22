using System;

namespace Server.Talent
{
    public class DivineStrength : BaseTalent
    {
        public DivineStrength()
        {
            StatModNames = new[] { "DivineStr" };
            DisplayName = "Divine strength";
            Description = "Increases strength by 2 per level.";
            ImageID = 166;
            GumpHeight = 70;
            AddEndY = 65;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            ResetMobileMods(mobile);
            mobile.AddStatMod(new StatMod(StatType.Str, StatModNames[0], Level * 2, TimeSpan.Zero));
        }
    }
}
