using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class DivineIntellect : BaseTalent, ITalent
    {
        public DivineIntellect() : base()
        {
            BlockedBy = new Type[] { };
            DisplayName = "Divine intellect";
            Description = "Increases intellect by 2 per level.";
            ImageID = 30034;
        }
        public override void UpdateMobile(Mobile mobile)
        {
            mobile.AddStatMod(new StatMod(StatType.Int, "DivineInt", Level*2, TimeSpan.Zero));
        }
    }
}
