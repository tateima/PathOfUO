using System;

namespace Server.Talent
{
    public class DivineDexterity : BaseTalent
    {
        public DivineDexterity()
        {
            StatModNames = new[] { "DivineDex" };
            BlockedBy = Array.Empty<Type>();
            DisplayName = "Divine dexterity";
            Description = "Increases dexterity by 2 per level.";
            ImageID = 147;
            GumpHeight = 70;
            AddEndY = 65;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            ResetMobileMods(mobile);
            mobile.AddStatMod(new StatMod(StatType.Dex, StatModNames[0], Level * 2, TimeSpan.Zero));
        }
    }
}
