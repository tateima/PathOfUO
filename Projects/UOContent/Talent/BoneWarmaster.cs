using System;
using Server.Items;

namespace Server.Talent
{
    public class BoneWarmaster : BaseTalent
    {
        public BoneWarmaster()
        {
            StatModNames = new[] { "BoneWarmasterStr", "BoneWarmasterInt" };
            DisplayName = "Bone warmaster";
            Description = "Reduces damage while wearing bone. Increases Int by 2 and Str by 2 per Level";
            AdditionalDetail = $"{PassiveDetail}";
            ImageID = 396;
            GumpHeight = 70;
            UpdateOnEquip = true;
            AddEndY = 75;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            ResetMobileMods(mobile);
            if (Items.BaseArmor.FullBone(mobile))
            {
                mobile.AddStatMod(new StatMod(StatType.Str, StatModNames[0], Level * 2, TimeSpan.Zero));
                mobile.AddStatMod(new StatMod(StatType.Int, StatModNames[1], Level * 2, TimeSpan.Zero));
            }
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (Items.BaseArmor.FullBone(defender)) {
                damage -= Level;
            }
            return damage;
        }
    }
}
