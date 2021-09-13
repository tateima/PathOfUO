using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class DivineStrength : BaseTalent, ITalent
    {
        public DivineStrength() : base()
        {
            DisplayName = "Divine strength";
            Description = "Increases strength by 2 per level.";
            ImageID = 166;
            GumpHeight = 70;
            AddEndY = 65;
        }
        public override void UpdateMobile(Mobile mobile)
        {
            mobile.RemoveStatMod("DivineStr");
            mobile.AddStatMod(new StatMod(StatType.Str, "DivineStr", Level*2, TimeSpan.Zero));
        }
    }
}
