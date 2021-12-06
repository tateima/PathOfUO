using System;
using Server.Items;

namespace Server.Talent
{
    public class LeatherWarmaster : BaseTalent
    {
        public LeatherWarmaster()
        {
            DisplayName = "Leather warmaster";
            Description = "Reduces damage while wearing leather. Increases Dex by 2 per Level";
            ImageID = 386;
            GumpHeight = 85;
            AddEndY = 90;
            UpdateOnEquip = true;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            if (BaseArmor.FullLeather(mobile))
            {
                mobile.RemoveStatMod("LeatherWarmaster");
                mobile.AddStatMod(new StatMod(StatType.Dex, "LeatherWarmaster", Level * 2, TimeSpan.Zero));
            }
        }
    }
}
