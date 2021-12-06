using System;

namespace Server.Talent
{
    public class DivineIntellect : BaseTalent
    {
        public DivineIntellect()
        {
            BlockedBy = Array.Empty<Type>();
            DisplayName = "Divine intellect";
            Description = "Increases intellect by 2 per level.";
            ImageID = 132;
            GumpHeight = 70;
            AddEndY = 55;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            mobile.RemoveStatMod("DivineInt");
            mobile.AddStatMod(new StatMod(StatType.All, "DivineInt", Level * 2, TimeSpan.Zero));
        }
    }
}
