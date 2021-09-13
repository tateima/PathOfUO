using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class DivineDexterity : BaseTalent, ITalent
    {
        public DivineDexterity() : base()
        {
            BlockedBy = new Type[] { };
            DisplayName = "Divine dexterity";
            Description = "Increases dexterity by 2 per level.";
            ImageID = 147;
            GumpHeight = 70;
            AddEndY = 65;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            mobile.RemoveStatMod("DivineDex");
            mobile.AddStatMod(new StatMod(StatType.Dex, "DivineDex", Level*2, TimeSpan.Zero));
        }


    }
}
