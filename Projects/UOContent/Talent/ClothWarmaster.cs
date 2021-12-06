using System;
using Server.Items;

namespace Server.Talent
{
    public class ClothWarmaster : BaseTalent
    {
        public ClothWarmaster()
        {
            DisplayName = "Cloth warmaster";
            Description = "Reduces damage while wearing cloth. Increases Int by 2 per Level";
            ImageID = 395;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            if (BaseArmor.FullChain(mobile) || BaseArmor.FullRing(mobile))
            {
                mobile.RemoveStatMod("ClothWarmaster");
                mobile.RemoveStatMod("ClothWarmaster");
                mobile.AddStatMod(new StatMod(StatType.Int, "ClothWarmaster", Level * 2, TimeSpan.Zero));
            }
        }
    }
}
