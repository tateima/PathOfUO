using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class DivineStrength : BaseTalent, ITalent
    {
        public DivineStrength() : base()
        {
            BlockedBy = new Type[] { };
            DisplayName = "Divine strength";
            Description = "Increases strength by 2 per level.";
            ImageID = 39870;
        }
        public override void UpdateMobile(Mobile mobile)
        {
            mobile.AddStatMod(new StatMod(StatType.Str, "DivineStr", Level*2, TimeSpan.Zero));
        }
    }
}
