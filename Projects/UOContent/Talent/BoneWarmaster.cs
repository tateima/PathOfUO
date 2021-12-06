using System;
using Server.Items;

namespace Server.Talent
{
    public class BoneWarmaster : BaseTalent
    {
        public BoneWarmaster()
        {
            DisplayName = "Bone warmaster";
            Description = "Reduces damage while wearing bone. Increases Int by 1 and Str by 1 per Level";
            ImageID = 396;
            GumpHeight = 70;
            UpdateOnEquip = true;
            AddEndY = 75;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            if (BaseArmor.FullBone(mobile))
            {
                mobile.RemoveStatMod("BoneWarmasterStr");
                mobile.RemoveStatMod("BoneWarmasterInt");
                mobile.AddStatMod(new StatMod(StatType.Str, "BoneWarmasterStr", Level, TimeSpan.Zero));
                mobile.AddStatMod(new StatMod(StatType.Int, "BoneWarmasterInt", Level, TimeSpan.Zero));
            }
        }
    }
}
